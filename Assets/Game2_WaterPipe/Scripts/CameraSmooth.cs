using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CityTycoon
{
    public class CameraSmooth : MonoBehaviour
    {
        public float dragSpeed = 2.0f;
        public float zoomSpeed = 2.0f;
        public float minZoom = 5.0f;
        public float maxZoom = 50.0f;
        public Rect moveArea;
        public SpriteRenderer moveAreaObj;
        public LayerMask dragLayerMask; // Add this

        private Vector3 dragOrigin;
        public bool isDragging { get; private set; } = false;

        private bool isPinching = false;
        private float initialPinchDistance;
        private float initialZoom;

        private Camera cam;

        void Start()
        {
            cam = GetComponent<Camera>();
            CalculateArea();
        }

        void Update()
        {
            HandleDrag();
            HandleZoom();
        }

        void HandleDrag()
        {
            // ลากด้วยเมาส์
            if (Input.GetMouseButtonDown(0))
            {
                // ตรวจสอบว่าไม่ใช่การคลิกบน UI หรือเกมอ็อบเจ็กต์
                if (!IsPointerOverGameObject())
                {
                    dragOrigin = Input.mousePosition;
                    isDragging = true;
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                isDragging = false;
            }

            // ลากด้วยนิ้ว
            if (Input.touchCount == 1)
            {
                if (isPinching) return;

                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                {
                    if (!IsPointerOverGameObject())
                    {
                        dragOrigin = touch.position;
                        isDragging = true;
                    }
                }

                if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                {
                    isDragging = false;
                }

                if (isDragging && touch.phase == TouchPhase.Moved)
                {
                    Vector3 difference = Camera.main.ScreenToWorldPoint(touch.position) - Camera.main.ScreenToWorldPoint(dragOrigin);
                    Vector3 newPosition = cam.transform.position - difference;
                    float halfHeight = cam.orthographicSize;
                    float halfWidth = cam.aspect * halfHeight;
                    newPosition.x = Mathf.Clamp(newPosition.x, moveArea.x + halfWidth, moveArea.width - halfWidth);
                    newPosition.y = Mathf.Clamp(newPosition.y, moveArea.y + halfHeight, moveArea.height - halfHeight);
                    cam.transform.position = newPosition;
                    dragOrigin = touch.position;
                }
            }

            // ลากด้วยเมาส์ต่อเมื่อยังอยู่ในสถานะการลาก
            if (isDragging && Input.GetMouseButton(0))
            {
                Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - Camera.main.ScreenToWorldPoint(dragOrigin);
                Vector3 newPosition = cam.transform.position - difference;
                float halfHeight = cam.orthographicSize;
                float halfWidth = cam.aspect * halfHeight;
                newPosition.x = Mathf.Clamp(newPosition.x, moveArea.x + halfWidth, moveArea.width - halfWidth);
                newPosition.y = Mathf.Clamp(newPosition.y, moveArea.y + halfHeight, moveArea.height - halfHeight);
                cam.transform.position = newPosition;
                dragOrigin = Input.mousePosition;
            }
        }

        void HandleZoom()
        {
            float size = Camera.main.orthographicSize;

            // ตรวจจับการเลื่อนเมาส์
            if (Input.GetAxis("Mouse ScrollWheel") != 0f)
            {
                float scroll = Input.GetAxis("Mouse ScrollWheel");
                size -= scroll * zoomSpeed;
                size = Mathf.Clamp(size, minZoom, maxZoom);
                Camera.main.orthographicSize = size;
                Vector3 newPosition = cam.transform.position;
                float halfHeight = cam.orthographicSize;
                float halfWidth = cam.aspect * halfHeight;
                newPosition.x = Mathf.Clamp(newPosition.x, moveArea.x + halfWidth, moveArea.width - halfWidth);
                newPosition.y = Mathf.Clamp(newPosition.y, moveArea.y + halfHeight, moveArea.height - halfHeight);
                cam.transform.position = newPosition;
            }

            // ตรวจจับการสัมผัสสองจุด
            if (Input.touchCount == 2)
            {
                Touch touchZero = Input.GetTouch(0);
                Touch touchOne = Input.GetTouch(1);

                if (!isPinching)
                {
                    // เริ่มต้นการซูม
                    isPinching = true;
                    initialPinchDistance = Vector2.Distance(touchZero.position, touchOne.position);
                    initialZoom = Camera.main.orthographicSize;
                }
                else
                {
                    // คำนวณระยะห่างปัจจุบัน
                    float currentPinchDistance = Vector2.Distance(touchZero.position, touchOne.position);
                    float pinchRatio = initialPinchDistance / currentPinchDistance;

                    // ปรับขนาดกล้อง
                    Camera.main.orthographicSize = Mathf.Clamp(initialZoom * pinchRatio, minZoom, maxZoom);
                    Vector3 newPosition = cam.transform.position;
                    float halfHeight = cam.orthographicSize;
                    float halfWidth = cam.aspect * halfHeight;
                    newPosition.x = Mathf.Clamp(newPosition.x, moveArea.x + halfWidth, moveArea.width - halfWidth);
                    newPosition.y = Mathf.Clamp(newPosition.y, moveArea.y + halfHeight, moveArea.height - halfHeight);
                    cam.transform.position = newPosition;
                }
            }
            else
            {
                // รีเซ็ตการซูม
                isPinching = false;
            }
        }

        bool IsPointerOverGameObject()
        {
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

            foreach (RaycastResult result in results)
            {
                if (result.gameObject.tag != "GamePlay")
                {
                    return true;
                }
            }
            return false;
        }

        public void SetMoveArea(SpriteRenderer newArea)
        {
            moveAreaObj = newArea;
            CalculateArea();
        }

        private void CalculateArea()
        {
            moveArea.x = moveAreaObj.bounds.min.x;
            moveArea.y = moveAreaObj.bounds.min.y;
            moveArea.width = moveAreaObj.bounds.max.x;
            moveArea.height = moveAreaObj.bounds.max.y;
        }
    }
}
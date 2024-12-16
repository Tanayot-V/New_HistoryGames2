using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    public GridSystem gridManager;
    public GameObject objectPrefab; // ไอเทมที่จะวาง
    public bool isPlaceObject;
    private bool isPlacing = false; // สถานะการวาง

    private GameObject currentObject;

    private void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.Space)) // คลิกซ้าย
        {
            PlaceObject();
        }*/
        
        // ตรวจสอบการคลิกเมาส์
        if (Input.GetMouseButtonDown(0)) // คลิกซ้าย
        {
            PlaceObject();
        }
        else if (Input.GetMouseButtonDown(1) && currentObject != null && !isPlaceObject) // คลิกขวา
        {
            Destroy(currentObject); // ลบไอเทมที่เลือก
        }


        // หากกำลังวาง ให้ติดตามเมาส์
        if (isPlacing && currentObject != null)
        {
            FollowMouse();
        }
    }

    private void PlaceObject()
    {
        // หาเมาส์ในตำแหน่งโลก
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;

        // Snap ตำแหน่งไปที่ Grid
        Vector3 snappedPosition = gridManager.SnapToGrid(mousePosition);

        if (isPlaceObject)
        {
            // สร้างไอเท็มใหม่เสมอเมื่อ isPlaceObject เป็น true
            Instantiate(objectPrefab, snappedPosition, Quaternion.identity);
        }
        else
        {
            if (currentObject == null)
            {
                // สร้างไอเท็มใหม่ครั้งแรก
                currentObject = Instantiate(objectPrefab, snappedPosition, Quaternion.identity);
            }
            else
            {
                // ย้ายไอเท็มไปตำแหน่งใหม่
                currentObject.transform.position = snappedPosition;
            }
        }
    }

    private void FollowMouse()
    {
        // ตำแหน่งเมาส์ในโลก
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;

        // Snap ตำแหน่งให้ตรงกับ Grid
        Vector3 snappedPosition = gridManager.SnapToGrid(mousePosition);

        // ย้าย Prefab ไปยังตำแหน่งที่ Snap
        currentObject.transform.position = snappedPosition;
    }

}

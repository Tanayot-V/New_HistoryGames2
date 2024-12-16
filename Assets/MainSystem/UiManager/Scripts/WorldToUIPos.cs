using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldToUIPos : MonoBehaviour
{
    public Transform targetTransform;
    public float offsetX = 0f; // Offset ในแนวนอน
    public float offsetY = 200f; // Offset ในแนวตั้ง
    private float offsetMultiplier;
    private float startCamSize = -1f;
    private Camera mCam;

    void Start()
    {
        mCam = Camera.main;
        if(startCamSize == -1f) startCamSize = mCam.orthographicSize;
    }

    void Update()
    {
        SetImagePositionToTransformWorld();
    }

    void SetImagePositionToTransformWorld()
    {
        if (targetTransform != null)
        {
            // คำนวณตำแหน่งจอภาพจากตำแหน่ง World
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(targetTransform.position);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                this.GetComponent<RectTransform>().parent as RectTransform,
                screenPosition,
                Camera.main,
                out Vector2 localPoint);

            // คำนวณ offsetMultiplier
            offsetMultiplier = startCamSize / mCam.orthographicSize;

            // เพิ่ม OffsetX และ OffsetY
            localPoint.x += offsetX * offsetMultiplier;
            localPoint.y += offsetY * offsetMultiplier;

            // อัปเดตตำแหน่งของ RectTransform
            this.GetComponent<RectTransform>().localPosition = localPoint;
        }
    }
}

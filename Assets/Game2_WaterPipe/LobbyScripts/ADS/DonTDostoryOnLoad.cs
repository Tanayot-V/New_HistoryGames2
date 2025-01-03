using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DonDostoryOnLoad : MonoBehaviour
{
    private void Awake()
    {
        // เช็คว่ามี instance ของสคริปต์นี้อยู่แล้วหรือยัง
        if (FindObjectsOfType<DonDostoryOnLoad>().Length > 1)
        {
            Destroy(this.gameObject); // ถ้ามีอยู่แล้ว ให้ทำลายตัวเอง
            return;
        }

        DontDestroyOnLoad(this.gameObject); // ถ้ายังไม่มี ให้ทำตัวเองคงอยู่ในทุก Scene
    }
}

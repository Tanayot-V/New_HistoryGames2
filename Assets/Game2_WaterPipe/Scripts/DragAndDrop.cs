using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class DragAndDrop : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject prefab;
    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("OnBeginDrag: " + name);
        //สร้าง Object
        prefab = GameManager.Instance.PipeManager().CreatePipeSlotDangDrop();
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("OnDrag: " + name);
        //Object ตามเมาส์

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("OnEndDrag: " + name);
        if (prefab != null) prefab.GetComponent<PipeSlotDragDrop>().OnMouseUpSlot();
    }
}

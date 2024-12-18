using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class DragAndDropUISlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public PipeData pipeData;
    public GameObject prefab;
    public SpriteRenderer pipeSR;

    public void OnBeginDrag(PointerEventData eventData)
    {
        //Debug.Log("OnBeginDrag: " + name);
        //สร้าง Object
        prefab = GameManager.Instance.CreatePipeSlotDragDropUI(pipeData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log("OnDrag: " + name);
        //Object ตามเมาส์

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //Debug.Log("OnEndDrag: " + name);
        //if (prefab != null) prefab.GetComponent<PipeSlotDragDrop>().OnMouseUpSlot();
    }
}

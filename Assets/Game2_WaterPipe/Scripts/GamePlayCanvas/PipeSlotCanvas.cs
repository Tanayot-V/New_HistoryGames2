using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PipeSlotCanvas : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public bool isDefault = false;
    public Vector2 pos;
    public GameObject item;
    private Image childImage;
    public Color heightLightColor;
    public Color defaultColor;
    public Color disableColor;

    void Start()
    {
        childImage = transform.GetChild(0).GetComponent<Image>();
        defaultColor = childImage.color;
        if(transform.childCount > 1)
        {
            item = transform.GetChild(1).gameObject;
            isDefault = true;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            if(item != null) return;
            DraggableItem draggableItem = eventData.pointerDrag.GetComponent<DraggableItem>();
            if(draggableItem.isSnapOnSlot) return;
            if (draggableItem != null)
            {
                draggableItem.SnapToSlot();
                draggableItem.transform.SetParent(transform);
                draggableItem.transform.localPosition = Vector3.zero;
            }
            PipeObject pipeObject = eventData.pointerDrag.GetComponent<PipeObject>();
            if (pipeObject != null)
            {
                pipeObject.pipeData.pos = pos;
                GameManager.Instance.UpdatePipeSlotTOList(pipeObject.pipeData.pos, pipeObject.pipeData);
            }
            item = eventData.pointerDrag;
            GameManager.Instance.UseMove();
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null) return;
        // Change the color of the child image to indicate hover
        if (childImage != null)
        {
            if(item != null)
            {
                childImage.color =  disableColor;// Change to your desired color
            }
            else
            {
                childImage.color =  heightLightColor;// Change to your desired color
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Revert the color of the child image to the original color
        if (childImage != null)
        {
            childImage.color = defaultColor; // Change to the original color
        }
    }
}

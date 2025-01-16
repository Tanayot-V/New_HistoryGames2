using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PipeSlotCanvas : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public bool isDefault = false;
    public Vector2 pos;
    public GameObject item;
    private Image childImage;
    public Color heightLightColor;
    public Color defaultColor;
    public Color disableColor;

    public GameObject heightLightItemObject;

    [Header("Road")]
    public bool isDefaultRoad = false;
    public GameObject roadObject;
    private Image slotImg;

    void Start()
    {
        
        childImage = transform.GetChild(0).GetComponent<Image>();
        defaultColor = childImage.color;
        heightLightItemObject = transform.GetChild(1).gameObject;
        if(transform.childCount > 2)
        {
            item = transform.GetChild(2).gameObject;
            isDefault = true;
        }
        slotImg = GetComponent<Image>();
        if(isDefaultRoad)
        {
            slotImg.color = new Color(0,0.5f,1,0.75f);
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            PipeObject pipeObject = eventData.pointerDrag.GetComponent<PipeObject>();
            DraggableItem draggableItem = eventData.pointerDrag.GetComponent<DraggableItem>();
            
            if(pipeObject.pipeData.pipeType == PipeType.Road)
            {
                if(roadObject != null) return;
                roadObject = eventData.pointerDrag;
                Obstacle obstacle = eventData.pointerDrag.GetComponent<Obstacle>();
                obstacle.slot = this;
            }
            else
            {
                if(item != null) return;
                item = eventData.pointerDrag;
            }

            if (draggableItem != null)
            {
                if(draggableItem.isSnapOnSlot) return;
                draggableItem.SnapToSlot();
                draggableItem.transform.SetParent(transform);
                draggableItem.transform.localPosition = Vector3.zero;
            }
            if (pipeObject != null)
            {
                pipeObject.pipeData.pos = pos;
                GameManager.Instance.UpdatePipeSlotTOList(pipeObject.pipeData.pos, pipeObject.pipeData);
            }
            GameManager.Instance.UseMove(draggableItem.placeCount);
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (childImage != null && GameManager.Instance.itemManager.isDrawLine)
        {
            childImage.color =  heightLightColor;// Change to your desired color
        }
        if (eventData.pointerDrag == null) 
        {
            return;
        }
        // Change the color of the child image to indicate hover
        if (childImage != null)
        {
            if(item != null)
            {
                childImage.color = disableColor;// Change to your desired color
            }
            else
            {
                childImage.color = heightLightColor;// Change to your desired color
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

    public void OnPointerClick(PointerEventData eventData)
    {
        if(GameManager.Instance.itemManager.isDrawLine)
        {
            if( heightLightItemObject != null)
            {
                heightLightItemObject.SetActive(!heightLightItemObject.activeSelf);
            }
        }
    }
}

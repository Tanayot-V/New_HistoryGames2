using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Obstacle : MonoBehaviour, IPointerClickHandler
{
    public PipeObject pipeObject;
    public PipeSlotCanvas slot;
    // Start is called before the first frame update
    void Start()
    {
        pipeObject = GetComponent<PipeObject>();
        if(pipeObject.pipeData.pipeType != PipeType.Road)
        {
            slot = GetComponentInParent<PipeSlotCanvas>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        //Open minigame
        
        //Mini game Callback
        if(pipeObject.pipeData.pipeType == PipeType.Road)
        {
            slot.roadObject = null;
        }
        else
        {
            slot.item = null;
            slot.isDefault = false;
        }
        Destroy(gameObject);

    }
}

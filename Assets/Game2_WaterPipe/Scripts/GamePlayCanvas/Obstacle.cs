using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Obstacle : MonoBehaviour, IPointerClickHandler
{
    public PipeObject pipeObject;
    private PipeSlotCanvas slot;
    // Start is called before the first frame update
    void Start()
    {
        pipeObject = GetComponent<PipeObject>();
        slot = GetComponentInParent<PipeSlotCanvas>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        slot.item = null;
        //Open minigame

        //Mini game Callback
        Destroy(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Obstacle : MonoBehaviour, IPointerClickHandler
{
    public PipeObject pipeObject;
    public PipeSlotCanvas slot;

    void Start()
    {
        pipeObject = GetComponent<PipeObject>();
        if(pipeObject.pipeData.pipeType != PipeType.Road)
        {
            slot = GetComponentInParent<PipeSlotCanvas>();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(GameManager.Instance.isGameEnd || !GameManager.Instance.isStartGame || GameManager.Instance.isRunWater) return;
        GameManager.Instance.gameUiManager.ShowMinigame(gameObject.name.Substring(0,5), (isFinish) =>
        {
            if(isFinish)
            {
                DestroyObstacle();
            }
        });
    }

    public void DestroyObstacle()
    {
        if(pipeObject.pipeData.pipeType == PipeType.Road)
        {
            slot.roadObject = null;
        }
        else
        {
            slot.item = null;
            slot.isDefault = false;
        }
        DestroyImmediate(gameObject);
    }
}

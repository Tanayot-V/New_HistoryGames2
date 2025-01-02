using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemManager : MonoBehaviour
{
    public int addTimeCount = 5;
    public int drawLineCount = 5;
    public int hammerCount = 5;

    [Header("Add Time")]
    public float timeToAdd = 15f;

    [Header("Draw Line")]
    public bool isDrawLine = false;
    public float drawLineTime = 15f;
    private float drawLineTimer;


    [Header("Hammer")]
    public bool onHammer;
    public GraphicRaycaster uiRaycaster;
    public EventSystem eventSystem;
    
    void Update()
    {
        if(onHammer)
        {
            if (Input.GetMouseButtonDown(0))
            {
                CheckClick();
            }
        }
        if(isDrawLine)
        {
            drawLineTimer -= Time.deltaTime;
            GameManager.Instance.gameUiManager.UpdateDrawTimer(drawLineTimer / drawLineTime);
            if(drawLineTimer <= 0)
            {
                isDrawLine = false;
                GameManager.Instance.gameUiManager.OnEndDrawLine();
            }
        }
    }

    public void addtimeBtnClick()
    {
        if (addTimeCount <= 0) return;
        GameManager.Instance.AddTime(timeToAdd);
        addTimeCount--;
        GameManager.Instance.gameUiManager.UpdateAddTimeCount(addTimeCount);
    }

    public void DrawLineBtnClick()
    {
        if (drawLineCount <= 0)
        {
            GameManager.Instance.gameUiManager.OnEndDrawLine();
            return;
        }
        // Draw Line logic
        isDrawLine = true;
        drawLineTimer = drawLineTime;
    }


    public void HammerBtnClick()
    {
        if (hammerCount <= 0)
        {
            GameManager.Instance.gameUiManager.OnEndHammer();
            return;
        } 
        if (hammerCount <= 0) return;
        // Hammer logic
        onHammer = true;
    }

    public void UseHammerComplete()
    {
        hammerCount--;
        GameManager.Instance.gameUiManager.UpdateHammerCount(hammerCount);
        onHammer = false;
        GameManager.Instance.gameUiManager.OnEndHammer();
    }

    void CheckClick()
    {
        PointerEventData pointerEventData = new PointerEventData(eventSystem);
        pointerEventData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        uiRaycaster.Raycast(pointerEventData, results);
        
        bool clickedOnPipeObject = false;

       foreach (RaycastResult result in results)
        {
            PipeObject pipeObject = result.gameObject.GetComponent<PipeObject>();
            if (pipeObject != null)
            {
                clickedOnPipeObject = true;
                break;
            }
        }

        if (!clickedOnPipeObject)
        {
            Debug.Log("Clicked on something other than a PipeObject or empty space");
            onHammer = false;
            GameManager.Instance.gameUiManager.OnEndHammer();
        }
    }
}

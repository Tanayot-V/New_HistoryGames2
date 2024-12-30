using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameManager : Singletons<GameManager>
{
    public PipeManager pipeManager;
    public GameUiManager gameUiManager;
    public GridCanvas gridCanvas;

    [Header("Game Data")]
    public float timePlay = 600;
    private float timer;
    public int moveCount = 20;
    public int hammerCount = 5;

    public bool onHammer;
    public GraphicRaycaster uiRaycaster;
    public EventSystem eventSystem;

    public void Start()
    {
        timer = timePlay;
        gameUiManager.Init(1f, moveCount, hammerCount);
    }

    void Update()
    {
        if (timer <= 0)
        {
            // lose game
            return;
        }
        timer -= Time.deltaTime;
        gameUiManager.UpdateTime(timer / timePlay);
        if(onHammer)
        {
            if (Input.GetMouseButtonDown(0))
            {
                CheckClick();
            }
        }
    }

    public void UpdatePipeSlotTOList(Vector2 _pos, PipeData _pipeData)
    {
        pipeManager.UpdatePipeSlotTOList(_pos, _pipeData);
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
            gameUiManager.OnEndHammer();
        }
    }

    public void UseHammer()
    {
        if (hammerCount <= 0) return;
        // Hammer logic
        onHammer = true;
    }

    public void UseHammerComplete()
    {
        hammerCount--;
        gameUiManager.UpdateHammerCount(hammerCount);
        onHammer = false;
        gameUiManager.OnEndHammer();
    }

    public bool UseMove()
    {
        if (moveCount <= 0)
        {
            // lose game

            return false;
        } 
        moveCount--;
        gameUiManager.UpdateMoveCount(moveCount);
        return true;
    }
}

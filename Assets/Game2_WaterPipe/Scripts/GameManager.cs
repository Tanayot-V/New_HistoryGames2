using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Assertions.Must;
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
    public int moveCount = 0;
    public int hammerCount = 5;

    public bool onHammer;
    public GraphicRaycaster uiRaycaster;
    public EventSystem eventSystem;

    public List<PipeStart> pipeStarts = new List<PipeStart>();
    public List<PipeEnd> pipeEnds = new List<PipeEnd>();

    public float runTimer = 2f;
    public bool isRunWater = false;

    public bool isGameEnd = false;

    public void Start()
    {
        timer = timePlay;
        pipeStarts.Clear();
        pipeEnds.Clear();
        gameUiManager.Init(1f, moveCount, hammerCount);
        pipeStarts.AddRange(FindObjectsOfType<PipeStart>());
        pipeEnds.AddRange(FindObjectsOfType<PipeEnd>());
    }

    void Update()
    {
        if(isGameEnd) return;
        
        if (timer <= 0)
        {
            // lose game
            StartCoroutine(EndGame(false, LoseCondition.timeOut));
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

        if(isRunWater)
        {
            runTimer -= Time.deltaTime;
            if(runTimer <= 0)
            {
                isRunWater = false;
                EndGameCheck();
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

    public bool UseMove(int count)
    {
        moveCount += count;
        gameUiManager.UpdateMoveCount(moveCount);
        return true;
    }

    public void StartRunWater()
    {
        pipeStarts.ForEach(x =>
        {
            if(!x.isWaitWaterIn)
                x.RunWater();
        });
        isRunWater = true;
    }

    public void RunningWater()
    {
        isRunWater = true;
        runTimer = 2f;
    }

    public void EndGameCheck()
    {
        if (pipeEnds.TrueForAll(x => x.isFinish))
        {
            if(!gridCanvas.CheckRoad())
            {
                // lose game
                StartCoroutine(EndGame(false, LoseCondition.RoadNonComplete));
                return;
            }
            // win game
            StartCoroutine(EndGame(true, LoseCondition.PipeNonComplete));
        }
        else
        {
            // lose game
            StartCoroutine(EndGame(false, LoseCondition.PipeNonComplete));
        }
    }

    private IEnumerator EndGame(bool isWin,LoseCondition loseCondition)
    {
        if(isGameEnd) yield break;
        isGameEnd = true;
        if(isWin)
        {
            gridCanvas.SaveState(gridCanvas.savelevelName);
        }
        yield return new WaitForSeconds(1f);
        gameUiManager.ShowResult(isWin,loseCondition);
    }
}

public enum LoseCondition
{
    timeOut,
    PipeNonComplete,
    RoadNonComplete,
    WasteNonComplete
}

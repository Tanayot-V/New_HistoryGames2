using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameManager : Singletons<GameManager>
{
    public bool isDebug = false;
    public int debugStarCount = 3;
    [Space(20)]
    public PipeManager pipeManager;
    public GameUiManager gameUiManager;
    public GridCanvas gridCanvas;
    public ItemManager itemManager;

    [Header("Game Data")]
    public bool isStartGame = false;
    public float timePlay = 600;
    private float timer;
    public int moveCount = 0;

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
        gameUiManager.Init(1f, moveCount, itemManager.addTimeCount, itemManager.drawLineCount, itemManager.hammerCount);
        pipeStarts.AddRange(FindObjectsOfType<PipeStart>());
        pipeEnds.AddRange(FindObjectsOfType<PipeEnd>());
    }

    public void StartGame()
    {
        isStartGame = true;
        gameUiManager.buttomBar.SetActive(true);
        gameUiManager.letsDoItBtn.SetActive(false);
    }

    void Update()
    {
        if(isGameEnd || !isStartGame) return;

        if (timer <= 0)
        {
            // lose game
            StartCoroutine(EndGame(false, LoseCondition.timeOut));
            return;
        }

        if(isRunWater)
        {
            runTimer -= Time.deltaTime;
            if(runTimer <= 0)
            {
                //isRunWater = false;
                EndGameCheck();
            }
        }
        else
        {
            timer -= Time.deltaTime;
            gameUiManager.UpdateTime(timer / timePlay);
        }
    }

    public void AddTime(float time)
    {
        timer += time;
        gameUiManager.UpdateTime(timer / timePlay);
    }

    public void UpdatePipeSlotTOList(Vector2 _pos, PipeData _pipeData)
    {
        pipeManager.UpdatePipeSlotTOList(_pos, _pipeData);
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
        if(isDebug)
        {
            yield return new WaitForSeconds(1f);
            gameUiManager.ShowResult(true,loseCondition,debugStarCount);
        }
        else
        {
            if(isWin)
            {
                gridCanvas.SaveState(gridCanvas.savelevelName);
            }
            yield return new WaitForSeconds(1f);
            gameUiManager.ShowResult(isWin,loseCondition,Random.Range(1,4));
        }
    }
}

public enum LoseCondition
{
    timeOut,
    PipeNonComplete,
    RoadNonComplete,
    WasteNonComplete
}

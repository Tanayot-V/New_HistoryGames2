using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public int level = 1;
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

        if(level == 3)
        {
            AdsManager.Instance.OpenAdsVideo(AdsType.EndStage4);
        }
    }

    public void StartGame()
    {
        isStartGame = true;
        gameUiManager.buttomBar.SetActive(true);
        gameUiManager.letsDoItBtn.SetActive(false);
    }

    void Update()
    {
        if(isGameEnd || !isStartGame || itemManager.isDrawLine) return;

        if (timer <= 0)
        {
            if(!PlayerPrefs.HasKey("TimeOutVideo"))
            {
                AdsManager.Instance.OpenAdsVideo(AdsType.MoveOut, () => {StartCoroutine(EndGame(false, LoseCondition.timeOut));});
                PlayerPrefs.SetInt("TimeOutVideo",0);
                return;
            }
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
            // if(level == 2)
            // {
            //     AdsManager.Instance.OpenAdsVideo(AdsType.EndStage2, ()=>{
            //         StartCoroutine(EndGame(true, LoseCondition.PipeNonComplete));
            //     });
            //     return;
            // }
            else if(level == 4)
            {
                AdsManager.Instance.OpenAdsVideo(AdsType.WastWaterGood, ()=>{
                    StartCoroutine(EndGame(true, LoseCondition.PipeNonComplete));
                });
                return;
            }
            else if(level == 9)
            {
                AdsManager.Instance.OpenAdsVideo(AdsType.EndStage9, ()=>{
                    StartCoroutine(EndGame(true, LoseCondition.PipeNonComplete));
                });
                return;
            }
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
        if(gridCanvas.isChallangeMode)
        {
            if(loseCondition == LoseCondition.timeOut)
            {
                gameUiManager.ShowResult(isWin,loseCondition,3);
                yield break;
            }
            gridCanvas.ClearGrid();
            gridCanvas.CreateGrid();
            gridCanvas.RandomPipe();
            isGameEnd = false;
            isRunWater = false;
            yield break;
        }
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
            
            int star = CalculateStar(false, timePlay - timer, timePlay, moveCount, pipeEnds.Count * 10);
            
            int wasteStart = pipeStarts.Count(x => x.isWasteWater == true);
            int score = CalculateScoreLeaderbaord((int)timePlay, gridCanvas.rows * gridCanvas.columns, pipeEnds.Count, wasteStart, (int)(timePlay - timer), moveCount, level);
            
            LevelDataManager.Instance.SetLevelData(level,star,score);
            LevelDataManager.Instance.SetCurrentLevel(level + 1);

            gameUiManager.ShowResult(isWin,loseCondition,star);
            
        }
    }

    #region CalculateStarAndScore
    //_isFixAgain = กลับมาแก้ไขอีกครั้ง _usedTime = เวลาที่ใช้  _totalTime = เวลาทั้งหมด  _moveUsed = จำนวนที่ใช้เคลื่อนท่อ  _maxMoves = จำนวนที่กำหนด
    public int CalculateStar(bool _isFixAgain,float _usedTime, float _totalTime, int _moveUsed, int _maxMoves)
    {
        if (_isFixAgain)
        {
            return 1;
        }
        //คำนวณดาว
        float timePercentage = _usedTime / _totalTime;
        int star = 1;
        //ใช้เวลาน้อยกว่า 70% และใช้เคลื่อนไหวน้อยกว่าจำนวนที่กำหนด
        if (timePercentage <= 0.7f && _moveUsed <= _maxMoves)
        {
            star = 3;
        }
        //ใช้เวลาน้อยกว่า 90% ของเวลาทั้งหมด
        else if (timePercentage <= 0.9f)
        {
            star = 2;
        }
        else
        {
            star = 1;
        }
        return star;
    }

    //_totalTimeAllowed = เวลาที่เกมกำหนดให้ _totalGridArea = พื้นที่ในด่าน _pointsToConnectPipes = จุดที่ต้องต่อท่อน้ำเพื่อเชื่อมต่อ _pointsToExitWaste = จุดน้ำเสียที่ต้องต่อท่อน้ำออกจากสถานที่ _playerTimeUsed = ระยะเวลาในการเล่น (วินาที) _playerPipesUsed = จำนวนท่อที่ใช้ _levelState = เลวลที่เล่นอยู่
    public int CalculateScoreLeaderbaord(int _totalTimeAllowed, int _totalGridArea, int _pointsToConnectPipes, int _pointsToExitWaste, int _playerTimeUsed, int _playerPipesUsed, int _levelState)
    {
        int score = _totalTimeAllowed + _totalGridArea + ((_pointsToConnectPipes + _pointsToExitWaste) * 10 * _levelState) - (_playerTimeUsed + (_playerPipesUsed * _levelState));
        Debug.Log($"Score: {score}");
        return score;
    }
    #endregion
}

public enum LoseCondition
{
    timeOut,
    PipeNonComplete,
    RoadNonComplete,
    WasteNonComplete
}

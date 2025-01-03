using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LeaderboardState
{
    public int order;
    public string userID;
    public string displayName;
    public Sprite picture;
    public int score;

    public LeaderboardState(string _userID, string _displayName, Sprite _picture, int _score)
    {
         userID = _userID;
         displayName = _displayName;
         picture = _picture;
         score = _score;
    }
}

public class LeaderboardManager : MonoBehaviour
{
    public LeaderboardDatabaseSO leaderboardDatabaseSO;
    public List<LeaderboardState> leaderbaordStateLists = new List<LeaderboardState>();

    [Header("UI")]
    public GameObject pageOBJ;
    public GameObject prefabSlot;
    public GameObject parent;
    public Sprite[] orders;
    public Sprite[] bgs;

    [Header("Current")]
    public LeaderboardState currentPlayer;
    public LeaderboardSlot currentPlayerSlot;

    [Header("Update Score")]
    public TMPro.TextMeshProUGUI scoreMainTX;
    public TMPro.TextMeshProUGUI scoreDiaryTX;    
    public TMPro.TextMeshProUGUI leaderboradTX;

    public void Start()
    {
        int score = LevelDataManager.Instance.GetLevelData(LevelDataManager.Instance.GetCurrentLevel()).score;
        scoreMainTX.text = score.ToString();
        scoreDiaryTX.text = score.ToString();
        leaderboradTX.text = score.ToString();
    }

    public void InitLeaderboard()
    {
        UiController.Instance.DestorySlot(parent);
        int indexBG = 0;
        leaderbaordStateLists = leaderboardDatabaseSO.leaderboardStatesList;
        leaderbaordStateLists.ForEach(o => {
            GameObject slot = UiController.Instance.InstantiateUIView(prefabSlot, parent);
            LeaderboardSlot leaderboardSlot = slot.GetComponent<LeaderboardSlot>();
            leaderboardSlot.SetupSlot(o);
            if(indexBG == 0) 
            {
                leaderboardSlot.SetBGIMG(bgs[0]);
                indexBG = 1;
            }
            else
            {
                leaderboardSlot.SetBGIMG(bgs[1]);
                indexBG = 0;
            }

            leaderboardSlot.orderTX.gameObject.SetActive(false);
            if(o.order == 1) leaderboardSlot.SetOrderIMG(orders[0]);
            else if(o.order == 2) leaderboardSlot.SetOrderIMG(orders[1]);
            else if(o.order == 3) leaderboardSlot.SetOrderIMG(orders[2]);
            else 
            {
                leaderboardSlot.SetOrderIMG(orders[3]);
                leaderboardSlot.orderTX.gameObject.SetActive(true);
            }
        });
        currentPlayer = leaderboardDatabaseSO.leaderboardState;
        currentPlayerSlot.SetupSlot(currentPlayer);

        int score = LevelDataManager.Instance.GetLevelData(LevelDataManager.Instance.GetCurrentLevel()).score;
        leaderboradTX.text = score.ToString();

    }

    public void OpenPage()
    {
        pageOBJ.SetActive(true);
        InitLeaderboard();
    }

    public void ClosePage()
    {
        pageOBJ.SetActive(false);
        UiController.Instance.DestorySlot(parent);
    }
}

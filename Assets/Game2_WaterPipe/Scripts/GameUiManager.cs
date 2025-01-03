using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Spine.Unity;

public class GameUiManager : MonoBehaviour
{
    public Image timerBar;
    public TMP_Text moveText;
    public TMP_Text hammerCountText;
    public Image hammerBgImage;
    public GameObject hammerAdsImage;
    public TMP_Text drawLineCountText;
    public Image drawLineBgImage;
    public GameObject drawLineAdsImage;
    public TMP_Text addTimeCountText;
    public GameObject addTimeAdsImage;
    public GameObject homeNotiImage;

    public GameObject buttomBar;
    public GameObject letsDoItBtn;

    [Header("Result")]
    public CanvasGroup resultPanel;
    public GameObject winPanel;
    public GameObject losePanel;
    public List<GameObject> loseCases;
    public CanvasGroup underGroundLayer;
    public SkeletonGraphic skeletonGraphic;
    public WheelRewardManager wheelRewardPanel;

    [Header("Minigame")]
    public CanvasGroup minigamePanel;
    public MinigameManager minigameManager;
    public System.Action<bool> minigameCallback;

    [Header("Setting")]
    public SettingManager settingManager;

    [Header("Reset")]
    public GameObject resetPanel;

    [Header("Home")]
    public GameObject homePanel;

    [Header("Loading")]
    public GameObject loadingGO;
    public LoadingFillAmount loadingFillAmount;

    public void StartLoading(System.Action callback)
    {
        loadingGO.SetActive(true);
        loadingFillAmount.StartFillAmount(() =>
        {
            callback();
        });
    }

    public void Init(float timepercent, int moveCount, int addTimeCount, int drawLineCount, int hammerCount)
    {
        if(LevelDataManager.Instance != null)
            homeNotiImage.SetActive(LevelDataManager.Instance.IsAllReadDiary());
        timerBar.fillAmount = timepercent;
        moveText.text = moveCount.ToString();
        addTimeCountText.text = "x" + addTimeCount.ToString();
        drawLineCountText.text = "x" + drawLineCount.ToString();
        hammerCountText.text = "x" + hammerCount.ToString();
    }

    public void UpdateTime(float timepercent)
    {
        timerBar.fillAmount = timepercent;
    }

    public void UpdateMoveCount(int moveCount)
    {
        moveText.text = moveCount.ToString();
    }

    
    public void OnHammerBtnClick()
    {
        GameManager.Instance.itemManager.HammerBtnClick();
        hammerBgImage.color = Color.green;
    }

    public void OnEndHammer()
    {
        hammerBgImage.color = Color.white;
    }

    public void UpdateHammerCount(int count)
    {
        hammerCountText.text = "x" + count.ToString();
        if(count <= 0)
        {
            hammerAdsImage.SetActive(true);
        }
        else
        {
            hammerAdsImage.SetActive(false);
        }
    }

    public void OnDrawLineBtnClick()
    {
        GameManager.Instance.itemManager.DrawLineBtnClick();
        drawLineBgImage.color = Color.green;
    }

    public void OnEndDrawLine()
    {
        drawLineBgImage.color = Color.white;
        drawLineBgImage.fillAmount = 1f;
    }

    public void UpdateDrawTimer(float fillAmount)
    {
        drawLineBgImage.fillAmount = fillAmount;
    }

    public void UpdateDrawLineCount(int count)
    {
        drawLineCountText.text = "x" + count.ToString();
        if(count <= 0)
        {
            drawLineAdsImage.SetActive(true);
        }
        else
        {
            drawLineAdsImage.SetActive(false);
        }
    }

    public void OnAddTimeBtnClick()
    {
        GameManager.Instance.itemManager.addtimeBtnClick();
    }
    public void UpdateAddTimeCount(int count)
    {
        addTimeCountText.text = "x" + count.ToString();
        if(count <= 0)
        {
            addTimeAdsImage.SetActive(true);
        }
        else
        {
            addTimeAdsImage.SetActive(false);
        }
    }

    public void ShowResult(bool isWin, LoseCondition loseCondition, int starCount)
    {
        StartCoroutine(ShowResultCoroutine(isWin, loseCondition, starCount));
    }

    private IEnumerator ShowResultCoroutine(bool isWin,LoseCondition loseCondition, int starCount)
    {
        if(isWin)
        {
            GameManager.Instance.gridCanvas.PrepairEndgame();
            underGroundLayer.alpha = 1f;
            underGroundLayer.blocksRaycasts = true;
            underGroundLayer.interactable = true;
            while(underGroundLayer.alpha > 0)
            {
                underGroundLayer.alpha -= Time.deltaTime * 2f;
                yield return null;
            }
            underGroundLayer.alpha = 0f;
            yield return new WaitForSeconds(2f);
        }
        else
        {
            yield return new WaitForSeconds(2f);
            foreach (var loseCase in loseCases)
            {
                loseCase.SetActive(false);
            }
            loseCases[(int)loseCondition].SetActive(true);
        }

        winPanel.SetActive(isWin);
        losePanel.SetActive(!isWin);
        while (resultPanel.alpha < 1)
        {
            resultPanel.alpha += Time.deltaTime * 2f;
            yield return null;
        }
        if(isWin)
        {
            yield return new WaitForSeconds(0.25f);
            if(starCount > 0)
            {
                skeletonGraphic.AnimationState.SetAnimation(0,starCount + " star_in", false);
                skeletonGraphic.AnimationState.AddAnimation(0,starCount + " star_loop", true, 0);
                skeletonGraphic.freeze = false;
            }
            yield return new WaitForSeconds(1f);
            wheelRewardPanel.gameObject.SetActive(true);
            wheelRewardPanel.Init();
        }
    }

    public void ShowMinigame(string itemId, System.Action<bool> callback)
    {
        minigameCallback = callback;
        minigameManager.SetUP(itemId);
        minigamePanel.alpha = 0;
        minigamePanel.blocksRaycasts = true;
        minigamePanel.interactable = true;
        StopAllCoroutines();
        StartCoroutine(FadeMinigame(true));
    }

    private IEnumerator FadeMinigame(bool isfadein)
    {
        if(isfadein)
        {
            minigamePanel.alpha = 0;
            while (minigamePanel.alpha < 1)
            {
                minigamePanel.alpha += Time.deltaTime * 2f;
                yield return null;
            }
            minigamePanel.alpha = 1;
        }
        else
        {
            minigamePanel.alpha = 1;
            minigamePanel.interactable = false;
            while (minigamePanel.alpha > 0)
            {
                minigamePanel.alpha -= Time.deltaTime  * 2f;
                yield return null;
            }
            minigamePanel.blocksRaycasts = false;
            minigamePanel.alpha = 0;
        }
        yield break;
    }

    public void hideMinigame(bool isfinish)
    {
        StopAllCoroutines();
        StartCoroutine(FadeMinigame(false));
        minigameCallback?.Invoke(isfinish);
    }

    public void OnSettingBtnClick()
    {
        settingManager.OpenPage();
    }

    public void OnResetBtnClick()
    {
        resetPanel.SetActive(true);
    }

    public void OnResetCancelBtnClick()
    {
        resetPanel.SetActive(false);
    }

    public void OnResetConfirmBtnClick()
    {
        int activeSceneIndex = SceneManager.GetActiveScene().buildIndex;
        StartLoading(() =>
        {
            SceneManager.LoadScene(activeSceneIndex);
        });
    }

    public void OnHomeBtnClick()
    {
        homePanel.SetActive(true);
    }

    public void OnHomeCancelBtnClick()
    {
        homePanel.SetActive(false);
    }

    public void OnHomeConfirmBtnClick()
    {
        StartLoading(() =>
        {
            SceneManager.LoadScene(0);
        });   
    }

    public void OnNextLevelBtnClick()
    {
        int activeSceneIndex = SceneManager.GetActiveScene().buildIndex;
        StartLoading(() =>
        {
            SceneManager.LoadScene(activeSceneIndex + 1);
        });
    }
}

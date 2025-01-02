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

    [Header("Result")]
    public CanvasGroup resultPanel;
    public GameObject winPanel;
    public GameObject losePanel;
    public List<GameObject> loseCases;
    public CanvasGroup underGroundLayer;
    public SkeletonGraphic skeletonGraphic;

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

    public void Init(float timepercent, int moveCount, int hammerCount)
    {
        timerBar.fillAmount = timepercent;
        moveText.text = moveCount.ToString();
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

    public void UpdateHammerCount(int hammerCount)
    {
        hammerCountText.text = "x" + hammerCount.ToString();
    }

    public void ShowResult(bool isWin, LoseCondition loseCondition, int starCount)
    {
        StartCoroutine(ShowResultCoroutine(isWin, loseCondition, starCount));
    }

    private IEnumerator ShowResultCoroutine(bool isWin,LoseCondition loseCondition, int starCount)
    {
        if(isWin)
        {
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
        }
    }

    public void OnHammerBtnClick()
    {
        GameManager.Instance.UseHammer();
        hammerBgImage.color = Color.green;
    }

    public void OnEndHammer()
    {
        hammerBgImage.color = Color.white;
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
            while (minigamePanel.alpha > 0)
            {
                minigamePanel.alpha -= Time.deltaTime  * 2f;
                yield return null;
            }
            minigamePanel.alpha = 0;
            minigamePanel.blocksRaycasts = false;
            minigamePanel.interactable = false;
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
        SceneManager.LoadScene(activeSceneIndex);
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
        SceneManager.LoadScene(0);
    }
}

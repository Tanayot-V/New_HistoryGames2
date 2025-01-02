using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    [Header("Minigame")]
    public CanvasGroup minigamePanel;
    public MinigameManager minigameManager;
    public System.Action<bool> minigameCallback;

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

    public void ShowResult(bool isWin, LoseCondition loseCondition)
    {
        StartCoroutine(ShowResultCoroutine(isWin,loseCondition));
    }

    private IEnumerator ShowResultCoroutine(bool isWin,LoseCondition loseCondition)
    {
        winPanel.SetActive(isWin);
        losePanel.SetActive(!isWin);
        if(!isWin)
        {
            foreach (var loseCase in loseCases)
            {
                loseCase.SetActive(false);
            }
            loseCases[(int)loseCondition].SetActive(true);
        }
        while (resultPanel.alpha < 1)
        {
            resultPanel.alpha += Time.deltaTime;
            yield return null;
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
}

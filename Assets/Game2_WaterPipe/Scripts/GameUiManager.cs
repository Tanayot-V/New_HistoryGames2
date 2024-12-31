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
}

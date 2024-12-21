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

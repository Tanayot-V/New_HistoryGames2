using System.Collections;
using System.Collections.Generic;
using JSG.FortuneSpinWheel;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WheelRewardManager : MonoBehaviour
{
    public bool isSpin = false;
    public bool isQuiz = false;
    
    public FortuneSpinWheel fortuneSpinWheel;

    public GameObject quizPanel;

    public TMP_Text spinBntText;
    public string spinText, quizText;

    public void Init()
    {
        isSpin = false;
        isQuiz = false;
        spinBntText.text = spinText;
    }

    public void SpinWheel()
    {
        if(isSpin)
        {
            //quiz
            ShowQuiz();
            return;
        }
        isSpin = true;
        fortuneSpinWheel.StartSpin();
        spinBntText.text = quizText;
    }

    public void HideRewardPanel()
    {
        fortuneSpinWheel.m_RewardPanel.gameObject.SetActive(false);
        if(isQuiz)
        {
            gameObject.SetActive(false);
        }
    }

    public void ShowQuiz()
    {
        isQuiz = true;
        quizPanel.SetActive(true);
    }

    public void HideQuiz(bool isCorrect)
    {
        quizPanel.SetActive(false);
        if(isCorrect)
            fortuneSpinWheel.StartSpin();
        else
            gameObject.SetActive(false);
    }
}

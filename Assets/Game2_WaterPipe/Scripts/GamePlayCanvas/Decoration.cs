using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.UI;

public class Decoration : MonoBehaviour
{
    public bool isFarm = false;
    public bool isFinish = false;
    public Image image;
    public DecorationData[] decorationDatas;
    public int currentDecorationIndex = 0;

    public List<PipeEnd> SuccessPipeEnds = new List<PipeEnd>();

    public List<SpineAnimationUIController> sauc = new List<SpineAnimationUIController>();

    void Start()
    {
        currentDecorationIndex = Random.Range(0, decorationDatas.Length);
        image.sprite = decorationDatas[currentDecorationIndex].mainSprite;
    }

    void Update()
    {
        if(isFinish) return;
        if(GameManager.Instance.isRunWater)
        {
            if(SuccessPipeEnds.TrueForAll(x => x.isFinish))
            {
                SetSuccessDecoration();
            }
            else
            {
                if(GameManager.Instance.runTimer < 0)
                {
                    SetFailedDecoration();
                }
            }
        }   
    }

    public void SetFailedDecoration()
    {
        if(isFinish) return;
        isFinish = true;
        // if(decorationDatas[currentDecorationIndex].failedSprite!=null)
        //     image.sprite = decorationDatas[currentDecorationIndex].failedSprite;
        if(isFarm)
        {
            foreach (var item in sauc)
            {
                item.gameObject.SetActive(true);
                item.SetAnimation("dry", false);
            }
        }
        else
        {
            foreach (var item in sauc)
            {
                item.gameObject.SetActive(true);
                item.SetAnimation("angry", true);
            }
        }
        
    }
    public void SetSuccessDecoration()
    {
        if(isFinish) return;
        isFinish = true;
        // if(decorationDatas[currentDecorationIndex].successSprite!=null)
        //     image.sprite = decorationDatas[currentDecorationIndex].successSprite;
        if(isFarm)
        {
            foreach (var item in sauc)
            {
                item.gameObject.SetActive(true);
                item.SetAnimation("wet", false);
            }
        }
        else
        {
            foreach (var item in sauc)
            {
                item.gameObject.SetActive(true);
                string animationName = Random.Range(0, 2) == 0 ? "happy" : "happy2";
                item.SetAnimation(animationName, true);
            }
        }
    }
}


[System.Serializable]
public class DecorationData
{
    public Sprite mainSprite;
    public Sprite failedSprite;
    public Sprite successSprite;   
}
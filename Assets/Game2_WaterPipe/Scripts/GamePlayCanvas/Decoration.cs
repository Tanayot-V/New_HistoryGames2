using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.UI;

public class Decoration : MonoBehaviour
{
    public Image image;
    public DecorationData[] decorationDatas;
    public int currentDecorationIndex = 0;

    public List<PipeEnd> SuccessPipeEnds = new List<PipeEnd>(); 

    void Start()
    {
        currentDecorationIndex = Random.Range(0, decorationDatas.Length);
        image.sprite = decorationDatas[currentDecorationIndex].mainSprite;
    }

    void Update()
    {
        if(GameManager.Instance.isRunWater)
        {
            if(SuccessPipeEnds.TrueForAll(x => x.isFinish))
            {
                SetSuccessDecoration();
            }
            else
            {
                if(GameManager.Instance.runTimer <= 0)
                {
                    SetFailedDecoration();
                }
            }
        }   
    }

    public void SetFailedDecoration()
    {
        if(decorationDatas[currentDecorationIndex].failedSprite!=null)
            image.sprite = decorationDatas[currentDecorationIndex].failedSprite;
    }
    public void SetSuccessDecoration()
    {
        if(decorationDatas[currentDecorationIndex].successSprite!=null)
            image.sprite = decorationDatas[currentDecorationIndex].successSprite;
    }
}


[System.Serializable]
public class DecorationData
{
    public Sprite mainSprite;
    public Sprite failedSprite;
    public Sprite successSprite;   
}
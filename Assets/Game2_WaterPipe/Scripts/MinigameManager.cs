using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinigameManager : MonoBehaviour
{

    public Image itemImg;
    public List<ItemData> itemData;
    public ItemData currentItemData;

    public int minHitCount = 3;
    public int maxHitCount = 5;
    public int hitCount = 0;
    private int currentHitCount = 0;

    public void SetUP(string itemId)
    {
        currentHitCount = 0;
        currentItemData = itemData.Find(x => x.itemId == itemId);
        if (currentItemData != null)
        {
            itemImg.sprite = currentItemData.itemSprite;
        }
        else
        {
            EndGame(false);
        }
        hitCount = UnityEngine.Random.Range(minHitCount, maxHitCount);
    }

    public void Hit()
    {
        currentHitCount++;
        if (currentHitCount >= hitCount)
        {
            EndGame(true);
        }
        else if (currentHitCount >= hitCount / 2)
        {
            itemImg.sprite = currentItemData.lowHpSprite;
        }
    }

    void EndGame(bool isfinish) 
    {
        GameManager.Instance.gameUiManager.hideMinigame(isfinish);
    }
}

[System.Serializable]
public class ItemData
{
    public string itemId;
    public Sprite itemSprite;
    public Sprite lowHpSprite;
}

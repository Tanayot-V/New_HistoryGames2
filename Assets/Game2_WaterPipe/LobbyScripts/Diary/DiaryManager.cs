using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

[System.Serializable]
public class DiaryState
{
    public string id;
    public DiaryModelSO modelSO;
    public bool isClaim;
}

public class DiaryManager : Singletons<DiaryManager>
{
    public DiaryDatabaseSO diaryDatabaseSO;
    public List<DiaryState> diaryStates = new List<DiaryState>();

    [Header("Diary UI")]
    public GameObject diaryOpenOBJ;
    public GameObject prefabSlot;
    public GameObject prefabSlotNull;
    public GameObject parentDiary;

    [Header("Diary Rewards")]
    public DiarySlot currentClaimSlot;
    public GameObject claimedOpenOBJ;
    public Image itemsIMG;
    public TMPro.TextMeshProUGUI topicTX;
    public TMPro.TextMeshProUGUI contentTX;

    public void InitDiary()
    {
        UiController.Instance.DestorySlot(parentDiary);
        diaryStates.ForEach(o => {
            DiarySlot slot = UiController.Instance.InstantiateUIView(prefabSlot, parentDiary).GetComponent<DiarySlot>();
            slot.SetupSlot(o,o.isClaim);
        });
        GameObject slot = UiController.Instance.InstantiateUIView(prefabSlotNull, parentDiary);
    }

    public void DiaryOpen()
    {
        diaryOpenOBJ.SetActive(true);
        InitDiary();
    }

    public void DiaryClose()
    {
        diaryOpenOBJ.SetActive(false);
        UiController.Instance.DestorySlot(parentDiary);
        parentDiary.GetComponent<RectTransform>().position = Vector3.zero;
    }

    #region ClaimReward
    public void ClaimRewardPageOpen(DiarySlot _diarySlot)
    {
        claimedOpenOBJ.SetActive(true);
        currentClaimSlot = _diarySlot;
        itemsIMG.sprite = currentClaimSlot.diaryState.modelSO.itemSP;
        topicTX.text = currentClaimSlot.diaryState.modelSO.topicST;
        contentTX.text = currentClaimSlot.diaryState.modelSO.descriptionST;
    }

    public void ClaimRewardPageClose()
    {
        claimedOpenOBJ.SetActive(false);
        currentClaimSlot = null;
    }
    #endregion
}

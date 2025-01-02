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
    
    public bool isRead;
}

public class DiaryManager : Singletons<DiaryManager>
{
    public DiaryDatabaseSO diaryDatabaseSO;
    public List<DiaryState> diaryStates = new List<DiaryState>();
    public GameObject redPointOBJ;

    [Header("Diary UI")]
    public GameObject diaryOpenOBJ;
    public GameObject prefabSlot;
    public GameObject prefabSlotNull;
    public GameObject parentDiary;

    [Header("Diary Description")]
    public GameObject diaryDescriptionOBJ;
    public Image headIMG;
    public Image textDesIMG;
    public TMPro.TextMeshProUGUI des2TX;
    public Image itemBGIMG;

    public Image claimART;
    public GameObject itemDesOBJ;
    public GameObject videoOBJ;

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
        claimedOpenOBJ.SetActive(false);
        diaryDescriptionOBJ.SetActive(false);
        currentClaimSlot = null;
    }

    public bool IsCheckRead()
    {
        bool isRead = false;
        foreach (var item in diaryStates)
        {
            if (!item.isRead)
            {
                isRead = true;
                break;
            }
        }
        return isRead;
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
        RectTransform parentDiaryRect = parentDiary.GetComponent<RectTransform>();
        parentDiaryRect.offsetMin = Vector2.zero; // Left, Bottom
        parentDiaryRect.offsetMax = Vector2.zero; // Right, Top
        
        // Set PosY to 0
        Vector3 anchoredPosition = parentDiaryRect.anchoredPosition;
        anchoredPosition.y = 0;

        redPointOBJ.SetActive(diaryStates.Any(o => !o.isRead));
    }

    #region Description
    public void DescriptionOpen(DiarySlot _diarySlot)
    {
        currentClaimSlot = _diarySlot;
        diaryDescriptionOBJ.SetActive(true);
        diaryDescriptionOBJ.transform.GetChild(1).GetComponent<CanvasGroupTransition>().FadeIn();

        headIMG.sprite = _diarySlot.diaryState.modelSO.headIMG;
        textDesIMG.sprite = _diarySlot.diaryState.modelSO.textDesIMG;
        itemDesOBJ.transform.GetChild(0).GetComponent<Image>().sprite = _diarySlot.diaryState.modelSO.itemSP;
        des2TX.text = _diarySlot.diaryState.modelSO.descriptionST2;
        
        if(_diarySlot.diaryState.isClaim)
        {
            itemDesOBJ.transform.GetChild(0).GetComponent<Image>().material = UILobbyGameManager.Instance.garyMATWorld;
            itemDesOBJ.transform.GetChild(1).GetComponent<Image>().material = UILobbyGameManager.Instance.garyMATWorld;
            itemDesOBJ.transform.GetChild(1).gameObject.SetActive(true);
            itemBGIMG.material = UILobbyGameManager.Instance.garyMATWorld;
        }
        else
        {
            itemDesOBJ.transform.GetChild(0).GetComponent<Image>().material = null;
            itemDesOBJ.transform.GetChild(1).GetComponent<Image>().material = null;
            itemBGIMG.material = null;
            itemDesOBJ.transform.GetChild(1).gameObject.SetActive(false);
        }
    }

    public void DescriptionClose()
    {
        diaryDescriptionOBJ.SetActive(false);
    }
    #endregion
    
    #region ClaimReward
    public void ClaimReward()
    {
        if (currentClaimSlot == null) return;

        DescriptionClose();
        if (currentClaimSlot.diaryState.isClaim) 
        {
            ClaimRewardPageClose();
            return;
        }
        if (currentClaimSlot.diaryState.isClaim) return;
        LobbyGameManager.Instance.DiaryOpenRewardPage(currentClaimSlot);
    }

    public void ClaimRewardPageOpen(DiarySlot _diarySlot)
    {
        claimedOpenOBJ.SetActive(true);
        currentClaimSlot = _diarySlot;
        itemsIMG.sprite = currentClaimSlot.diaryState.modelSO.itemSP;
        topicTX.text = currentClaimSlot.diaryState.modelSO.topicST;
        contentTX.text = currentClaimSlot.diaryState.modelSO.descriptionST;
        _diarySlot.diaryState.isClaim = true;
        _diarySlot.SetupSlot(_diarySlot.diaryState, true);
    }

    public void ClaimRewardPageClose()
    {
        claimedOpenOBJ.SetActive(false);
        currentClaimSlot = null;
    }
    #endregion
}

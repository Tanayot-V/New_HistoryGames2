using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Unity.VisualScripting;

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

    [Header("Lobby UI")]
    public GameObject prefabSlot;
    public GameObject prefabSlotNull;
    public GameObject parentDiary;

    [Header("Lobby Rewards")]
    public Image itemsIMG;
    public TMPro.TextMeshProUGUI topicTX;
    public TMPro.TextMeshProUGUI contentTX;

    public void InitDiary()
    {
        UiController.Instance.DestorySlot(parentDiary);
        diaryStates.ForEach(o => {
            DiarySlot slot = UiController.Instance.InstantiateUIView(prefabSlot, parentDiary).GetComponent<DiarySlot>();
            slot.topicIMG.sprite = o.modelSO.headSP;
            slot.itemIMG.sprite = o.modelSO.itemSP;
            slot.topicTX.text = o.modelSO.topicST;
            slot.contentTX.text = o.modelSO.descriptionST;
            if (o.isClaim)
            {
                slot.itemIMG.material = UILobbyGameManager.Instance.garyMAT;
                slot.claimedIMG.gameObject.SetActive(true);
            }
            else
            {
                slot.itemIMG.material = null;
                slot.claimedIMG.gameObject.SetActive(false);
            }
        });
        GameObject slot = UiController.Instance.InstantiateUIView(prefabSlotNull, parentDiary);
    }
}

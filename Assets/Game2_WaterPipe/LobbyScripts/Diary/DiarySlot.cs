using System.Collections;
using System.Collections.Generic;
using Spine;
using UnityEngine;
using UnityEngine.UI;

public class DiarySlot : MonoBehaviour
{
    public DiaryState diaryState;
    public Image topicIMG;
    public Image itemIMG;
    public Image claimedIMG;
    public GameObject redPointOBJ;
    public TMPro.TextMeshProUGUI topicTX;
    public TMPro.TextMeshProUGUI contentTX;

    public void SetupSlot(DiaryState _diaryState, bool _isClaim)
    {
        diaryState = _diaryState;
        topicIMG.sprite = _diaryState.modelSO.headSP;
        itemIMG.sprite = _diaryState.modelSO.itemSP;
        topicTX.text = _diaryState.modelSO.topicST;
        contentTX.text = _diaryState.modelSO.descriptionST;
        if (_isClaim)
        {
            itemIMG.material = UILobbyGameManager.Instance.garyMATUI;
            claimedIMG.gameObject.SetActive(true);
        }
        else
        {
            itemIMG.material = null;
            claimedIMG.gameObject.SetActive(false);
        }

        if(_diaryState.isRead)
        {
            redPointOBJ.SetActive(false);
        }
        else
        {
            redPointOBJ.SetActive(true);
        }
    }

    public void ClaimButton()
    {
        if (diaryState.isClaim) return;
        LobbyGameManager.Instance.DiaryOpenRewardPage(this);
        Debug.Log("ClaimButton:" + name);
    }

    public void OpenDescriptionButton()
    {
        LobbyGameManager.Instance.OpenDescriptionButton(this);
        diaryState.isRead = true;
        UILobbyGameManager.Instance.ClickAudio();
    }
}

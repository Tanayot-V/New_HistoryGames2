using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardSlot : MonoBehaviour
{
    public LeaderboardState leaderbaordState;
    public Image orderIMG;
    public Image playerIMG;
    public Image bgIMG;
    public TMPro.TextMeshProUGUI displayNameTX;
    public TMPro.TextMeshProUGUI scoreTX;
    public TMPro.TextMeshProUGUI userIDTX;
    public TMPro.TextMeshProUGUI orderTX;

    public void SetupSlot(LeaderboardState _leaderbaordState)
    {
        leaderbaordState = _leaderbaordState;
        if (leaderbaordState.order > 3) orderTX.text = _leaderbaordState.order.ToString();
        playerIMG.sprite = leaderbaordState.picture;
        displayNameTX.text = leaderbaordState.displayName;
        userIDTX.text = leaderbaordState.userID;
        scoreTX.text = leaderbaordState.score.ToString();
        bgIMG.sprite = leaderbaordState.picture;

    }

    public void SetBGIMG(Sprite _sprite)
    {
        bgIMG.sprite = _sprite;
    }

    public void SetOrderIMG(Sprite _sprite)
    {
        orderIMG.sprite = _sprite;
    }
}

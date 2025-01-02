using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyGameManager : Singletons<LobbyGameManager>
{
    public AudioModelSO audioModelSO;
    public UILobbyGameManager uiLobbyGameManager;
    public SettingManager settingManager;
    public DiaryManager diaryManager;
    public LeaderboardManager leaderboardManager;

    public void Start()
    {
        SoundManager.Instance.Init(audioModelSO);
        SoundManager.Instance.PlayAudioSource("BGM"); 
        SoundManager.Instance.SetVolumeBGM(0.25f);
        
        settingManager.ClosePage();
        diaryManager.DiaryClose();
        leaderboardManager.ClosePage();
    }

    #region Setting
    public void SettingOpenButton()
    {
        uiLobbyGameManager.ClickAudio();
        settingManager.OpenPage();
    }

    public void SetiingCloseButton()
    {
        settingManager.ClosePage();
    }
    #endregion

    #region Diary

    public void DiaryOpenPageButton()
    {
        diaryManager.InitDiary();
        diaryManager.DiaryOpen();
    }

    public void DiaryClosePageButton()
    {
        diaryManager.DiaryClose();
    }

    public void OpenDescriptionButton(DiarySlot _diarySlot)
    {
        UILobbyGameManager.Instance.ClickAudio();
        diaryManager.DescriptionOpen(_diarySlot);
    }

    public void DiaryOpenRewardPage(DiarySlot _diarySlot)
    {
        diaryManager.ClaimRewardPageOpen(_diarySlot);
    }

    public void DiaryCloseRewardPageButton()
    {
        diaryManager.ClaimRewardPageClose();
    }

    public void DiaryRewardClaimButton()
    {
        diaryManager.ClaimReward();
    }
    #endregion

    #region Leaderborad
    public void LeaderboardOpenButton()
    {
        leaderboardManager.OpenPage();
        leaderboardManager.InitLeaderboard();
    }    
    
    public void LeaderboardCloseButton()
    {
        leaderboardManager.ClosePage();
    }
    #endregion

}

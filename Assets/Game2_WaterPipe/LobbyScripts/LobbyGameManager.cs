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

    public SelectLevelManager selectLevelManager;

    public void Start()
    {
        SoundManager.Instance.Init(audioModelSO);
        SoundManager.Instance.PlayAudioSource("BGM"); 
        if (PlayerPrefs.HasKey("VolumeBGM")) SoundManager.Instance.SetVolumeBGM(PlayerPrefs.GetFloat("VolumeBGM"));
        else SoundManager.Instance.SetVolumeBGM(0.25f);
        if(PlayerPrefs.HasKey("VolumeSFX"))SoundManager.Instance.SetVolumeSFX(PlayerPrefs.GetFloat("VolumeSFX"));
        else SoundManager.Instance.SetVolumeSFX(0.5f);

        settingManager.ClosePage();
        diaryManager.DiaryClose();
        leaderboardManager.ClosePage();
        
        selectLevelManager.InitSelectLevel();
        leaderboardManager.StartLeader();
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

    public bool DiaryIsCheckRead()
    {   
        return diaryManager.IsCheckRead();
    }

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
        UILobbyGameManager.Instance.ClickAudio();
        diaryManager.ClaimRewardPageClose();
    }

    public void DiaryRewardClaimButton()
    {
        UILobbyGameManager.Instance.ClickAudio();
        diaryManager.ClaimReward();
    }
    #endregion

    #region Leaderborad
    public void LeaderboardOpenButton()
    {
        leaderboardManager.OpenPage();
        leaderboardManager.InitLeaderboard();
        UILobbyGameManager.Instance.ClickAudio();
    }    
    
    public void LeaderboardCloseButton()
    {
        leaderboardManager.ClosePage();
        UILobbyGameManager.Instance.ClickAudio();
    }
    #endregion

    public void ClickLink()
    {
        Application.OpenURL("https://gamesflexx-56768.web.app/Game3Hero/index.html");
    }

    public void ToChallange()
    {
        UILobbyGameManager.Instance.StartLoading(() =>
        {
            DataCenterManager.Instance.LoadSceneByName("ChallangeMode");
            Debug.Log("Loading Complete");
        });
    }
}

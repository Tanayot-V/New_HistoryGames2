using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingManager : MonoBehaviour
{
    public GameObject settingOBJ;
    public Slider volumeSlider_BGM;
    public Slider volumeSlider_SFX;

    public void Start()
    {
        InitSetting();
    }

    public void OpenPage()
    {
        settingOBJ.SetActive(true);
    }

    public void ClosePage()
    {
        settingOBJ.SetActive(false);
    }

    public void InitSetting()
    {
        volumeSlider_BGM.value = SoundManager.Instance.volumeBGM;
        volumeSlider_SFX.value = SoundManager.Instance.volumeBGM;
    }

    public void SetVolumeBGM()
    {
        SoundManager.Instance.SetVolumeBGM(volumeSlider_BGM.value);
    }

    public void SetVolumeSFX()
    {
        SoundManager.Instance.SetVolumeSFX(volumeSlider_SFX.value);
    }

    public void ClickAudio()
    {
        SoundManager.Instance.PlayAudioSource("Click");
    }
}

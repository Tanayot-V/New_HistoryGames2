using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILobbyGameManager : Singletons<UILobbyGameManager>
{
    [Header("Material")]
    public Material garyMATUI;
    public Material garyMATWorld;
    public Material spriteMAT;

    [Header("Loading")]
    public GameObject loadingGO;
    public LoadingFillAmount loadingFillAmount;

    public void StartLoading(System.Action callback)
    {
        loadingGO.SetActive(true);
        loadingFillAmount.StartFillAmount(() =>
        {
            callback();
        });
    }
    
    public void ClickAudio()
    {
        if (SoundManager.Instance == null)
        {
            Debug.LogWarning("SoundManager instance is not available.");
            return;
        }
        SoundManager.Instance.PlayAudioSource("Click");
    }
}

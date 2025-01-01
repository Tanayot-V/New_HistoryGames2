using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingFillAmount : MonoBehaviour
{
    public Image fillIMG;
    public float duration = 1;

    private IEnumerator StartLoading(System.Action callback)
    {
        float time = 0;
        while (time < duration)
        {
            time += Time.deltaTime;
            fillIMG.fillAmount = time / duration; 
            yield return null;
        }
        fillIMG.fillAmount = 1;
        callback();
    }

    public void StartFillAmount(System.Action callback)
    {
        StartCoroutine(StartLoading(callback));
    }
}

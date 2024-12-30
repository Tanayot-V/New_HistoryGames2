using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class UIBounceAnimation : MonoBehaviour
{
    public RectTransform uiElement; 
    public float scaleMultiplier = 1.5f;
    public float duration = 0.5f;
    public Ease animationEase = Ease.InOutQuad; 

    void Start()
    {
        uiElement = this.GetComponent<RectTransform>();
        ScaleUI();
    }

    void ScaleUI()
    {
        uiElement.DOScale(Vector3.one * scaleMultiplier, duration)
                 .SetEase(animationEase) 
                 .SetLoops(-1, LoopType.Yoyo) 
                 .SetUpdate(true);
    }
}

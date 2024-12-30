using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BounceAnimation : MonoBehaviour
{
    public Transform targetTransform; 
    public float positionOffset = 1.5f; 
    public float duration = 0.5f; 
    public Ease animationEase = Ease.InOutQuad; 

    void Start()
    {
        if (targetTransform == null)
        {
            targetTransform = this.transform; 
        }

        AnimatePosition();
    }

    void AnimatePosition()
    {
        Vector3 startPosition = targetTransform.position;

        targetTransform.DOMoveY(startPosition.y + positionOffset, duration)
            .SetEase(animationEase) 
            .SetLoops(-1, LoopType.Yoyo) 
            .SetUpdate(true); 
    }
}

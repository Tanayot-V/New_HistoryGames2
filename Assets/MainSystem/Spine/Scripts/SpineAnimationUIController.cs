using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class SpineAnimationUIController : MonoBehaviour
{
    [SerializeField]private SkeletonGraphic skeletonGraphic; 
    [SerializeField]private Spine.AnimationState animationState;

    public void Start()
    {
        if (skeletonGraphic == null) skeletonGraphic = GetComponent<SkeletonGraphic>();
        animationState = skeletonGraphic.AnimationState;
        //AddAnimationToQueue("dry", false);
    }

    public void AddAnimationToQueue(string animationName, bool loop)
    {
        if (animationState != null && !string.IsNullOrEmpty(animationName))
        {
            animationState.AddAnimation(0, animationName, loop, 0);
        }
        else
        {
            Debug.LogWarning($"Animation '{animationName}' not found or AnimationState is null.");
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class SpineAnimationUIController : MonoBehaviour
{
    [SerializeField]private SkeletonGraphic skeletonGraphic; 

    public void Start()
    {
        if (skeletonGraphic == null) skeletonGraphic = GetComponent<SkeletonGraphic>();
        //AddAnimationToQueue("dry", false);
    }

    public void SetAnimation(string animationName, bool loop)
    {
        if (!string.IsNullOrEmpty(animationName))
        {
            //Debug.Log($"SetAnimation: {animationName} - {loop}");
            skeletonGraphic.AnimationState.SetAnimation(0, animationName, loop);
        }
        else
        {
            Debug.LogWarning($"Animation '{animationName}' not found or AnimationState is null.");
        }
    }

    public void AddAnimationToQueue(string animationName, bool loop)
    {
        if (!string.IsNullOrEmpty(animationName))
        {
            //Debug.Log($"AddAnimationToQueue: {animationName} - {loop}");
            skeletonGraphic.AnimationState.AddAnimation(0, animationName, loop, 0);
        }
        else
        {
            Debug.LogWarning($"Animation '{animationName}' not found or AnimationState is null.");
        }
    }
}

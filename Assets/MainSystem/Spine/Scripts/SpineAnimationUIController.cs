using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class SpineAnimationUIController : MonoBehaviour
{
    private SkeletonGraphic skeletonGraphic; 
    [SerializeField] private bool loopDefaultAnimation = false; 

    private Spine.AnimationState animationState;
    public void Start()
    {
        /*
        if (loopDefaultAnimation)
        {
            AddAnimationToQueue("dry", false);
        }*/
    }

    private void Awake()
    {
         if (skeletonGraphic == null)
        {
            skeletonGraphic = GetComponent<SkeletonGraphic>();
        }

        animationState = skeletonGraphic.AnimationState;
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

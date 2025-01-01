using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PipeEnd : MonoBehaviour
{
    public bool isWaste = false;
    public bool isFinish = false;

    public UnityEvent onFinished;

    public void OnWaterIn(bool mIsWaste)
    {
        if (isFinish) return;
        if (mIsWaste)
        {
            if (isWaste)
            {
                isFinish = true;
                onFinished?.Invoke();
            }
        }
        else
        {
            if (!isWaste)
            {
                isFinish = true;
                onFinished?.Invoke();
            }
        }
    }
}

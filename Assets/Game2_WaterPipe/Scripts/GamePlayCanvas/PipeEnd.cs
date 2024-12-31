using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeEnd : MonoBehaviour
{
    public bool isWaste = false;
    public bool isFinish = false;

    public void OnWaterIn(bool mIsWaste)
    {
        if (isFinish) return;
        if (mIsWaste)
        {
            if (isWaste)
            {
                isFinish = true;
            }
        }
        else
        {
            if (!isWaste)
            {
                isFinish = true;
            }
        }
    }
}

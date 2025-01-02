using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
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
                PipeObject pipeObject = GetComponentInChildren<PipeObject>();
                if (pipeObject != null)
                {
                    GameObject go = GameManager.Instance.gridCanvas.slots[(int)pipeObject.startPipePos.x,(int)pipeObject.startPipePos.y].item;
                    if(go != null)
                    {
                        PipeStart ps = go.GetComponent<PipeStart>();
                        if(ps != null)
                        {
                            Debug.Log($"Waste Pipe start finish {ps.name} ay {pipeObject.startPipePos}");
                            ps.isFinish = true;
                        }
                    }

                }
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

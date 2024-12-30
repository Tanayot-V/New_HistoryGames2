using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeEnd : MonoBehaviour
{
    PipeObject pipeObject;

    

    public bool isFinish = false;

    void Start()
    {
        pipeObject = GetComponent<PipeObject>();
    }
}

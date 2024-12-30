using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

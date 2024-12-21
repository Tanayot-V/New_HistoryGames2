using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeConnectionCheck : MonoBehaviour
{
    public Direction direction;

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"Connection Check trigger {direction} : {other.name} tag : {other.tag}");
    }
}

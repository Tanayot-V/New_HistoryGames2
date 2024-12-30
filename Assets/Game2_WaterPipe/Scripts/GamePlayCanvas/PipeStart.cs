using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PipeStart : MonoBehaviour, IPointerClickHandler
{
    public PipeObject pipeObject;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("PipeStart OnPointerClick");
        Vector2 dir = Vector2.down;
        switch (pipeObject.pipeData.direction)
        {
            case Direction.Up:
                dir = Vector2.up;
                break;
            case Direction.Right:
                dir = Vector2.right;
                break;
            case Direction.Down:
                dir = Vector2.down;
                break;
            case Direction.Left:
                dir = Vector2.left;
                break;
        }
        pipeObject.WaterOut(dir);
    }
}

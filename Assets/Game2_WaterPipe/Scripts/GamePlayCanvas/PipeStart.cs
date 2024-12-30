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
        List<Vector2> dirs = new List<Vector2>();
        switch (pipeObject.pipeData.direction)
        {
            case Direction.Up:
                dirs.Add(Vector2.up);
                break;
            case Direction.Right:
                dirs.Add(Vector2.right);
                break;
            case Direction.Down:
                dirs.Add(Vector2.down);
                break;
            case Direction.Left:
                dirs.Add(Vector2.left);
                break;
            case Direction.U_R:
                dirs.Add(Vector2.up);
                dirs.Add(Vector2.right);
                break;
            case Direction.U_L:
                dirs.Add(Vector2.down);
                dirs.Add(Vector2.left);
                break;
            case Direction.D_R:
                dirs.Add(Vector2.down);
                dirs.Add(Vector2.right);
                break;
            case Direction.D_L:
                dirs.Add(Vector2.down);
                dirs.Add(Vector2.left);
                break;
            case Direction.U_R_D:
                dirs.Add(Vector2.up);
                dirs.Add(Vector2.right);
                dirs.Add(Vector2.down);
                break;
            case Direction.R_D_L:
                dirs.Add(Vector2.right);
                dirs.Add(Vector2.down);
                dirs.Add(Vector2.left);
                break;
            case Direction.D_L_U:
                dirs.Add(Vector2.down);
                dirs.Add(Vector2.left);
                dirs.Add(Vector2.up);
                break;
            case Direction.L_U_R:
                dirs.Add(Vector2.left);
                dirs.Add(Vector2.up);
                dirs.Add(Vector2.right);
                break;
            case Direction.All:
                dirs.Add(Vector2.up);
                dirs.Add(Vector2.down);
                dirs.Add(Vector2.left);
                dirs.Add(Vector2.right);
                break;
            case Direction.U_D:
                dirs.Add(Vector2.up);
                dirs.Add(Vector2.down);
                break;
            case Direction.L_R:
                dirs.Add(Vector2.left);
                dirs.Add(Vector2.right);
                break;
        }
        pipeObject.WaterOut(dirs.ToArray());
    }
}

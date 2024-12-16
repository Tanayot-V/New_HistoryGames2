using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PipeData
{
    public string pipeID;
    public Vector2 pos;
    public Direction direction;
    public GameObject pipeObject; //pipeSlot

    public PipeData()
    {

    }

    public PipeData(string _pipeID,Vector2 _pos, Direction _direction, GameObject _pipeObject)
    {
        this.pipeID = _pipeID;
        this.pos = _pos;
        this.direction = _direction;
        this.pipeObject = _pipeObject; //pipeSlot
    }
}

public enum Direction
{
    None = 0,
    Up = 1,
    Right = 2,
    Down = 3,
    Left = 4
}

public class PipeManager : MonoBehaviour
{
    [SerializeField] private List<PipeData> pipeDatas = new List<PipeData>();

    public void AddPipeData(PipeData _pipeData)
    {
        pipeDatas.Add(_pipeData);
    }
    public void ClearPipeData()
    {
        pipeDatas.Clear();
    }
}

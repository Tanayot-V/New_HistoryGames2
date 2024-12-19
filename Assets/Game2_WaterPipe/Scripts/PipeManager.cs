using System.Collections;
using System.Collections.Generic;
using Spine;
using Spine.Unity;
using UnityEngine;

[System.Serializable]
public class PipeModel
{
    public PipeType pipeType;
    public Sprite picture;
}

[System.Serializable]
public class PipeData
{
    public string pipeID;
    public Vector2 pos;
    public PipeType pipeType;
    public Direction direction;
    public PipeSlot pipeSlot; //pipeSlot

    public PipeData(){}

    public PipeData(string _pipeID,Vector2 _pos, PipeType _pipeType,Direction _direction, PipeSlot _pipeSlot)
    {
        this.pipeID = _pipeID;
        this.pos = _pos;
        this.pipeType = _pipeType;
        this.direction = _direction;
        this.pipeSlot = _pipeSlot; //pipeSlot
    }

    public PipeData(PipeType _pipeType, Direction _direction)
    {
        this.pipeType = _pipeType;
        this.direction = _direction;
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

public enum PipeType
{
    None = 0,
    Obstacle = 1,
    Straight = 2,
    Degree90 = 3,
    Tee = 4,
    Cross = 5,
}

public class PipeManager : MonoBehaviour
{
    [Header("Database")]
    [SerializeField] private PipeDatabase pipeDatabase;

    [Header("PipeData")]
    [SerializeField] private List<PipeData> pipeDatas = new List<PipeData>();
    [SerializeField] private Dictionary<Vector2,PipeData> pipeDatasDIC = new Dictionary<Vector2, PipeData>();

    public void RandomPipeMap()
    {
        pipeDatas.ForEach(o => {
            //Random PipeType
            o.pipeType = (PipeType)Random.Range(0, 5);
            o.pipeSlot.InitSlot(o);
        });
    }

    public void AddPipeData(PipeData _pipeData)
    {
        pipeDatas.Add(_pipeData);
        pipeDatasDIC.Add(_pipeData.pos,_pipeData);
    }

    public void ClearPipeData()
    {
        pipeDatas.Clear();
    }

    public void SetColorDefaulfPipeSlotData()
    {
        pipeDatas.ForEach(o=> {
            o.pipeSlot.GetComponent<PipeSlot>().ColorDefalut();
        });
    }

    public PipeModel GetPipeModel(PipeType _pipeType)
    {
        return pipeDatabase.GetPipeModel(_pipeType);
    }

    public void UpdatePipeSlotTOList(Vector2 _pos, PipeData _pipeData)
    {
        pipeDatasDIC[_pos] = _pipeData;
        pipeDatas.ForEach(o => {
            if (o.pos == _pos)
            {
                o.pipeType = _pipeData.pipeType;
                o.direction = _pipeData.direction;
                Debug.Log($"UpdatePipe: {pipeDatasDIC[_pos].pos} | {pipeDatasDIC[_pos].pipeType} | {pipeDatasDIC[_pos].direction}");
            }
        });
    }

   
}

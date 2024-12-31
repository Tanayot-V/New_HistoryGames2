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
    public Sprite pictureWater;
    public Sprite pictureWater1;
    public GameObject prefab;
}

[System.Serializable]
public class PipeData
{
    public Vector2 pos;
    public PipeType pipeType;
    public Direction direction;

    public PipeData(){}

    public PipeData(Vector2 _pos, PipeType _pipeType,Direction _direction)
    {
        this.pos = _pos;
        this.pipeType = _pipeType;
        this.direction = _direction;
    }

    public PipeData(PipeType _pipeType, Direction _direction)
    {
        this.pipeType = _pipeType;
        this.direction = _direction;
    }
}

public enum Direction
{
    Up = 0,
    Right = 1,
    Down = 2,
    Left = 3,
    U_R = 4,
    U_L = 5,
    D_R = 6,
    D_L = 7,
    U_R_D = 8,
    R_D_L = 9,
    D_L_U = 10,
    L_U_R = 11,
    All = 12,
    U_D = 13,
    L_R = 14, 
}

public enum PipeType
{
    None = 0,
    Obstacle = 1,
    Straight = 2,
    Degree90 = 3,
    Tee = 4,
    Cross = 5,
    StraightCross = 6,
    Degree90Cross = 7,
    Start = 8,
    End = 9,
    Map = 10,
    Road = 11,
    WasteStart = 12,
    WasteEnd = 13,
}

public class PipeManager : MonoBehaviour
{
    [Header("Database")]
    [SerializeField] private PipeDatabase pipeDatabase;

    [Header("PipeData")]
    [SerializeField] private List<PipeData> pipeDatas = new List<PipeData>();
    public Dictionary<Vector2,PipeData> pipeDatasDIC = new Dictionary<Vector2, PipeData>();

    public void RandomPipeMap()
    {
        pipeDatas.ForEach(o => {
            //Random PipeType
            o.pipeType = (PipeType)Random.Range(0, 5);

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

    public PipeModel GetPipeModel(PipeType _pipeType)
    {
        return pipeDatabase.GetPipeModel(_pipeType);
    }

    public float GetRotationZ(Direction dir)
    {
        float result = 0f;
        switch (dir)
        {
            case Direction.Up:
            result = 180f;
            break;
            case Direction.Right:
            result = 90f;
            break;
            case Direction.Down:
            result = 0f;
            break;
            case Direction.Left:
            result = 270f;
            break;
        }
        return result;
    }

    public void UpdatePipeSlotTOList(Vector2 _pos, PipeData _pipeData)
    {
        pipeDatasDIC[_pos] = _pipeData;
        pipeDatas.ForEach(o => {
            if (o.pos == _pos)
            {
                o.pipeType = _pipeData.pipeType;
                o.direction = _pipeData.direction;
                //Debug.Log($"UpdatePipe: {pipeDatasDIC[_pos].pos} | {pipeDatasDIC[_pos].pipeType} | {pipeDatasDIC[_pos].direction}");
            }
        });
    }  
}

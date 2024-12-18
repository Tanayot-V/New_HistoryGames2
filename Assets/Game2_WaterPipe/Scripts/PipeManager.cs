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

    public PipeData(string _pipeID,Vector2 _pos, Direction _direction, PipeType _pipeType, PipeSlot _pipeSlot)
    {
        this.pipeID = _pipeID;
        this.pos = _pos;
        this.direction = _direction;
        this.pipeType = _pipeType;
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
    Straight = 1,
    Degree90 = 2,
    Tee = 3,
    Cross = 4,
    CrossStraight = 5
}

public class PipeManager : MonoBehaviour
{
    [Header("Database")]
    [SerializeField] private PipeDatabase pipeDatabase;

    [Header("PipeData")]
    [SerializeField] private List<PipeData> pipeDatas = new List<PipeData>();

    [Header("DangAndDrop")]
    [SerializeField] private GameObject pipeSlotDragDrop;
    [SerializeField] private GameObject pipeSlotDragDrop_Parent;
    public PipeSlot targetDragDrop;

    [Header("DangAndDrop Form UI")]
    public GameObject currentDragDrop;

    [Header("DangAndDrop Form GameWorld")]
    [SerializeField] private GameObject currentDragDropG;
    [SerializeField] private PipeSlot dragSlot;
    [SerializeField] private PipeSlot dropSlot;

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

    #region DragDropUI
    public GameObject CreatePipeSlotDragDrop(PipeData _pipeData)
    {
        if (currentDragDrop != null) return null;
        GameObject slot = Instantiate(pipeSlotDragDrop, pipeSlotDragDrop.GetComponent<PipeSlotDragDrop>().GetMouseWorldPos(), Quaternion.identity, transform);
        slot.transform.SetParent(pipeSlotDragDrop_Parent.transform);
        slot.GetComponent<PipeSlotDragDrop>().InitSlot(new PipeData(_pipeData.pipeType, _pipeData.direction));
        return slot;
    }

    public GameObject CreatePipeSlotDragDropUI(PipeData _pipeData)
    {
        currentDragDrop = CreatePipeSlotDragDrop(_pipeData);
        return currentDragDrop;
    }

    public bool IsDragAndDropingUI()
    {
        if (currentDragDrop == null) return false;
        else return true;
    }

    public void SetTargetDragDropUI(PipeSlot _target)
    {
        targetDragDrop = _target;
    }

    public void SetPipeTypeSlotUI(PipeData _pipeData)
    {
        if (targetDragDrop != null) targetDragDrop.SetupPipe(_pipeData);
    }

    private Dictionary<Direction, float> directionToRotation = new Dictionary<Direction, float>()
    {
        { Direction.None, 0f },
        { Direction.Up, 0f },
        { Direction.Right, 90f },
        { Direction.Down, -180f },
        { Direction.Left, -90f }
    };
    public float GetRotateFromDirection(Direction _direction)
    {
        return directionToRotation.ContainsKey(_direction) ? directionToRotation[_direction] : 0f;
    }
    #endregion

    #region DragDrop On GameWorld
    public GameObject CreatePipeSlotDragDropG(PipeData _pipeData)
    {
        currentDragDropG = CreatePipeSlotDragDrop(_pipeData);
        return currentDragDropG;
    }

    public bool IsDragAndDropingG()
    {
        if (currentDragDropG == null) return false;
        else return true;
    }

    public void SetDragDropG(PipeSlot _target,bool _isDang)
    {
        if (_isDang) dragSlot = _target;
        else dropSlot = _target;
    }

    public void SetupPipeG(PipeData _pipeData)
    {
        if (targetDragDrop != null) targetDragDrop.SetupPipe(_pipeData);
    }

    #endregion
}

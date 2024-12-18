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
    public Direction direction;
    public PipeType pipeType;
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
    public GameObject currentDragDrop;
    public PipeSlot targetDragDrop;

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

    public GameObject CreatePipeSlotDragDrop(PipeType _pipeType)
    {
        if (currentDragDrop != null) return null;
        GameObject slot = Instantiate(pipeSlotDragDrop, pipeSlotDragDrop.GetComponent<PipeSlotDragDrop>().GetMouseWorldPos(), Quaternion.identity, transform);
        slot.transform.SetParent(pipeSlotDragDrop_Parent.transform);
        slot.GetComponent<PipeSlotDragDrop>().InitSlot(_pipeType);
        currentDragDrop = slot;
        return slot;
    }

    public bool IsDragAndDroping()
    {
        if (currentDragDrop == null) return false;
        else return true;
    }

    public void SetTargetDragDrop(PipeSlot _target)
    {
        targetDragDrop = _target;
    }

    public void SetPipeTypeSlot(PipeType _pipeType)
    {
        targetDragDrop.ChangePipeType(_pipeType);
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
}

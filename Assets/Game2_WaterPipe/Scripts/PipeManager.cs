using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PipeData
{
    public string pipeID;
    public Vector2 pos;
    public Direction direction;
    public PipeType pipeType;
    public GameObject pipeObject; //pipeSlot

    public PipeData(){}

    public PipeData(string _pipeID,Vector2 _pos, Direction _direction, PipeType _pipeType, GameObject _pipeObject)
    {
        this.pipeID = _pipeID;
        this.pos = _pos;
        this.direction = _direction;
        this.pipeType = _pipeType;
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
    [SerializeField] private List<PipeData> pipeDatas = new List<PipeData>();

    [Header("DangAndDrop")]
    [SerializeField] private GameObject pipeSlotDragDrop;
    [SerializeField] private GameObject pipeSlotDragDrop_Parent;
    public GameObject currentDragDrop;

    public void AddPipeData(PipeData _pipeData)
    {
        pipeDatas.Add(_pipeData);
    }
    public void ClearPipeData()
    {
        pipeDatas.Clear();
    }

    public GameObject CreatePipeSlotDangDrop()
    {
        if (currentDragDrop != null) return null;
        GameObject slot = Instantiate(pipeSlotDragDrop, pipeSlotDragDrop.GetComponent<PipeSlotDragDrop>().GetMouseWorldPos(), Quaternion.identity, transform);
        slot.transform.SetParent(pipeSlotDragDrop_Parent.transform);
        currentDragDrop = slot;
        return slot;
    }

    public bool IsDragAndDroping()
    {
        if (currentDragDrop == null) return false;
        else return true;
    }

    public void SetColorDefaulfPipeSlotData()
    {
        pipeDatas.ForEach(o=> {
            o.pipeObject.GetComponent<PipeSlot>().ColorDefalut();
        });
    }
}

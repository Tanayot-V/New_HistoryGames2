using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragDropManager : MonoBehaviour
{
    [Header("DangAndDrop")]
    [SerializeField] private GameObject pipeSlotDragDrop;
    [SerializeField] private GameObject pipeSlotDragDrop_Parent;
    public PipeSlot targetDragDrop;

    [Header("DangAndDrop Form UI")]
    public GameObject currentDragDropUI;

    [Header("DangAndDrop Form GameWorld")]
    [SerializeField] private GameObject currentDragDropG;
    [SerializeField] private PipeSlot dragSlot;
    [SerializeField] private PipeSlot dropSlot;

    #region DragDrop
    public GameObject CreatePipeSlotDragDrop(PipeData _pipeData)
    {
        if (currentDragDropUI != null) return null;
        GameObject slot = Instantiate(pipeSlotDragDrop, pipeSlotDragDrop.GetComponent<DragDropPipeSlot>().GetMouseWorldPos(), Quaternion.identity, transform);
        slot.transform.SetParent(pipeSlotDragDrop_Parent.transform);
        slot.GetComponent<DragDropPipeSlot>().InitSlot(new PipeData(_pipeData.pipeType, _pipeData.direction));
        return slot;
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
    #region DragDropUI

    public GameObject CreatePipeSlotDragDropUI(PipeData _pipeData)
    {
        currentDragDropUI = CreatePipeSlotDragDrop(_pipeData);
        return currentDragDropUI;
    }

    public bool IsDragAndDropingUI()
    {
        if (currentDragDropUI == null) return false;
        else return true;
    }

    public void SetTargetDragDropUI(PipeSlot _target)
    {
        targetDragDrop = _target;
    }

    public void OnMouseUpDropUI(PipeData _dropData)
    {
        if (!IsDragAndDropingUI()) return;
        if (targetDragDrop != null)
        {
            if (targetDragDrop.pipeData.pipeType == PipeType.None)
            {
                if (targetDragDrop != null)
                {
                    targetDragDrop.SetupPipe(new PipeData(
                        targetDragDrop.pipeData.pipeID,
                        targetDragDrop.pipeData.pos,
                        _dropData.pipeType,
                        _dropData.direction,
                        targetDragDrop.pipeData.pipeSlot));
                }
            }
        }
    }
    #endregion

    #region DragDrop On GameWorld
    public GameObject CreatePipeSlotDragDropGW(PipeData _pipeData)
    {
        currentDragDropG = CreatePipeSlotDragDrop(_pipeData);
        return currentDragDropG;
    }

    public bool IsDragAndDropingGW()
    {
        if (currentDragDropG == null) return false;
        else return true;
    }

    public void SetDragDropGW(PipeSlot _target, bool _isDrag)
    {
        if (_isDrag) dragSlot = _target;
        else dropSlot = _target;
    }

    public void SetupPipeGW(PipeData _pipeData)
    {
        if (targetDragDrop != null) targetDragDrop.SetupPipe(_pipeData);
    }

    public void OnMouseUpDropGW(PipeSlot _dropSlot, PipeData _dropData)
    {
        SetDragDropGW(_dropSlot, false);
        if (targetDragDrop == null) return;
        if (dragSlot == null) return;
        if (dropSlot == null) return;
        else
        {
            //Obstacle => เด้งกลับที่เดิม
            if (dropSlot.pipeData.pipeType == PipeType.Obstacle)
            {
                dragSlot.SetupPipe(_dropData);
                return;
            }

            //ดรอปลงบนพื้นที่ว่าง => dragSlot ว่าง dropSlot ได้ข้อมูล
            if (dropSlot.pipeData.pipeType == PipeType.None)
            {
                targetDragDrop.SetupPipe(_dropData);
                return;
            }

            //สลับ
            if (dropSlot.pipeData.pipeType != PipeType.None && dropSlot.pipeData.pipeType != PipeType.Obstacle)
            {
                PipeData backUpDrag = new PipeData(
                    targetDragDrop.pipeData.pipeID,
                    targetDragDrop.pipeData.pos,
                    _dropData.pipeType,
                    _dropData.direction,
                    targetDragDrop.pipeData.pipeSlot);
                
                //PipeData currentDragData = currentDragDropG.GetComponent<DragDropPipeSlot>().pipeData;
                PipeData backUpDrop = new PipeData(
                    dropSlot.pipeData.pipeID,
                    dropSlot.pipeData.pos,
                    dropSlot.pipeData.pipeType,
                    dropSlot.pipeData.direction,
                    dropSlot.pipeData.pipeSlot);

                dropSlot.SetupPipe(backUpDrag);
                dragSlot.SetupPipe(backUpDrop);
            }
        }
    }
    #endregion
    #endregion
}

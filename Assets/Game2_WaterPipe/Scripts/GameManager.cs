using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class GameManager : Singletons<GameManager>
{
    public Canvas canvas;
    [SerializeField] private GridSystem gridSystem;
    [SerializeField] private PipeManager pipeManager;
    [SerializeField] private DragDropManager dragDropManager;

    public void Start()
    {
        gridSystem.CreateGrid();
    }

    public GridSystem GridSystem()
    {
        return DataCenterManager.GetData(ref gridSystem, "GridSystem");
    }

    #region PipeManager
    public PipeManager PipeManager()
    {
        return DataCenterManager.GetData(ref pipeManager, "PipeManager");
    }

    public Sprite GetPipeModelPicture(PipeType _pipeType)
    {
        if (_pipeType == PipeType.None) return null;
        return pipeManager.GetPipeModel(_pipeType).picture;
    }

    public void UpdatePipeSlotTOList(Vector2 _pos, PipeData _pipeData)
    {
        pipeManager.UpdatePipeSlotTOList(_pos, _pipeData);
    }

    public void SetColorDefaulfPipeSlotData()
    {
        pipeManager.SetColorDefaulfPipeSlotData();
    }
    #endregion

    #region DragAndDrop
    #region DragDrop UI
    public GameObject CreatePipeSlotDragDropUI(PipeData _pipeData)
    {
        return dragDropManager.CreatePipeSlotDragDropUI(_pipeData);
    }

    public void SetTargetDragDropUI(PipeSlot _target)
    {
        dragDropManager.SetTargetDragDropUI(_target);
    }

    public float GetRotateFromDirection(Direction _direction)
    {
        return dragDropManager.GetRotateFromDirection(_direction);
    }

    public bool IsDragAndDropingUI()
    {
        return dragDropManager.IsDragAndDropingUI();
    }

    public void OnMouseUpDropUI(PipeData _dropData)
    {
        dragDropManager.OnMouseUpDropUI(_dropData);
    }
    #endregion

    #region DragDrop GameWorld

    public GameObject CreatePipeSlotDragDropGW(PipeData _pipeData)
    {
        return dragDropManager.CreatePipeSlotDragDropGW(_pipeData);
    }

    public void SetCurrentDragDropGW(PipeSlot _target, bool _isDrag)
    {
        dragDropManager.SetDragDropGW(_target, _isDrag);
    }

    public bool IsDragAndDropingGW()
    {
        return dragDropManager.IsDragAndDropingGW();
    }

    public void OnMouseUpDropGW(PipeSlot _dropSlot, PipeData _dropData)
    {
        dragDropManager.OnMouseUpDropGW(_dropSlot, _dropData);
    }
    #endregion
    #endregion
}

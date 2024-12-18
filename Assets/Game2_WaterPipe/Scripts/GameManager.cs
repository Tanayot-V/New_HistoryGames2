using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singletons<GameManager>
{
    public Canvas canvas;
    public GridSystem gridSystem;
    public PipeManager pipeManager;

    public void Start()
    {
        gridSystem.CreateGrid();
        //pipeManager.RandomPipeMap();
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

    public void SetupPipeTypeSlot(PipeData _pipeData, bool _isUI)
    {
        if (_isUI) pipeManager.SetPipeTypeSlotUI(_pipeData);
        else pipeManager.SetupPipeG(_pipeData);
    }

    #region DragDrop UI
    public GameObject CreatePipeSlotDragDropUI(PipeData _pipeData)
    {
        return pipeManager.CreatePipeSlotDragDropUI(_pipeData);
    }

    public void SetTargetDragDropUI(PipeSlot _target)
    {
        pipeManager.SetTargetDragDropUI(_target);
    }

    public float GetRotateFromDirection(Direction _direction)
    {
        return pipeManager.GetRotateFromDirection(_direction);
    }
    #endregion

    #region DragDrop GameWorld

    public GameObject CreatePipeSlotDragDropG(PipeData _pipeData)
    {
        return pipeManager.CreatePipeSlotDragDropG(_pipeData);
    }

    public void SetCurrentDragDropG(PipeSlot _target, bool _isDrag)
    {
        pipeManager.SetDragDropG(_target, _isDrag);
    }

    #endregion
    #endregion
}

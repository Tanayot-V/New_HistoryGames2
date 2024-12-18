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

    public GameObject CreatePipeSlotDragDrop(PipeType _pipeType)
    {
        return pipeManager.CreatePipeSlotDragDrop(_pipeType);
    }

    public Sprite GetPipeModelPicture(PipeType _pipeType)
    {
        return pipeManager.GetPipeModel(_pipeType).picture;
    }

    public void SetTargetDragDrop(PipeSlot _target)
    {
        pipeManager.SetTargetDragDrop(_target);
    }

    public void SetPipeTypeSlot(PipeType _pipeType)
    {
        pipeManager.SetPipeTypeSlot(_pipeType);
    }
    #endregion
}

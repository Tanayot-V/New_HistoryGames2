using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singletons<GameManager>
{
    public GridSystem gridSystem;
    public PipeManager pipeManager;

    public void Start(){}

    public GridSystem GridSystem()
    {
        return DataCenterManager.GetData(ref gridSystem, "GridSystem");
    }

    public PipeManager PipeManager()
    {
        return DataCenterManager.GetData(ref pipeManager, "PipeManager");
    }
}

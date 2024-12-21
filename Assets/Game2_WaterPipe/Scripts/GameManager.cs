using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameManager : Singletons<GameManager>
{
    [SerializeField] private GridSystem gridSystem;
    [SerializeField] private PipeManager pipeManager;
    [SerializeField] private DragDropManager dragDropManager;
    [SerializeField] private GameUiManager gameUiManager;
    [SerializeField] private GridCanvas gridCanvas;

    [Header("Game Data")]
    public float timePlay = 600;
    private float timer;
    public int moveCount = 20;
    public int hammerCount = 5;

    public bool onHammer;
    public GraphicRaycaster uiRaycaster;
    public EventSystem eventSystem;

    public void Start()
    {
        timer = timePlay;
        gridSystem.CreateGrid();
        gameUiManager.Init(1f, moveCount, hammerCount);
    }

    void Update()
    {
        if (timer <= 0)
        {
            // lose game
            return;
        }
        timer -= Time.deltaTime;
        gameUiManager.UpdateTime(timer / timePlay);
        if(onHammer)
        {
            if (Input.GetMouseButtonDown(0))
            {
                CheckClick();
            }
        }
    }

    void CheckClick()
    {
        PointerEventData pointerEventData = new PointerEventData(eventSystem);
        pointerEventData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        uiRaycaster.Raycast(pointerEventData, results);
        
        bool clickedOnPipeObject = false;

       foreach (RaycastResult result in results)
        {
            PipeObject pipeObject = result.gameObject.GetComponent<PipeObject>();
            if (pipeObject != null)
            {
                clickedOnPipeObject = true;
                break;
            }
        }

        if (!clickedOnPipeObject)
        {
            Debug.Log("Clicked on something other than a PipeObject or empty space");
            onHammer = false;
            gameUiManager.OnEndHammer();
        }
    }

    public void UseHammer()
    {
        if (hammerCount <= 0) return;
        // Hammer logic
        onHammer = true;
    }

    public void UseHammerComplete()
    {
        hammerCount--;
        gameUiManager.UpdateHammerCount(hammerCount);
        onHammer = false;
        gameUiManager.OnEndHammer();
    }

    public bool UseMove()
    {
        if (moveCount <= 0)
        {
            // lose game

            return false;
        } 
        moveCount--;
        gameUiManager.UpdateMoveCount(moveCount);
        return true;
    }

    public GridSystem GridSystem()
    {
        return DataCenterManager.GetData(ref gridSystem, "GridSystem");
    }

    public GridCanvas GridCanvas()
    {
        return DataCenterManager.GetData(ref gridCanvas, "PlaySlot");
    }

    #region PipeManager
    public PipeManager PipeManager()
    {
        return DataCenterManager.GetData(ref pipeManager, "PipeManager");
    }

    public GameUiManager GameUiManager()
    {
        return DataCenterManager.GetData(ref gameUiManager, "-- GameManager --");
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

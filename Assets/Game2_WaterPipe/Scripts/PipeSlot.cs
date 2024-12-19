using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PipeSlot : MonoBehaviour
{
    public PipeData pipeData = new PipeData();
    private float targetRotationZ = 0f;
    [SerializeField] SpriteRenderer bgSR;
    [SerializeField] SpriteRenderer pipeSR;
    private Color defalutColor;
    private bool isRotating;
    private bool isDragAndDropingUI;

    private float mouseDownTime;
    private bool isDragging;
    private float holdThreshold = 0.25f;

    private void Start()
    {
        bgSR = this.GetComponent<SpriteRenderer>();
        defalutColor = bgSR.color;
        isDragAndDropingUI = GameManager.Instance.IsDragAndDropingUI();
    }

    private void Update() {}

    public void InitSlot(PipeData _pipeData)
    {
        pipeData = _pipeData;
        pipeData.pipeType = (PipeType)Random.Range(0, 5);
        SetupPipe(pipeData);
    }

    public void SetupPipe(PipeData _pipeData)
    {
        pipeData = _pipeData;
        if (pipeData.pipeType == PipeType.None) pipeSR.sprite = null;
        else pipeSR.sprite = GameManager.Instance.GetPipeModelPicture(pipeData.pipeType);
        pipeSR.transform.rotation = Quaternion.Euler(0, 0, GameManager.Instance.GetRotateFromDirection(pipeData.direction));
        GameManager.Instance.UpdatePipeSlotTOList(_pipeData.pos, _pipeData);
    }

    private void DragDrop()
    {
        if (isDragAndDropingUI) return;
        if (isRotating) return;

        GameManager.Instance.CreatePipeSlotDragDropGW(pipeData);
        GameManager.Instance.SetCurrentDragDropGW(this, true);
        pipeData.pipeType = PipeType.None;
        pipeData.direction = Direction.Up;
        SetupPipe(pipeData);
    }

    private void Rotat()
    {
        if (pipeData.pipeType == PipeType.None) return;
        if (isDragAndDropingUI) return;
        if (isDragging) return;
        if (isRotating) return;

        targetRotationZ += 90f;
        pipeData.direction = (Direction)(((int)pipeData.direction + 1) % 5);
        if (pipeData.direction == Direction.None) pipeData.direction = Direction.Up;

        transform.DORotate(new Vector3(0, 0, targetRotationZ), 0.5f, RotateMode.FastBeyond360)
            .SetEase(Ease.OutQuad)
            .OnStart(() =>
            {
                isRotating = true;
            })
            .OnComplete(() =>
            {
                isRotating = false;
                if (targetRotationZ >= 360) targetRotationZ = 0;
            });
        GameManager.Instance.UpdatePipeSlotTOList(pipeData.pos, pipeData);
    }

    void OnMouseDown()
    {
        if (pipeData.pipeType == PipeType.None) return;
        if (pipeData.pipeType == PipeType.Obstacle) return;

        mouseDownTime = Time.time;
        isDragging = false;
    }

    void OnMouseDrag()
    {
        if (pipeData.pipeType == PipeType.None) return;
        if (pipeData.pipeType == PipeType.Obstacle) return;

        if (Time.time - mouseDownTime >= holdThreshold && !isDragging)
        {
            DragDrop();
            isDragging = true;
        }
    }

    void OnMouseUp()
    {
        if (pipeData.pipeType == PipeType.None) return;
        if (pipeData.pipeType == PipeType.Obstacle) return;

        if (!isDragging)
        {
            Rotat();
        }
    }

    public void OnMouseOver()
    {
        if (pipeData.pipeType == PipeType.Obstacle) return;
        ColorGrey();
    }

    public void OnMouseExit()
    {
        ColorDefalut();
    }

    public void ColorGrey()
    {
        bgSR.color = Color.grey;
    }

    public void ColorGreen()
    {
        bgSR.color = Color.green;
    }

    public void ColorDefalut()
    {
        bgSR.color = defalutColor;
    }
}

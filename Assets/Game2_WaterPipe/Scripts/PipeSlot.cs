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
    private bool isDragAndDroping;

    private void Start()
    {
        bgSR = this.GetComponent<SpriteRenderer>();
        defalutColor = bgSR.color;
        isDragAndDroping = GameManager.Instance.pipeManager.IsDragAndDroping();
    }

    private void Update() {}

    public void InitSlot(PipeData _pipeData)
    {
        pipeData = _pipeData;
        pipeData.pipeType = (PipeType)Random.Range(0, 5);
        ChangePipeType(pipeData.pipeType);
    }

    public void ChangePipeType(PipeType _pipeType)
    {
        pipeData.pipeType = _pipeType;
        if (pipeData.pipeType == PipeType.None) pipeSR.sprite = null;
        else pipeSR.sprite = GameManager.Instance.GetPipeModelPicture(_pipeType);
    }

    public void OnMouseDown()
    {
        if (isDragAndDroping) return;
        if (isRotating) return;
        targetRotationZ += 90f;
        pipeData.direction = (Direction)(((int)pipeData.direction + 1) % 5);

        transform.DORotate(new Vector3(0, 0, targetRotationZ), 0.5f, RotateMode.FastBeyond360)
            .SetEase(Ease.OutQuad)
            .OnStart(() =>
            {
                Debug.Log("Rotation Started");
                isRotating = true;
            })
            .OnComplete(() =>
            {
                Debug.Log("Rotation Completed");
                isRotating = false; // หมุนเสร็จแล้ว
                if (targetRotationZ >= 360) targetRotationZ = 0; // รีเซ็ตมุม
            });


        Debug.Log($"{name}: {pipeData.direction}");

    }

    public void OnMouseOver()
    {
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

using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PipeObject : MonoBehaviour
{
    public PipeData pipeData;
    public bool isRotating = false;
    private float targetRotationZ = 0f;

    public void Rotat()
    {
        if (isRotating) return;

        targetRotationZ -= 90f;
        pipeData.direction = (Direction)(((int)pipeData.direction + 1) % 4);

        transform.DORotate(new Vector3(0, 0, targetRotationZ), 0.5f, RotateMode.Fast)
            .SetEase(Ease.OutQuad)
            .OnStart(() =>
            {
                isRotating = true;
            })
            .OnComplete(() =>
            {
                isRotating = false;
                if (targetRotationZ >= 360) targetRotationZ = 0;
                GameManager.Instance.UseMove();
            });
        GameManager.Instance.UpdatePipeSlotTOList(pipeData.pos, pipeData);
    }
}

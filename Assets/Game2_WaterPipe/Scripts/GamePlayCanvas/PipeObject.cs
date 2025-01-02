using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PipeObject : MonoBehaviour
{
    public PipeData pipeData;
    public bool isRotating = false;
    private float targetRotationZ = 0f;

    public float duration = 0.5f;
    public Image waterFillImg0;
    public Image waterFillImg1;
    private bool isUseWater1 = false;
    private bool isFullWater = false;
    public bool canpass2 = false;

    public bool isWaste = false;
    public Vector2 startPipePos = Vector2.zero;

    private CanvasGroup canvasGroup;
    private DraggableItem draggableItem;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        draggableItem = GetComponent<DraggableItem>();
        PipeSlotCanvas slot = GetComponentInParent<PipeSlotCanvas>();
        if(slot != null) pipeData.pos = slot.pos;
        if(waterFillImg0 != null) waterFillImg0.fillAmount = 0;
        if(waterFillImg1 != null) waterFillImg1.fillAmount = 0;
    }

    void Update()
    {
        if(draggableItem != null)
        {
            if(!draggableItem.isSnapOnSlot)
            {
                canvasGroup.blocksRaycasts = false;
                return;
            }
        }
        if(GameManager.Instance.itemManager.isDrawLine)
        {
            canvasGroup.blocksRaycasts = false;
            canvasGroup.alpha = 0.5f;
        }
        else
        {
            canvasGroup.blocksRaycasts = true;
            canvasGroup.alpha = 1f;
        }
    }

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
                GameManager.Instance.UseMove(1);
            });
        GameManager.Instance.UpdatePipeSlotTOList(pipeData.pos, pipeData);
    }

    public bool WaterIn(Vector2 pos, bool mIsWaste, Vector2 mStartPipePos)
    {
        if(isFullWater) return false;
        GameManager.Instance.RunningWater();
        //Debug.Log($"WaterIn {name} {pipeData.pos} from {pos}");
        isWaste = mIsWaste;
        startPipePos = mStartPipePos;
        if (pipeData.pipeType == PipeType.None || pipeData.pipeType == PipeType.Obstacle
        || pipeData.pipeType == PipeType.Start || pipeData.pipeType == PipeType.Map) 
        return false;

        if (pipeData.pipeType == PipeType.End)
        {
            // change status pipeEnd
            PipeEnd pe = GetComponent<PipeEnd>();
            if(pe != null)
            {
                pe.OnWaterIn(isWaste);
            }
            return true;
        }

        Vector2 dir = pos - pipeData.pos;
        if(dir == Vector2.up)
        {
            if(pipeData.pipeType == PipeType.Straight)
            {
                if(pipeData.direction == Direction.Up || pipeData.direction == Direction.Down)
                {
                    waterFillImg0.fillMethod = Image.FillMethod.Vertical;
                    waterFillImg0.fillOrigin = pipeData.direction == Direction.Up ? (int)Image.OriginVertical.Bottom : (int)Image.OriginVertical.Top;
                    FillWater(Vector2.down);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if(pipeData.pipeType == PipeType.Degree90)
            {
                if(pipeData.direction == Direction.Up)
                {
                    waterFillImg0.fillMethod = Image.FillMethod.Radial90;
                    waterFillImg0.fillOrigin = (int)Image.Origin90.BottomRight;
                    waterFillImg0.fillClockwise = true;
                    FillWater(Vector2.left);
                    return true;
                }
                else if (pipeData.direction == Direction.Right)
                {
                    waterFillImg0.fillMethod = Image.FillMethod.Radial90;
                    waterFillImg0.fillOrigin = (int)Image.Origin90.BottomRight;
                    waterFillImg0.fillClockwise = false;
                    FillWater(Vector2.right);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if(pipeData.pipeType == PipeType.Tee)
            {
                if(pipeData.direction == Direction.Up)
                {
                    waterFillImg0.fillMethod = Image.FillMethod.Vertical;
                    waterFillImg0.fillOrigin = (int)Image.OriginVertical.Bottom;
                    FillWater(Vector2.left,Vector2.right);
                    return true;
                }
                else if(pipeData.direction == Direction.Right)
                {
                    waterFillImg0.fillMethod = Image.FillMethod.Horizontal;
                    waterFillImg0.fillOrigin = (int)Image.OriginHorizontal.Right;
                    FillWater(Vector2.down,Vector2.right);
                    return true;
                }
                else if(pipeData.direction == Direction.Left)
                {
                    waterFillImg0.fillMethod = Image.FillMethod.Horizontal;
                    waterFillImg0.fillOrigin = (int)Image.OriginHorizontal.Left;
                    FillWater(Vector2.down,Vector2.left);
                    return true;
                }
                return false;
            }
            else if(pipeData.pipeType == PipeType.Cross)
            {
                if(pipeData.direction == Direction.Up)
                {
                    waterFillImg0.fillMethod = Image.FillMethod.Vertical;
                    waterFillImg0.fillOrigin = (int)Image.OriginVertical.Bottom;
                }
                else if ( pipeData.direction == Direction.Right)
                {
                    waterFillImg0.fillMethod = Image.FillMethod.Horizontal;
                    waterFillImg0.fillOrigin = (int)Image.OriginHorizontal.Right;
                }
                else if(pipeData.direction == Direction.Left)
                {
                    waterFillImg0.fillMethod = Image.FillMethod.Horizontal;
                    waterFillImg0.fillOrigin = (int)Image.OriginHorizontal.Left;
                }
                else if(pipeData.direction == Direction.Down)
                {
                    waterFillImg0.fillMethod = Image.FillMethod.Vertical;
                    waterFillImg0.fillOrigin = (int)Image.OriginVertical.Top;
                }
                FillWater(Vector2.down,Vector2.left,Vector2.right);
                return true;
            }
            else if(pipeData.pipeType == PipeType.StraightCross)
            {
                if(pipeData.direction == Direction.Up)
                {
                    waterFillImg0.fillMethod = Image.FillMethod.Vertical;
                    waterFillImg0.fillOrigin = (int)Image.OriginVertical.Bottom;
                }
                else if(pipeData.direction == Direction.Down)
                {
                    waterFillImg0.fillMethod = Image.FillMethod.Vertical;
                    waterFillImg0.fillOrigin = (int)Image.OriginVertical.Top;
                }
                else if ( pipeData.direction == Direction.Right)
                {
                    waterFillImg1.fillMethod = Image.FillMethod.Horizontal;
                    waterFillImg1.fillOrigin = (int)Image.OriginHorizontal.Right;
                    isUseWater1 = true;
                }
                else if(pipeData.direction == Direction.Left)
                {
                    waterFillImg1.fillMethod = Image.FillMethod.Horizontal;
                    waterFillImg1.fillOrigin = (int)Image.OriginHorizontal.Left;
                    isUseWater1 = true;
                }
                FillWater(Vector2.down);
                return true;
            }
            else if(pipeData.pipeType == PipeType.Degree90Cross)
            {
                if(pipeData.direction == Direction.Up)
                {
                    waterFillImg0.fillMethod = Image.FillMethod.Radial90;
                    waterFillImg0.fillOrigin = (int)Image.Origin90.BottomRight;
                    waterFillImg0.fillClockwise = true;
                    FillWater(Vector2.left);
                }
                else if(pipeData.direction == Direction.Right)
                {
                    waterFillImg0.fillMethod = Image.FillMethod.Radial90;
                    waterFillImg0.fillOrigin = (int)Image.Origin90.BottomRight;
                    waterFillImg0.fillClockwise = false;
                    FillWater(Vector2.right);
                }
                else if(pipeData.direction == Direction.Down)
                {
                    waterFillImg1.fillMethod = Image.FillMethod.Radial90;
                    waterFillImg1.fillOrigin = (int)Image.Origin90.BottomRight;
                    waterFillImg1.fillClockwise = true;
                    FillWater(Vector2.left);
                    isUseWater1 = true;
                }
                else if(pipeData.direction == Direction.Left)
                {
                    waterFillImg1.fillMethod = Image.FillMethod.Radial90;
                    waterFillImg1.fillOrigin = (int)Image.Origin90.BottomRight;
                    waterFillImg1.fillClockwise = false;
                    FillWater(Vector2.right);
                    isUseWater1 = true;
                }
                return true;
            }
            return false;
        }
        else if(dir == Vector2.down)
        {
            if(pipeData.pipeType == PipeType.Straight)
            {
                if(pipeData.direction == Direction.Up || pipeData.direction == Direction.Down)
                {
                    waterFillImg0.fillMethod = Image.FillMethod.Vertical;
                    waterFillImg0.fillOrigin = pipeData.direction == Direction.Up ? (int)Image.OriginVertical.Top : (int)Image.OriginVertical.Bottom;
                    FillWater(Vector2.up);
                    return true;
                }
                return false;
            }
            else if(pipeData.pipeType == PipeType.Degree90)
            {
                if(pipeData.direction == Direction.Down)
                {
                    waterFillImg0.fillMethod = Image.FillMethod.Radial90;
                    waterFillImg0.fillOrigin = (int)Image.Origin90.BottomRight;
                    waterFillImg0.fillClockwise = true;
                    FillWater(Vector2.right);
                    return true;
                }
                else if (pipeData.direction == Direction.Left)
                {
                    waterFillImg0.fillMethod = Image.FillMethod.Radial90;
                    waterFillImg0.fillOrigin = (int)Image.Origin90.BottomRight;
                    waterFillImg0.fillClockwise = false;
                    FillWater(Vector2.left);
                    return true;
                }
                return false;
            }
            else if(pipeData.pipeType == PipeType.Tee)
            {
                if(pipeData.direction == Direction.Down)
                {
                    waterFillImg0.fillMethod = Image.FillMethod.Vertical;
                    waterFillImg0.fillOrigin = (int)Image.OriginVertical.Bottom;
                    FillWater(Vector2.left,Vector2.right);
                    return true;
                }
                else if(pipeData.direction == Direction.Right)
                {
                    waterFillImg0.fillMethod = Image.FillMethod.Horizontal;
                    waterFillImg0.fillOrigin = (int)Image.OriginHorizontal.Left;
                    FillWater(Vector2.up,Vector2.right);
                    return true;
                }
                else if(pipeData.direction == Direction.Left)
                {
                    waterFillImg0.fillMethod = Image.FillMethod.Horizontal;
                    waterFillImg0.fillOrigin = (int)Image.OriginHorizontal.Right;
                    FillWater(Vector2.up,Vector2.left);
                    return true;
                }
                return false;
            }
            else if(pipeData.pipeType == PipeType.Cross)
            {
                if(pipeData.direction == Direction.Up)
                {
                    waterFillImg0.fillMethod = Image.FillMethod.Vertical;
                    waterFillImg0.fillOrigin = (int)Image.OriginVertical.Top;
                }
                else if ( pipeData.direction == Direction.Right)
                {
                    waterFillImg0.fillMethod = Image.FillMethod.Horizontal;
                    waterFillImg0.fillOrigin = (int)Image.OriginHorizontal.Left;
                }
                else if(pipeData.direction == Direction.Left)
                {
                    waterFillImg0.fillMethod = Image.FillMethod.Horizontal;
                    waterFillImg0.fillOrigin = (int)Image.OriginHorizontal.Right;
                }
                else if(pipeData.direction == Direction.Down)
                {
                    waterFillImg0.fillMethod = Image.FillMethod.Vertical;
                    waterFillImg0.fillOrigin = (int)Image.OriginVertical.Bottom;
                }
                FillWater(Vector2.up,Vector2.left,Vector2.right);
                return true;
            }
            else if(pipeData.pipeType == PipeType.StraightCross)
            {
                if(pipeData.direction == Direction.Up)
                {
                    waterFillImg0.fillMethod = Image.FillMethod.Vertical;
                    waterFillImg0.fillOrigin = (int)Image.OriginVertical.Top;
                }
                else if(pipeData.direction == Direction.Down)
                {
                    waterFillImg0.fillMethod = Image.FillMethod.Vertical;
                    waterFillImg0.fillOrigin = (int)Image.OriginVertical.Bottom;
                }
                else if ( pipeData.direction == Direction.Right)
                {
                    waterFillImg1.fillMethod = Image.FillMethod.Horizontal;
                    waterFillImg1.fillOrigin = (int)Image.OriginHorizontal.Left;
                    isUseWater1 = true;
                }
                else if(pipeData.direction == Direction.Left)
                {
                    waterFillImg1.fillMethod = Image.FillMethod.Horizontal;
                    waterFillImg1.fillOrigin = (int)Image.OriginHorizontal.Right;
                    isUseWater1 = true;
                }
                FillWater(Vector2.up);
                return true;
            }
            else if(pipeData.pipeType == PipeType.Degree90Cross)
            {
                if(pipeData.direction == Direction.Up)
                {
                    waterFillImg1.fillMethod = Image.FillMethod.Radial90;
                    waterFillImg1.fillOrigin = (int)Image.Origin90.BottomRight;
                    waterFillImg1.fillClockwise = true;
                    FillWater(Vector2.right);
                    isUseWater1 = true;
                }
                else if(pipeData.direction == Direction.Right)
                {
                    waterFillImg1.fillMethod = Image.FillMethod.Radial90;
                    waterFillImg1.fillOrigin = (int)Image.Origin90.BottomRight;
                    waterFillImg1.fillClockwise = false;
                    FillWater(Vector2.left);
                    isUseWater1 = true;
                }
                else if(pipeData.direction == Direction.Down)
                {
                    waterFillImg0.fillMethod = Image.FillMethod.Radial90;
                    waterFillImg0.fillOrigin = (int)Image.Origin90.BottomRight;
                    waterFillImg0.fillClockwise = true;
                    FillWater(Vector2.right);
                }
                else if(pipeData.direction == Direction.Left)
                {
                    waterFillImg0.fillMethod = Image.FillMethod.Radial90;
                    waterFillImg0.fillOrigin = (int)Image.Origin90.BottomRight;
                    waterFillImg0.fillClockwise = false;
                    FillWater(Vector2.left);
                }
                return true;
            }
            return false;
        }
        else if(dir == Vector2.right)
        {
            if(pipeData.pipeType == PipeType.Straight)
            {
                if(pipeData.direction == Direction.Right || pipeData.direction == Direction.Left)
                {
                    waterFillImg0.fillMethod = Image.FillMethod.Vertical;
                    waterFillImg0.fillOrigin = pipeData.direction == Direction.Left ? (int)Image.OriginVertical.Top : (int)Image.OriginVertical.Bottom;
                    FillWater(Vector2.left);
                    return true;
                }
                return false;
            }
            else if(pipeData.pipeType == PipeType.Degree90)
            {
                if(pipeData.direction == Direction.Down)
                {
                    waterFillImg0.fillMethod = Image.FillMethod.Radial90;
                    waterFillImg0.fillOrigin = (int)Image.Origin90.BottomRight;
                    waterFillImg0.fillClockwise = false;
                    FillWater(Vector2.down);
                    return true;
                }
                else if (pipeData.direction == Direction.Right)
                {
                    waterFillImg0.fillMethod = Image.FillMethod.Radial90;
                    waterFillImg0.fillOrigin = (int)Image.Origin90.BottomRight;
                    waterFillImg0.fillClockwise = true;
                    FillWater(Vector2.up);
                    return true;
                }
                return false;
            }
            else if(pipeData.pipeType == PipeType.Tee)
            {
                if(pipeData.direction == Direction.Down)
                {
                    waterFillImg0.fillMethod = Image.FillMethod.Horizontal;
                    waterFillImg0.fillOrigin = (int)Image.OriginHorizontal.Right;
                    FillWater(Vector2.left,Vector2.down);
                    return true;
                }
                else if(pipeData.direction == Direction.Right)
                {
                    waterFillImg0.fillMethod = Image.FillMethod.Vertical;
                    waterFillImg0.fillOrigin = (int)Image.OriginVertical.Bottom;
                    FillWater(Vector2.up,Vector2.down);
                    return true;
                }
                else if(pipeData.direction == Direction.Up)
                {
                    waterFillImg0.fillMethod = Image.FillMethod.Horizontal;
                    waterFillImg0.fillOrigin = (int)Image.OriginHorizontal.Left;
                    FillWater(Vector2.up,Vector2.left);
                    return true;
                }
                return false;
            }
            else if(pipeData.pipeType == PipeType.Cross)
            {
                if(pipeData.direction == Direction.Up)
                {
                    waterFillImg0.fillMethod = Image.FillMethod.Horizontal;
                    waterFillImg0.fillOrigin = (int)Image.OriginHorizontal.Left;
                }
                else if ( pipeData.direction == Direction.Right)
                {
                    waterFillImg0.fillMethod = Image.FillMethod.Vertical;
                    waterFillImg0.fillOrigin = (int)Image.OriginVertical.Bottom;
                }
                else if(pipeData.direction == Direction.Left)
                {
                    waterFillImg0.fillMethod = Image.FillMethod.Vertical;
                    waterFillImg0.fillOrigin = (int)Image.OriginVertical.Top;
                }
                else if(pipeData.direction == Direction.Down)
                {
                    waterFillImg0.fillMethod = Image.FillMethod.Horizontal;
                    waterFillImg0.fillOrigin = (int)Image.OriginHorizontal.Right;
                }
                FillWater(Vector2.up,Vector2.left,Vector2.down);
                return true;
            }
            else if(pipeData.pipeType == PipeType.StraightCross)
            {
                if(pipeData.direction == Direction.Up)
                {
                    waterFillImg1.fillMethod = Image.FillMethod.Horizontal;
                    waterFillImg1.fillOrigin = (int)Image.OriginHorizontal.Left;
                    isUseWater1 = true;
                }
                else if(pipeData.direction == Direction.Down)
                {
                    waterFillImg0.fillMethod = Image.FillMethod.Horizontal;
                    waterFillImg0.fillOrigin = (int)Image.OriginHorizontal.Right;
                    isUseWater1 = true;
                }
                else if ( pipeData.direction == Direction.Right)
                {
                    waterFillImg0.fillMethod = Image.FillMethod.Vertical;
                    waterFillImg0.fillOrigin = (int)Image.OriginVertical.Bottom;
                }
                else if(pipeData.direction == Direction.Left)
                {
                    waterFillImg0.fillMethod = Image.FillMethod.Vertical;
                    waterFillImg0.fillOrigin = (int)Image.OriginVertical.Top;
                }
                FillWater(Vector2.left);
                return true;
            }
            else if(pipeData.pipeType == PipeType.Degree90Cross)
            {
                if(pipeData.direction == Direction.Up)
                {
                    waterFillImg1.fillMethod = Image.FillMethod.Radial90;
                    waterFillImg1.fillOrigin = (int)Image.Origin90.BottomRight;
                    waterFillImg1.fillClockwise = false;
                    FillWater(Vector2.down);
                    isUseWater1 = true;
                }
                else if(pipeData.direction == Direction.Right)
                {
                    waterFillImg0.fillMethod = Image.FillMethod.Radial90;
                    waterFillImg0.fillOrigin = (int)Image.Origin90.BottomRight;
                    waterFillImg0.fillClockwise = true;
                    FillWater(Vector2.up);
                }
                else if(pipeData.direction == Direction.Down)
                {
                    waterFillImg0.fillMethod = Image.FillMethod.Radial90;
                    waterFillImg0.fillOrigin = (int)Image.Origin90.BottomRight;
                    waterFillImg0.fillClockwise = false;
                    FillWater(Vector2.down);
                }
                else if(pipeData.direction == Direction.Left)
                {
                    waterFillImg1.fillMethod = Image.FillMethod.Radial90;
                    waterFillImg1.fillOrigin = (int)Image.Origin90.BottomRight;
                    waterFillImg1.fillClockwise = true;
                    FillWater(Vector2.up);
                    isUseWater1 = true;
                }
                return true;
            }
            return false;
        }
        else if(dir == Vector2.left)
        {
            if(pipeData.pipeType == PipeType.Straight)
            {
                if(pipeData.direction == Direction.Right || pipeData.direction == Direction.Left)
                {
                    waterFillImg0.fillMethod = Image.FillMethod.Vertical;
                    waterFillImg0.fillOrigin = pipeData.direction == Direction.Right ? (int)Image.OriginVertical.Top : (int)Image.OriginVertical.Bottom;
                    FillWater(Vector2.right);
                    return true;
                }
                return false;
            }
            else if(pipeData.pipeType == PipeType.Degree90)
            {
                if(pipeData.direction == Direction.Left)
                {
                    waterFillImg0.fillMethod = Image.FillMethod.Radial90;
                    waterFillImg0.fillOrigin = (int)Image.Origin90.BottomRight;
                    waterFillImg0.fillClockwise = true;
                    FillWater(Vector2.down);
                    return true;
                }
                else if (pipeData.direction == Direction.Up)
                {
                    waterFillImg0.fillMethod = Image.FillMethod.Radial90;
                    waterFillImg0.fillOrigin = (int)Image.Origin90.BottomRight;
                    waterFillImg0.fillClockwise = false;
                    FillWater(Vector2.up);
                    return true;
                }
                return false;
            }
            else if(pipeData.pipeType == PipeType.Tee)
            {
                if(pipeData.direction == Direction.Down)
                {
                    waterFillImg0.fillMethod = Image.FillMethod.Horizontal;
                    waterFillImg0.fillOrigin = (int)Image.OriginHorizontal.Left;
                    FillWater(Vector2.right,Vector2.down);
                    return true;
                }
                else if(pipeData.direction == Direction.Left)
                {
                    waterFillImg0.fillMethod = Image.FillMethod.Vertical;
                    waterFillImg0.fillOrigin = (int)Image.OriginVertical.Bottom;
                    FillWater(Vector2.up,Vector2.down);
                    return true;
                }
                else if(pipeData.direction == Direction.Up)
                {
                    waterFillImg0.fillMethod = Image.FillMethod.Horizontal;
                    waterFillImg0.fillOrigin = (int)Image.OriginHorizontal.Right;
                    FillWater(Vector2.right,Vector2.up);
                    return true;
                }
                return false;
            }
            else if(pipeData.pipeType == PipeType.Cross)
            {
                if(pipeData.direction == Direction.Up)
                {
                    waterFillImg0.fillMethod = Image.FillMethod.Horizontal;
                    waterFillImg0.fillOrigin = (int)Image.OriginHorizontal.Right;
                }
                else if ( pipeData.direction == Direction.Right)
                {
                    waterFillImg0.fillMethod = Image.FillMethod.Vertical;
                    waterFillImg0.fillOrigin = (int)Image.OriginVertical.Top;
                }
                else if(pipeData.direction == Direction.Left)
                {
                    waterFillImg0.fillMethod = Image.FillMethod.Vertical;
                    waterFillImg0.fillOrigin = (int)Image.OriginVertical.Bottom;
                }
                else if(pipeData.direction == Direction.Down)
                {
                    waterFillImg0.fillMethod = Image.FillMethod.Horizontal;
                    waterFillImg0.fillOrigin = (int)Image.OriginHorizontal.Left;
                }
                FillWater(Vector2.right,Vector2.up,Vector2.down);
                return true;
            }
            else if(pipeData.pipeType == PipeType.StraightCross)
            {
                if(pipeData.direction == Direction.Up)
                {
                    waterFillImg1.fillMethod = Image.FillMethod.Horizontal;
                    waterFillImg1.fillOrigin = (int)Image.OriginHorizontal.Right;
                    isUseWater1 = true;
                }
                else if(pipeData.direction == Direction.Down)
                {
                    waterFillImg0.fillMethod = Image.FillMethod.Horizontal;
                    waterFillImg0.fillOrigin = (int)Image.OriginHorizontal.Left;
                    isUseWater1 = true;
                }
                else if ( pipeData.direction == Direction.Right)
                {
                    waterFillImg0.fillMethod = Image.FillMethod.Vertical;
                    waterFillImg0.fillOrigin = (int)Image.OriginVertical.Top;
                }
                else if(pipeData.direction == Direction.Left)
                {
                    waterFillImg0.fillMethod = Image.FillMethod.Vertical;
                    waterFillImg0.fillOrigin = (int)Image.OriginVertical.Bottom;
                }
                FillWater(Vector2.right);
                return true;
            }
            else if(pipeData.pipeType == PipeType.Degree90Cross)
            {
                if(pipeData.direction == Direction.Up)
                {
                    waterFillImg0.fillMethod = Image.FillMethod.Radial90;
                    waterFillImg0.fillOrigin = (int)Image.Origin90.BottomRight;
                    waterFillImg0.fillClockwise = false;
                    FillWater(Vector2.up);
                }
                else if(pipeData.direction == Direction.Right)
                {
                    waterFillImg1.fillMethod = Image.FillMethod.Radial90;
                    waterFillImg1.fillOrigin = (int)Image.Origin90.BottomRight;
                    waterFillImg1.fillClockwise = true;
                    FillWater(Vector2.down);
                    isUseWater1 = true;
                }
                else if(pipeData.direction == Direction.Down)
                {
                    waterFillImg1.fillMethod = Image.FillMethod.Radial90;
                    waterFillImg1.fillOrigin = (int)Image.Origin90.BottomRight;
                    waterFillImg1.fillClockwise = false;
                    FillWater(Vector2.up);
                    isUseWater1 = true;
                }
                else if(pipeData.direction == Direction.Left)
                {
                    waterFillImg0.fillMethod = Image.FillMethod.Radial90;
                    waterFillImg0.fillOrigin = (int)Image.Origin90.BottomRight;
                    waterFillImg0.fillClockwise = true;
                    FillWater(Vector2.down);
                }
                return true;
            }
            return false;
        }
        return false;
    }

    private IEnumerator FillWaterCoroutine(Action callback)
    {
        if(canpass2)
        {
            canpass2 = false;
        }
        else
        {
            isFullWater = true;
        }
        float time = 0;
        while(time <= duration)
        {
            time += Time.deltaTime;
            if(isUseWater1)
            {
                if(isWaste)
                {
                    waterFillImg1.color = GameManager.Instance.pipeManager.wasteColor;
                }
                waterFillImg1.fillAmount = time / duration;
            }
            else
            {
                if(isWaste)
                {
                    waterFillImg0.color = GameManager.Instance.pipeManager.wasteColor;
                }
                waterFillImg0.fillAmount = time / duration;
            }
            yield return null;
        }
        callback?.Invoke();
    }

    private void FillWater(params Vector2[] dirs)
    {
        StartCoroutine(FillWaterCoroutine(() =>
        {
            WaterOut(dirs);
        }));
    }

    public void WaterOut(params Vector2[] dirs)
    {
        for (int i = 0; i < dirs.Length; i++)
        {
            Vector2 nextPipePos = pipeData.pos + dirs[i];
            //Debug.Log($"WaterOut {name} {pipeData.pos} to {nextPipePos}");
            if(GameManager.Instance.gridCanvas.slots[(int)nextPipePos.x, (int)nextPipePos.y].item == null) continue;
            PipeObject next = GameManager.Instance.gridCanvas.slots[(int)nextPipePos.x, (int)nextPipePos.y].item.GetComponent<PipeObject>();
            if(next != null)
            {
                next.WaterIn(pipeData.pos, isWaste, startPipePos);
            }
        }
    }
}

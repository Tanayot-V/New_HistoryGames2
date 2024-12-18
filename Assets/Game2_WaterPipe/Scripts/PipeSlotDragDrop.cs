using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class PipeSlotDragDrop : MonoBehaviour
{
    [SerializeField] private PipeData pipeData;
    [SerializeField] private SpriteRenderer pipeSR;
    [SerializeField] private Vector3 offset;
    [SerializeField] private PipeSlot triggerObj;

    [SerializeField] private List<Transform> hitLists = new List<Transform>();
    [SerializeField] private LayerMask dropMask;
    [SerializeField] private float targetRange;

    public void Update()
    {
        transform.position = GetMouseWorldPos() + offset;
        FindTargetDrop();
        if (triggerObj != null)
        {
            GameManager.Instance.pipeManager.SetColorDefaulfPipeSlotData();
            triggerObj.ColorGreen();
        }
        if (Input.GetMouseButtonUp(0))
        {
            OnMouseUpSlot();
        }
    }

    public void InitSlot(PipeData _pipeData)
    {
        pipeData = _pipeData;
        pipeSR.sprite = GameManager.Instance.GetPipeModelPicture(_pipeData.pipeType);
        this.transform.rotation = Quaternion.Euler(0,0,GameManager.Instance.GetRotateFromDirection(_pipeData.direction));
    }

    public void OnMouseUpSlot()
    {
        Debug.Log(name + ": OnMouseUp");
        GameManager.Instance.SetupPipeTypeSlot(pipeData, true);
        GameManager.Instance.pipeManager.SetColorDefaulfPipeSlotData();
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
    }

    private void OnTriggerExit2D(Collider2D other)
    {
    }

    public void FindTargetDrop()
    {
        hitLists.Clear();
        Collider2D[] hits = Physics2D.OverlapBoxAll(
             this.transform.position,
             targetRange * (Vector2)transform.localScale,
             0f,
             dropMask
         );

        if (hits.Length > 0)
        {
            hits.ToList().ForEach(o =>
            {
                if (o.transform != null && o.transform.gameObject != null)
                {
                    hitLists.Add(o.transform);
                }
            });

            hitLists.RemoveAll(item => item == null || item.gameObject == null);

            if (hitLists.Count > 0)
            {
                triggerObj = hitLists[hitLists.Count-1].GetComponent<PipeSlot>();
                GameManager.Instance.SetTargetDragDropUI(triggerObj);
            }
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.cyan;
        Handles.DrawWireDisc(this.transform.position, transform.forward, targetRange);
    }
#endif

    public Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePoint);
        worldPosition.z = 0f;
        return worldPosition;
    }
}

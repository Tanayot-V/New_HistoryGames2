using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening.Core.Easing;
using UnityEditor;
using UnityEngine;

public class DragDropPipeSlot : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    public PipeData pipeData;
    [SerializeField] private SpriteRenderer pipeSR;
    [SerializeField] private Vector3 offset;
    [SerializeField] private PipeSlot triggerObj;

    [SerializeField] private List<Transform> hitLists = new List<Transform>();
    [SerializeField] private LayerMask dropMask;
    [SerializeField] private float targetRange;

    private void Start()
    {
        gameManager = GameManager.Instance;
    }
    public void Update()
    {
        transform.position = GetMouseWorldPos() + offset;
        FindTargetDrop();
        if (triggerObj != null)
        {
            gameManager.SetColorDefaulfPipeSlotData();
            triggerObj.ColorGreen();
        }
        if (Input.GetMouseButtonUp(0))
        {
            OnMouseUpSlot();
        }
    }

    public void InitSlot(PipeData _pipeData)
    {
        if (gameManager == null) gameManager = GameManager.Instance;
        pipeData = _pipeData;
        pipeSR.sprite = gameManager.GetPipeModelPicture(_pipeData.pipeType);
        this.transform.rotation = Quaternion.Euler(0,0, gameManager.GetRotateFromDirection(_pipeData.direction));
    }

    public void OnMouseUpSlot()
    {
        Debug.Log(name + ": OnMouseUp");
        gameManager.SetColorDefaulfPipeSlotData();
        //Set dropslot DDUI
        gameManager.OnMouseUpDropUI(pipeData);
        //Set dropslot DDGW
        //gameManager.OnMouseUpDropGW(triggerObj, pipeData);
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
                gameManager.SetTargetDragDropUI(triggerObj);
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

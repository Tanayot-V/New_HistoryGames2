using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class PipeSlotDragDrop : MonoBehaviour
{
    [SerializeField] private Vector3 offset;
    public PipeSlot triggerObj;

    public List<Transform> hitListsEntity = new List<Transform>();
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private float targetRange;

    public void Update()
    {
        transform.position = GetMouseWorldPos() + offset;
        FindTargetEnemy();
        if (triggerObj != null)
        {
            GameManager.Instance.pipeManager.SetColorDefaulfPipeSlotData();
            triggerObj.ColorRed();
        }
    }

    public void OnMouseUpSlot()
    {
        Debug.Log(name + ": OnMouseUp");
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        /*
        GameManager.Instance.pipeManager.SetColorDefaulfPipeSlotData();
        triggerObj = other.GetComponent<PipeSlot>();
        triggerObj.ColorRed();

        if (other.CompareTag("Empty"))
        {
            Debug.Log($"{name}: Collided with Empty Tag");

        }*/
    }

    private void OnTriggerExit2D(Collider2D other)
    {
    }

    public void FindTargetEnemy()
    {
        hitListsEntity.Clear();
        Collider2D[] hits = Physics2D.OverlapBoxAll(
             this.transform.position,
             targetRange * (Vector2)transform.localScale,
             0f,
             enemyMask
         );

        if (hits.Length > 0)
        {
            hits.ToList().ForEach(o =>
            {
                if (o.transform != null && o.transform.gameObject != null)
                {
                    hitListsEntity.Add(o.transform);
                }
            });

            hitListsEntity.RemoveAll(item => item == null || item.gameObject == null);

            if (hitListsEntity.Count > 0)
            {
                triggerObj = hitListsEntity[hitListsEntity.Count-1].GetComponent<PipeSlot>();
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

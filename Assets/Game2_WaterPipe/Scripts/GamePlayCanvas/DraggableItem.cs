using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    public bool isSnapOnSlot = false;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector3 offset;

    PipeObject pipeObject;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        pipeObject = GetComponent<PipeObject>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(pipeObject.isRotating || isSnapOnSlot) return;
        transform.SetParent(transform.root);
        canvasGroup.blocksRaycasts = false;
        offset = rectTransform.position - Camera.main.ScreenToWorldPoint(eventData.position);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(pipeObject.isRotating || isSnapOnSlot) return;
        Vector3 newPosition = Camera.main.ScreenToWorldPoint(eventData.position) + offset;
        rectTransform.position = new Vector3(newPosition.x, newPosition.y, rectTransform.position.z);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        if (!isSnapOnSlot)
        {
            Destroy(gameObject);
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (pipeObject != null)
        {
            if(GameManager.Instance.onHammer)
            {
                GameManager.Instance.GridCanvas().slots[(int)pipeObject.pipeData.pos.x, (int)pipeObject.pipeData.pos.y].item = null;
                Destroy(gameObject);
                GameManager.Instance.UseHammerComplete();
            }
            else
            {
                pipeObject.Rotat();
            }
        }
    }

    public void SnapToSlot()
    {
        isSnapOnSlot = true;
    }
}
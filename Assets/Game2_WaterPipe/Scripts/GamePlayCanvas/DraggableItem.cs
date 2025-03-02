using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    public bool isSnapOnSlot = false;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector3 offset;
    public int placeCount = 1;

    PipeObject pipeObject;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        pipeObject = GetComponent<PipeObject>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(pipeObject.isRotating || isSnapOnSlot)
        {
            eventData.pointerDrag = null;
            return;
        } 
        transform.SetParent(transform.root);
        canvasGroup.blocksRaycasts = false;
        offset = rectTransform.position - Camera.main.ScreenToWorldPoint(eventData.position);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(pipeObject.isRotating || isSnapOnSlot)
        {
            eventData.pointerDrag = null;
            return;
        } 
        Vector3 newPosition = Camera.main.ScreenToWorldPoint(eventData.position) + offset;
        rectTransform.position = new Vector3(newPosition.x, newPosition.y, rectTransform.position.z);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(!PlayerPrefs.HasKey("firstWasteTank"))
        {
            GameManager.Instance.isStartGame = false;
            AdsManager.Instance.OpenAdsVideo(AdsType.WastWaterMove, () => {GameManager.Instance.isStartGame = true;});

            PlayerPrefs.SetInt("firstWasteTank",0);
        }
        canvasGroup.blocksRaycasts = true;
        if (!isSnapOnSlot)
        {
            Destroy(gameObject);
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if(GameManager.Instance.isGameEnd || !GameManager.Instance.isStartGame || GameManager.Instance.isRunWater) return;
        if (pipeObject != null)
        {
            if(pipeObject.pipeData.pipeType == PipeType.Road) return;
            if(GameManager.Instance.itemManager.onHammer)
            {
                GameManager.Instance.gridCanvas.slots[(int)pipeObject.pipeData.pos.x, (int)pipeObject.pipeData.pos.y].item = null;
                Destroy(gameObject);
                GameManager.Instance.itemManager.UseHammerComplete();
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
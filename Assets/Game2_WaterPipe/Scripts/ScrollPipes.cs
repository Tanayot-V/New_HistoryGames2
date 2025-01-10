using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollPipes : MonoBehaviour
{
    public ScrollRect scrollRect;

    private bool isDragging = false;
    private Vector2 startTouchPosition;
    private Vector2 currentTouchPosition;
    void Update()
    {
        if(!UiController.Instance.IsPointerOverGameObjectWithTag("Menu")) return;
        if (Input.GetMouseButtonDown(0) || Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            isDragging = true;
            startTouchPosition = Input.mousePosition;
        }

        if (isDragging && (Input.GetMouseButton(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)))
        {
            currentTouchPosition = Input.mousePosition;

            Vector2 dragDelta = currentTouchPosition - startTouchPosition;
            scrollRect.content.anchoredPosition += new Vector2(dragDelta.x, 0);
            startTouchPosition = currentTouchPosition;
        }

        if (Input.GetMouseButtonUp(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended))
        {
            isDragging = false;
        }
    }
}

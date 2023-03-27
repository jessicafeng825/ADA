using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class ClueOnBoardDrag : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IBeginDragHandler, IEndDragHandler
{
    private string clueID;

    [SerializeField]
    private Canvas canvas;
    
    [SerializeField]
    private RectTransform movableArea;
    private float movableAreaMaxHeight, movableAreaMinHeight, movableAreaMaxWidth, movableAreaMinWidth;
    private Vector2 newPosition;
    private float canvasWidth, canvasHeight;

    // Mouse over to open clue detail
    private bool mouseHoldToOpen, isDragging;
    [SerializeField]
    private float pressedTimeRequired;
    [SerializeField]
    private float timer;

    private void Start()
    {
        canvasWidth = canvas.GetComponent<RectTransform>().rect.width;
        canvasHeight = canvas.GetComponent<RectTransform>().rect.height;
        movableArea = transform.parent.GetComponent<RectTransform>();
        movableAreaMaxHeight = (movableArea.rect.height + movableArea.offsetMin.y) * movableArea.transform.lossyScale.y;
        movableAreaMinHeight = movableArea.offsetMin.y * movableArea.transform.lossyScale.y;
        movableAreaMaxWidth = movableArea.rect.width * movableArea.transform.lossyScale.x;
        movableAreaMinWidth = 0;
        timer = 0;
    }

    private void Update()
    {
        if (mouseHoldToOpen && Input.GetMouseButton(0) && !isDragging)
        {
            timer += Time.deltaTime;
        }

        if(timer > pressedTimeRequired)
        {
            DetectiveBoardManager.Instance.OpenClueInfoOnBoard(clueID, transform.position);
            timer = 0;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        mouseHoldToOpen = true;
        // high light ui
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouseHoldToOpen = false;
        timer = 0;
        // cancel highlight
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isDragging)
        {
            DetectiveBoardManager.Instance.ClueSelected(clueID);
        }
    }

    #region Dragging Related Functions
    // Function to move clue on board
    public void OnBeginDrag(PointerEventData eventData)
    {
        isDragging = true;
        mouseHoldToOpen = false;
    }

    public void DragHandler(BaseEventData eventData)
    {
        PointerEventData pointerEventData = (PointerEventData)eventData;

        if (InCanvasRegion(pointerEventData.position))
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform)canvas.transform, pointerEventData.position, canvas.worldCamera, out newPosition);
            transform.position = canvas.transform.TransformPoint(newPosition);
            //DetectiveBoardManager.Instance.UpdateLinesConnection(clueID, newPosition);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;
    }
    #endregion

    public void SetCanvas(Canvas mainCanvas)
    {
        canvas = mainCanvas;
    }

    private bool InCanvasRegion(Vector3 position)
    {
        if (position.y < movableAreaMaxHeight && position.y > movableAreaMinHeight &&
            position.x < movableAreaMaxWidth && position.x > movableAreaMinWidth)
        {
            return true;
        }
        return false;
    }

    public void SetClueID(string givenClueID)
    {
        clueID = givenClueID;
    }

    public string GetClueID()
    {
        return clueID;
    }
}

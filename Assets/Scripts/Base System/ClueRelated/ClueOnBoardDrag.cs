using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class ClueOnBoardDrag : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
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
    private bool mouseOver;
    [SerializeField]
    private float waitForSeconds;
    [SerializeField]
    private float timer;

    private void Start()
    {
        canvasWidth = canvas.GetComponent<RectTransform>().rect.width;
        canvasHeight = canvas.GetComponent<RectTransform>().rect.height;
        movableArea = this.transform.parent.GetComponent<RectTransform>();
        movableAreaMaxHeight = (movableArea.rect.height + movableArea.offsetMin.y) * movableArea.transform.lossyScale.y;
        movableAreaMinHeight = movableArea.offsetMin.y * movableArea.transform.lossyScale.y;
        movableAreaMaxWidth = movableArea.rect.width * movableArea.transform.lossyScale.x;
        movableAreaMinWidth = 0;
        timer = 0;
    }

    private void Update()
    {
        if (mouseOver)
        {
            timer += Time.deltaTime;
        }

        // call detectiveboardManager to open clue after several seconds
        if (timer >= waitForSeconds)
        {
            DetectiveBoardManager.Instance.OpenClueInfoOnBoard(clueID, transform.position); 
        }

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        mouseOver = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouseOver = false;
        timer = 0;
    }

    public void DragHandler(BaseEventData eventData)
    {
        mouseOver = false;
        PointerEventData pointerEventData = (PointerEventData)eventData;

        if (InCanvasRegion(pointerEventData.position))
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform)canvas.transform, pointerEventData.position, canvas.worldCamera, out newPosition);
            transform.position = canvas.transform.TransformPoint(newPosition);
        }
    }

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

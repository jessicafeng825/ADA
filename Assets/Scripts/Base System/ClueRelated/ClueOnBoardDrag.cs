using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class ClueOnBoardDrag : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
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
        movableArea = transform.parent.GetComponent<RectTransform>();
        movableAreaMaxHeight = (movableArea.rect.height + movableArea.offsetMin.y) * movableArea.transform.lossyScale.y;
        movableAreaMinHeight = movableArea.offsetMin.y * movableArea.transform.lossyScale.y;
        movableAreaMaxWidth = movableArea.rect.width * movableArea.transform.lossyScale.x;
        movableAreaMinWidth = 0;
    }

    private void Update()
    {

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // high light ui
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // cancel highlight
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.clickCount == 1)
        {
            // select this game object as first game object in dbm
            if (DetectiveBoardManager.Instance.GetFirstSelectedClue() == null)
            {
                DetectiveBoardManager.Instance.SetFirstSelectedClue(clueID);
            }
            else if (DetectiveBoardManager.Instance.GetSecondSelectedClue() == null)
            {
                DetectiveBoardManager.Instance.SetSecondSelectedClue(clueID);
            }
            else
            {
                DetectiveBoardManager.Instance.SetSecondSelectedClue(null);
            }
            
        }
        else if (eventData.clickCount == 2)
        {
            DetectiveBoardManager.Instance.OpenClueInfoOnBoard(clueID, transform.position);
        }
    }

    public void DragHandler(BaseEventData eventData)
    {
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

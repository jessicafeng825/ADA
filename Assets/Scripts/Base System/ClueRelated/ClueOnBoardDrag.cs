using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

public class ClueOnBoardDrag : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IPointerUpHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerDownHandler
{
    private string clueID;

    [SerializeField]
    private Canvas canvas;
    private RectTransform rectTransform;

    [SerializeField]
    private RectTransform movableArea;
    private float movableAreaMaxHeight, movableAreaMinHeight, movableAreaMaxWidth, movableAreaMinWidth;
    private Vector2 newPosition;

    // Mouse over to open clue detail
    [SerializeField]
    private bool mouseOver, isDragging;
    private bool isDrag = false;
    [SerializeField]
    private float pressedTimeRequired;
    [SerializeField]
    private float timer;
    [SerializeField]
    private GameObject progressBar;
    private Image fillImage;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        movableArea = transform.parent.GetComponent<RectTransform>();
        movableAreaMaxHeight = (movableArea.rect.height + movableArea.offsetMin.y) * movableArea.transform.lossyScale.y;
        movableAreaMinHeight = movableArea.offsetMin.y * movableArea.transform.lossyScale.y;
        movableAreaMaxWidth = movableArea.rect.width * movableArea.transform.lossyScale.x;
        movableAreaMinWidth = 0;
        timer = 0;
        progressBar.SetActive(false);
        fillImage = progressBar.GetComponentsInChildren<Image>()[1];
    }

    private void Update()
    {
        if (mouseOver && !isDragging)
        {
            timer += Time.deltaTime; 
            progressBar.SetActive(true);
            fillImage.fillAmount = timer / pressedTimeRequired;
        }
        else
        {
            timer = 0;
            fillImage.fillAmount = 0;
            progressBar.SetActive(false);
        }

        if(timer > pressedTimeRequired)
        {
            ResetLoading();
            DetectiveBoardManager.Instance.OpenClueInfoOnBoard(clueID, this.GetComponent<RectTransform>().localPosition);
        }
    }

    private void ResetLoading()
    {
        timer = 0;
        fillImage.fillAmount = 0;
        progressBar.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        mouseOver = true;
        // high light ui
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouseOver = false;
        // cancel highlight
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isDragging)
        {
            DetectiveBoardManager.Instance.ClueSelected(clueID);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (isDragging)
        {
            isDragging = false;
        }

        if (isDrag == false)
        {
            Debug.Log("Click");

            DetectiveBoardManager.Instance.addOnlyTwoPoints(this.gameObject);
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("OnPointerDown");
    }
    #region Dragging Related Functions
    // Function to move clue on board
    public void OnBeginDrag(PointerEventData eventData)
    {
        isDragging = true;
        isDrag = true;
        Debug.Log("OnbeginDrag");
    }
    //use this draging function instead
    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        Debug.Log("OnDrag");
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
        isDrag = false;
        Debug.Log("OnbeginDrag");
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

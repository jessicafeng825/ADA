using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class ClueOnBoardDrag : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private string clueID;

    [SerializeField]
    private Canvas canvas;
    private Vector2 newPosition;
    private float canvasWidth, canvasHeight;

    // Mouse over to open clue detail
    private bool mouseOver;
    [SerializeField]
    private float waitForSeconds;
    [SerializeField]
    private float timer = 0;

    private void Start()
    {
        canvasWidth = canvas.GetComponent<RectTransform>().rect.width;
        canvasHeight = canvas.GetComponent<RectTransform>().rect.height;
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
            DetectiveBoardManager.Instance.OpenClueInfoOnBoard(clueID);
            Debug.Log("Open");
            timer = 0;
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
        if (position.y < canvasHeight - 250 && position.y > 300 &&
            position.x < canvasWidth && position.x > 0)
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

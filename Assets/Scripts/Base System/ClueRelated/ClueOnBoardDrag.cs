using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class ClueOnBoardDrag : MonoBehaviour
{
    private string clueID;

    [SerializeField]
    private Canvas canvas;
    private Vector2 newPosition;
    private float canvasWidth, canvasHeight;

    private bool mouseOver;

    private void Start()
    {
        canvasWidth = canvas.GetComponent<RectTransform>().rect.width;
        canvasHeight = canvas.GetComponent<RectTransform>().rect.height;
    }

    private void Update()
    {
        // if (mouseOver)
        // {
        //     Debug.Log("mouseOver");
        // }
        // else
        // {
        //     Debug.Log("Not over");
        // }
    }

    private void OnMouseOver()
    {
        mouseOver = true;
    }

    private void OnMouseExit()
    {
        mouseOver = false;
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
        if (position.y < canvasHeight*2 - 250 && position.y > 300 &&
            position.x < canvasWidth*2 && position.x > 0)
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

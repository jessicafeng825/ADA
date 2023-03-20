using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class ClueOnBoardDrag : MonoBehaviour
{
    [SerializeField]
    private Canvas canvas;
    private Vector2 newPosition;

    private float canvasWidth, canvasHeight;

    private void Start()
    {
        canvasWidth = canvas.GetComponent<RectTransform>().rect.width;
        canvasHeight = canvas.GetComponent<RectTransform>().rect.height;
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
        if (position.y < canvasHeight && position.y > 0 &&
            position.x < canvasWidth && position.x > 0)
        {
            return true;
        }
        return false;
    }
}

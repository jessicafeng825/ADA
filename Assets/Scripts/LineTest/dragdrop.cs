using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class dragdrop : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    // Start is called before the first frame update
    [SerializeField ]private Canvas canvas;
    private RectTransform rectTransform;
    public bool isDrag = false;
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("onDrag");
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("onEndDrag");
        isDrag = false;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        //Debug.Log("OnPointerDown");
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if(isDrag == false)
        {
            Debug.Log("Click");

            lineTest.Instance.addOnlyTwoPoints(this.gameObject);
        }
        
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("onBeginDrag");
        isDrag = true;
    }
}

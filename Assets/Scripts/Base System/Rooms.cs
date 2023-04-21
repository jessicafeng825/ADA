using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Rooms : MonoBehaviour
{    
    //These are the properties of the rooms

    [field: SerializeField]
    public float roomScale
    { get; private set; } = 1;

    [field: SerializeField]
    public bool firstRoominMemory 
    { get; set; }

    [field: SerializeField]
    public bool midRoom 
    { get; private set; }


    [field: SerializeField]
    public bool isHidden 
    { get; set; }


    [field: SerializeField]
    public Memory locatedMemory 
    { get; private set;}

    [field: SerializeField]
    public string roomName 
    { get; private set;}

    [field: SerializeField]
    public int interestPointCount 
    { get; set;}





    void Awake()
    {
        
        
    }
    private void Start() {
        roomName = this.gameObject.name;
        if(midRoom)
            return;
        foreach(Transform child in this.transform.GetChild(1))
        {
            interestPointCount++;
        }
        if(!firstRoominMemory)
        {   
            this.GetComponent<CanvasGroup>().alpha = 0;
            this.GetComponent<CanvasGroup>().interactable = false;
            this.GetComponent<CanvasGroup>().blocksRaycasts = false;
            this.gameObject.SetActive(false);
        }
    }
}

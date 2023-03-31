using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public Memory locatedMemory 
    { get; private set;}

    [field: SerializeField]
    public string roomName 
    { get; private set;}

    public int interestPointCount 
    { get; private set;}

    public bool allInterestPointsCollected 
    { get; private set; }



    void Awake()
    {
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Photon.Pun;

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
        roomName = this.gameObject.name;
        if(midRoom)
            return;
        foreach(Transform child in this.transform.GetChild(1))
        {
            interestPointCount++;
        }
        
    }
    private void Start() {
        
        if(midRoom)
            return;
        if(!firstRoominMemory)
        {   
            this.GetComponent<CanvasGroup>().alpha = 0;
            this.GetComponent<CanvasGroup>().interactable = false;
            this.GetComponent<CanvasGroup>().blocksRaycasts = false;
            if(PhotonNetwork.IsMasterClient)
            {
                
                return;
            }
        
            this.gameObject.SetActive(false);
        }
    }
    public void UpdateIPCount()
    {
        interestPointCount--;
        if(!PhotonNetwork.IsMasterClient)
            return;
        InvestigationManager.Instance.SyncPCMapIPCount(locatedMemory, roomName, interestPointCount);       
    }
}

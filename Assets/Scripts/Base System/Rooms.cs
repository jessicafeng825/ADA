using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rooms : MonoBehaviour
{    
    //These are the properties of the rooms
    [field: SerializeField]
    public bool firstRoominMemory 
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
        foreach(Transform child in this.transform.GetChild(1))
        {
            interestPointCount++;
        }
        Debug.Log(roomName + ": " + interestPointCount + " interest points");
        if(!firstRoominMemory)
        {            
            this.gameObject.SetActive(false);
        }
    }
}

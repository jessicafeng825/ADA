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

    public int teleportPointCount 
    { get; private set;}

    public int interestPointCount 
    { get; private set;}

    public int doorCount 
    { get; private set;}


    void Awake()
    {
        foreach(Transform child in this.transform.GetChild(0))
        {
            teleportPointCount++;
        }
        foreach(Transform child in this.transform.GetChild(1))
        {
            interestPointCount++;
        }
        foreach(Transform child in this.transform.GetChild(2))
        {
            doorCount++;
        }
        Debug.Log(roomName + ":\r\n " + "Teleport points: " + teleportPointCount + "   Interest points: " + interestPointCount + "   Doors: " + doorCount);
        if(!firstRoominMemory)
        {            
            this.gameObject.SetActive(false);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rooms : MonoBehaviour
{    
    //These are the properties of the rooms
    [field: SerializeField]
    public Memory locatedMemory 
    { get; private set;}

    [field: SerializeField]
    public string roomName 
    { get; private set;}

    [field: SerializeField]
    public float zoom
    { get; private set;}
}

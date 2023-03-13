using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rooms : MonoBehaviour
{

    [field: SerializeField]
    public string roomName 
    { get; private set;}

    [field: SerializeField]
    public float zoom
    { get; private set;}
}

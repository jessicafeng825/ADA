using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInfo : MonoBehaviour
{
    [field: SerializeField]
    public Rooms targetRoom
    { get; private set; }
}

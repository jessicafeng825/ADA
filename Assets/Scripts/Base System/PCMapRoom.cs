using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCMapRoom : MonoBehaviour
{
    [field: SerializeField]
    public Memory memory
    {get; private set;}
    
    [field: SerializeField]
    public string roomName
    {get; private set;}

    [field: SerializeField]
    public int PlayerCount
    {get; set;}

    private void Start()
    {
        roomName = this.gameObject.name;
    }

}

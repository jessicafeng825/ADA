using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class PCMapRoom : MonoBehaviour
{
    [field: SerializeField]
    public Memory memory
    {get; private set;}
    
    [field: SerializeField]
    public string roomName
    {get; private set;}

    [field: SerializeField]
    public bool secretRoom
    {get; set;}

    [field: SerializeField]
    public int PlayerCount
    {get; set;}

    private void Start()
    {
        roomName = this.gameObject.name;
        if(memory == InvestigationManager.Instance.startMemory.GetComponent<MemoryInfo>().memory)
        {
            if(roomName == InvestigationManager.Instance.startRoom.roomName)
            {
                PlayerCount = PhotonNetwork.CurrentRoom.PlayerCount - 1;
                this.transform.Find("Number").gameObject.SetActive(true);
                this.transform.Find("Number").GetChild(0).GetComponent<TextMeshProUGUI>().text = PlayerCount.ToString();
            }
        }
        if(secretRoom)
        {
            this.GetComponent<CanvasGroup>().alpha = 0;
        }
    }

}

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
    public Rooms correspondingRoom
    {get; private set;}

    [field: SerializeField]
    public bool secretRoom
    {get; set;}

    [field: SerializeField]
    public int PlayerCount
    {get; set;}

    [field: SerializeField]
    public int InterestPoinCount
    {get; set;}

    private void Awake() {
        roomName = this.gameObject.name;
        InterestPoinCount = correspondingRoom.interestPointCount;
        transform.Find("Number").GetChild(0).GetComponent<TextMeshProUGUI>().text = InterestPoinCount.ToString();
        if(InterestPoinCount == 0)
        {
            transform.Find("Number").gameObject.SetActive(false);
        }
    }
    private void Start()
    {
        // if(memory == InvestigationManager.Instance.startMemory.GetComponent<MemoryInfo>().memory)
        // {
        //     if(roomName == InvestigationManager.Instance.startRoom.roomName)
        //     {
        //         PlayerCount = PhotonNetwork.CurrentRoom.PlayerCount - 1;
        //         this.transform.Find("Number").gameObject.SetActive(true);
        //         this.transform.Find("Number").GetChild(0).GetComponent<TextMeshProUGUI>().text = PlayerCount.ToString();
        //     }
        // }
        if(secretRoom)
        {
            this.GetComponent<CanvasGroup>().alpha = 0;
        }
    }
    public void UpdateIPCount(int count)
    {
        InterestPoinCount  = count;        
        transform.Find("Number").GetChild(0).GetComponent<TextMeshProUGUI>().text = InterestPoinCount.ToString();
        if(InterestPoinCount == 0)
        {
            transform.Find("Number").gameObject.SetActive(false);
        }
    }

}

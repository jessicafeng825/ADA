using System.Collections;
using System.Collections.Generic;
using TMPro;
using Photon.Pun;
using UnityEngine;

public class DetectiveBoardManager : Singleton<DetectiveBoardManager>
{
    private PhotonView pv;
    [SerializeField]
    private Canvas mainCanvas;
    [SerializeField]
    private GameObject detectiveBoard;
    [SerializeField]
    private GameObject clueOnBoardTemplate;
    private GameObject tempClue;

    private void Start()
    {
        pv = GetComponent<PhotonView>();
    }

    public void ShareClue(string clueID)
    {
        pv.RPC(nameof(SynchronizeShareClue), RpcTarget.MasterClient, clueID);
    }

    [PunRPC]
    private void SynchronizeShareClue(string clueID)
    {
        Debug.Log("share");
        tempClue = Instantiate(clueOnBoardTemplate);
        tempClue.GetComponentInChildren<TMP_Text>().text = clueID;
        tempClue.GetComponent<ClueOnBoardDrag>().SetCanvas(mainCanvas);
        tempClue.GetComponent<Transform>().SetParent(detectiveBoard.GetComponent<Transform>(), false);
    }

    public void ActivateDetectiveBoard()
    {
        detectiveBoard.SetActive(true);
    }

    public void DeactivateDetectiveBoard()
    {
        detectiveBoard.SetActive(false);
    }
}

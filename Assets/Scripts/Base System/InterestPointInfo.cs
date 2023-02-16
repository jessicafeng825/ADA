using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine;

public class InterestPointInfo : MonoBehaviour
{
    [SerializeField]
    private List<string> clueList;
    private int cnt_currentClue = 0;
    public PhotonView pv;

    private void Start()
    {
        
        pv = GetComponent<PhotonView>();
        this.GetComponent<Button>().onClick.AddListener(AddClue);
    }

    
    private void AddClue()
    {
        if (cnt_currentClue < clueList.Count)
        {
            InvestigationManager.Instance.AddCluePrefab(clueList[cnt_currentClue]);
            pv.RPC(nameof(clueStatusUpdate), RpcTarget.All);
        }
        else
            Debug.Log("there's no clues");
    }

    [PunRPC] private void clueStatusUpdate()
    {
        cnt_currentClue++;
    }
}

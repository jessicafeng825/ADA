using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine;

public class InterestPointInfo : MonoBehaviour
{
    [SerializeField]
    private List<InterestPoint> collectableList;
    private int cnt_current = 0;
    public PhotonView pv;

    [System.Serializable]
    public class InterestPoint
    {
        public string name;
        public bool isClue;
    }

    private void Start()
    {
        pv = GetComponent<PhotonView>();
        GetComponent<Button>().onClick.AddListener(AddCollectable);
    }

    private void AddCollectable()
    {
        if (cnt_current < collectableList.Count)
        {
            if (collectableList[cnt_current].isClue)
                InvestigationManager.Instance.AddCluePrefab(collectableList[cnt_current].name);
            else
                InvestigationManager.Instance.AddPuzzlePrefab(collectableList[cnt_current].name);

            pv.RPC(nameof(clueStatusUpdate), RpcTarget.All);
        }
        else
        {
            Debug.Log("No other collectable");
            // show alert message
        }
    }

    [PunRPC] private void clueStatusUpdate()
    {
        cnt_current++;
    }
}

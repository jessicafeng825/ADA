using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using TMPro;
using Photon.Realtime;
using System.Linq;
using UnityEngine.UI;

public class playerController : MonoBehaviour
{
    public static playerController Instance;

    public PlayerManagerForAll.gamestage stageNow = PlayerManagerForAll.gamestage.Intro;//playerstage now
    [SerializeField] public float playerSpeed = 2.0f;//player icon
    [SerializeField] public string playerJob = "None";//player job

    public int maxAP;
    public int currentAP;
    private PhotonView pv;

    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        pv = GetComponent<PhotonView>();
        DontDestroyOnLoad(this);

        maxAP = InvestigationManager.Instance.GetPlayerInitialAP();
        currentAP = maxAP;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    #region Change Phase
    public void introtoInvest()
    {
        stageNow = PlayerManagerForAll.gamestage.Investigate;
    }
    public void investtoDicuss()
    {
        stageNow = PlayerManagerForAll.gamestage.Dissussion;
    }
    public void jobSelect(string job)
    {
        playerJob = job;
    }

    public void discusstoInvest()
    {
        
    }
    public void ChangeStage(PlayerManagerForAll.gamestage stage)
    {
        stageNow = stage;
        pv.RPC("SynchronizeStageNow", RpcTarget.All, stageNow);
    }
    #endregion

    [PunRPC]
    public void SynchronizeStageNow(PlayerManagerForAll.gamestage stage)
    {
        stageNow = stage;
        //jump to discussion stage
    }

    public void Cost_currentAP(int costAP)
    {
        currentAP -= costAP;
    }
}

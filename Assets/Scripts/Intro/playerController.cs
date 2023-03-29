using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using TMPro;
using Photon.Realtime;
using System.Linq;
using UnityEngine.UI;


public class playerController : MonoBehaviour /*, IPunObservable*/
{
    public static playerController Instance;

    public PlayerManagerForAll.gamestage stageNow = PlayerManagerForAll.gamestage.Intro;//playerstage now
    [SerializeField] public float playerSpeed = 2.0f;//player icon
    [SerializeField] public string playerJob = "None";//player job
    [SerializeField] public string playerName = "None";//player job
    [SerializeField] public string playerBackground = "None";//player job
    [SerializeField] public string skillText = "None";//player skillText
    [SerializeField] public string relationshipText = "None";//player relationshiptext

    [SerializeField] public Sprite playerImage;//player Image
    [SerializeField] public bool isselected = false;
   public int maxAP;
    public int currentAP;
    public Rooms currentRoom;
    public Transform currentMemory;
    private PhotonView pv;

    // Share Clue Part
    public int currentClueSharedNum = 0;

    private void Awake()
    {
        //Only the local player will be the instance instead of the last player entering the scene
        if(this.GetComponent<PhotonView>().IsMine)
            Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        pv = GetComponent<PhotonView>();
        DontDestroyOnLoad(this);

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    #region Change Phase
    public void introtoInvest()
    {
        stageNow = PlayerManagerForAll.gamestage.Investigate;
        Debug.Log("Investigate");
    }
    public void investtoDicuss()
    {
        stageNow = PlayerManagerForAll.gamestage.Discussion;
    }
    public void jobSelect(string job,string playername,string playerbackground, string playerskill, string playerrelationship, string playerimage)
    {
        //if(isselected == false)
        //{
            playerJob = job;
            playerName = playername;
            playerBackground = playerbackground;
            skillText = playerskill;
            relationshipText = playerrelationship;
            playerImage = Resources.Load<Sprite>("CharacterUI/Characters/" + "Round_" + playerimage);
            isselected = true;
       // }
       
    }

    public void discusstoInvest()
    {
        
    }
    public void ChangeStage(PlayerManagerForAll.gamestage stage)
    {
        pv.RPC("SynchronizeStageNow", RpcTarget.All, stage);
    }
    public void ChangeAllStage(PlayerManagerForAll.gamestage stage)
    {
        pv.RPC("SyncAllStageNow", RpcTarget.All, stage);
    }
    [PunRPC]
    public void SynchronizeStageNow(PlayerManagerForAll.gamestage stage)
    {
        stageNow = stage;
    }
    [PunRPC]
    public void SyncAllStageNow(PlayerManagerForAll.gamestage stage)
    {
        Instance.stageNow = stage;
    }
    #endregion

    

    public void Change_currentAP(int costAP)
    {
        currentAP += costAP;
        BaseUIManager.Instance.UpdateAPUI(currentAP);
        pv.RPC(nameof(SynchronizeCurrentAP), RpcTarget.All, currentAP);
    }
    public void Change_maxAP(int maxAP)
    {
        pv.RPC(nameof(SynchronizeMaxAP), RpcTarget.All, maxAP);
    }
    [PunRPC]
    public void SynchronizeCurrentAP(int currentAP)
    {
        this.currentAP = currentAP;
    }
    [PunRPC]
    public void SynchronizeMaxAP(int maxAP)
    {
        this.maxAP = maxAP;
    }
    //This commented part is a way of synchronizing the stageNow variable across all players, but I have decided to use RPC instead

    // public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    // {
    //     Debug.Log("OnPhotonSerializeView");
    //     if (stream.IsWriting)
    //     {
    //         Debug.Log("IsWriting");
    //         stream.SendNext(stageNow);
    //     }
    //     else
    //     {
    //         Debug.Log("IsRecieving");
    //         stageNow = (PlayerManagerForAll.gamestage)stream.ReceiveNext();
    //     }
    // }

}

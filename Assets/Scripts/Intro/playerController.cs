using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using TMPro;
using Photon.Realtime;
using System.Linq;
using UnityEngine.UI;


public class playerController : MonoBehaviourPunCallbacks /*, IPunObservable*/
{
    public static playerController Instance;

    public gamestage stageNow = gamestage.Intro;//playerstage now
    [SerializeField] public float playerSpeed = 2.0f;//player icon
    [SerializeField] public string playerJob = "None";//player job
    [SerializeField] public string playerName = "None";//player job
    [SerializeField] public string playerBackground = "None";//player job
    [SerializeField] public string skillText = "None";//player skillText
    [SerializeField] public string alibiText = "None";
    [SerializeField] public string secretText = "None";
    [SerializeField] public Sprite playerUpperImage;//player Image
    [SerializeField] public Sprite playerRoundImage;//player Image
    [SerializeField] public bool isselected = false;

    [SerializeField] public bool voteForAccused = false;

    public string accusedPlayer;
    public int maxAP;
    public int currentAP;
    public Rooms currentRoom;
    public string currentRoomName;
    public Transform currentMemory;
    public string currentMemoryName;
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
        if(PhotonNetwork.IsMasterClient && pv.IsMine)
        {
            playerJob = "Host";
            pv.RPC(nameof(SetMasterAsHostJob), RpcTarget.All);
        }
        DontDestroyOnLoad(this);
        accusedPlayer = "None";

    }
    [PunRPC]
    public void SetMasterAsHostJob()
    {
        playerJob = "Host";
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        pv.RPC(nameof(JoinAfterJobSelectRPC), RpcTarget.All, playerJob, playerName, playerBackground, skillText, alibiText, secretText, playerJob);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    #region Change Phase
    public void introtoInvest()
    {
        stageNow = gamestage.Investigate;
    }
    public void investtoDicuss()
    {
        stageNow = gamestage.Discussion;
    }
    public void jobSelect(string job,string playername,string playerbackground, string playerskill, string playerAlibi, string playerSecret, string playerimage)
    {
        //if(isselected == false)
        //{
            playerJob = job;
            playerName = playername;
            playerBackground = playerbackground;
            skillText = playerskill;
            alibiText = playerAlibi;
            secretText = playerSecret;
            if (!PhotonNetwork.IsMasterClient)
            {
                playerUpperImage = UIManager.Instance.GetPlayerSprite("Upper_" + playerimage);
                playerRoundImage = UIManager.Instance.GetPlayerSprite("Round_" + playerimage);
            }
            isselected = true;
       // }
       
    }
    public void JoinAfterJobSelect(JOb job)
    {
        pv.RPC(nameof(JoinAfterJobSelectRPC), RpcTarget.All, job.jobName, job.playername, job.backgroundstory, job.skilltext, job.alibitext, job.secret, job.playerImage);
    }

    [PunRPC]
    public void JoinAfterJobSelectRPC(string job,string playername,string playerbackground, string playerskill, string playerAlibi, string playerSecret, string playerimage)
    {
        jobSelect(job,playername,playerbackground,playerskill,playerAlibi,playerSecret,playerimage);
    }
    public void discusstoInvest()
    {
        
    }
    public void ChangeStage(gamestage stage)
    {
        pv.RPC("SynchronizeStageNow", RpcTarget.All, stage);
    }
    public void ChangeAllStage(gamestage stage)
    {
        pv.RPC("SyncAllStageNow", RpcTarget.All, stage);
    }
    [PunRPC]
    public void SynchronizeStageNow(gamestage stage)
    {
        stageNow = stage;
    }
    [PunRPC]
    public void SyncAllStageNow(gamestage stage)
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

    public void AccusePlayer(string ac)
    {
        accusedPlayer = ac;
        pv.RPC(nameof(AccusePlayerRPC), RpcTarget.All, ac);
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
    [PunRPC]
    public void AccusePlayerRPC(string ac)
    {
        accusedPlayer = ac;
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

    public bool IsMasterClient()
    {
        return PhotonNetwork.IsMasterClient;
    }
    public void SyncRoomMemory(string roomName, string memoryName)
    {
        pv.RPC(nameof(SyncRoomMemoryRPC), RpcTarget.All, roomName, memoryName);
    }

    [PunRPC]
    public void SyncRoomMemoryRPC(string roomName, string memoryName)
    {
        currentRoomName = roomName;
        currentMemoryName = memoryName;
    }

    public void SyncVoteForAccusationPhase()
    {
        voteForAccused = !voteForAccused;
        pv.RPC(nameof(VoteForAccusationPhaseRPC), RpcTarget.All);
    }

    [PunRPC]
    public void VoteForAccusationPhaseRPC()
    {
        voteForAccused = !voteForAccused;
    }
}

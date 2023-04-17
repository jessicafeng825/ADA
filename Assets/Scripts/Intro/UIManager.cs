using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using TMPro;
using Photon.Realtime;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.Video;
public class UIManager : MonoBehaviourPunCallbacks
{
    //====Panel Objects====//
    public static UIManager Instance;
    [SerializeField] private VideoPlay videoPlay;
    public GameObject pcPanel;
    public GameObject playerPanel;
    public Menu[] pcPanelList;
    public Menu[] playerPanelList;
    public JOb[] jobList;
    public PhotonView pv;
    //=====Launcher Objects=====//
    public string url;
    [SerializeField] public TMP_Text playerName;//player Name
    [SerializeField] public TMP_Text playerJob;//player Job
    [SerializeField] public TMP_Text playerbk;//player background
    [SerializeField] public TMP_Text playersk;//player skills
    [SerializeField] public TMP_Text playerAlibi;//player alibi
    [SerializeField] public TMP_Text playerSecret;//player secret
    [SerializeField] public Image playerImageUI;//player Image
    [SerializeField] public VideoPlayer video;
    [SerializeField] private GameObject characterBrief;
    [SerializeField] private GameObject PCBrief;
    [SerializeField] private GameObject otherCharacterInfo;
    private bool ifintroend = false;
    public bool ifMemVideoStart = false;
    private bool ifMemVideoEnd = false;
    private bool ifselected = false;
    private GameObject[] listofgameObjectwithtag;
    private GameObject gameObjectNow;
    public bool superAP;

    private void Awake()
    {
        ExitGames.Client.Photon.Hashtable setScene = new ExitGames.Client.Photon.Hashtable();
        setScene.Add("gameRunning", false);
        PhotonNetwork.CurrentRoom.SetCustomProperties(setScene);
        pv = GetComponent<PhotonView>();
        //video.prepareCompleted += OnPrepareVideo;
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = false;
        if(PhotonNetwork.CurrentRoom.CustomProperties["gameRunning"].Equals(true))
        { 
            PhotonNetwork.IsMessageQueueRunning = false;
            UIManager.Instance.CloseMenu("BgLoad");
            UIManager.Instance.OpenMenu("JoinAfter");
        }
        //DontDestroyOnLoad(this);
        //===set up pc and player depend on whether it is master or not===//
        pcPanel.SetActive(PhotonNetwork.IsMasterClient);
        playerPanel.SetActive(!PhotonNetwork.IsMasterClient);
        //video.url = url;
        listofgameObjectwithtag = GameObject.FindGameObjectsWithTag("Player");

        playerName.text = PhotonNetwork.NickName;

    }
    // Update is called once per frame
    void Update()
    {
        
        introToSelect();
        MemTechVideoEndPlay();
        
    }
    
    public override void OnPlayerLeftRoom(Player leftPlayer)
    {
        if(!PhotonNetwork.IsMasterClient)
        {
            return;
        }
        
        pv.RPC(nameof(AllJobButtonActivation), RpcTarget.AllBufferedViaServer);
        
        if (checkIfAllhaveSelectJob() && !ifMemVideoStart)
        {           
            pv.RPC(nameof(MasterJobSelectToBackroundIntro), RpcTarget.MasterClient);
        }
        Debug.Log("player left"); // seen when other disconnects
    }
    [PunRPC]
    private void AllJobButtonActivation()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        
        for (int i = 0; i < jobList.Length; i++)
        {
            foreach(GameObject player in players)
            {
                if (jobList[i].jobName == player.GetComponent<playerController>().playerJob)
                {
                    jobList[i].select();
                    break;             
                }
                else
                {
                    jobList[i].unSelect();//set this to be active                                      
                }
            }
            

        }
    }

    void OnPrepareVideo(VideoPlayer vp)
    {
        video.Play();
    }
    //====Panel Functions====//
    public void OpenMenu(string menuName)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            for (int i = 0; i < pcPanelList.Length; i++)
            {
                if (pcPanelList[i].menuName == menuName)
                {
                    pcPanelList[i].Open();
                    
                }
                
            }
        }
        else
        {
            for (int i = 0; i < playerPanelList.Length; i++)
            {
                if (playerPanelList[i].menuName == menuName)
                {
                    playerPanelList[i].Open();
                    
                    
                }
                
            }
        }
        
    }

    public void CloseMenu(string menuName)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            for (int i = 0; i < pcPanelList.Length; i++)
            {
                if (pcPanelList[i].menuName == menuName)
                {
                    pcPanelList[i].Close();
                }
                
            }
        }
        else
        {
            for (int i = 0; i < playerPanelList.Length; i++)
            {
                if (playerPanelList[i].menuName == menuName)
                {
                    playerPanelList[i].Close();
                }
                
            }
        }

    }
    public void OpenMenu(Menu menu)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            // First, close the currently open menu
            for (int i = 0; i < pcPanelList.Length; i++)
            {
                if (pcPanelList[i].open)
                {
                    //CloseMenu(pcPanelList[i]);
                }
            }
            menu.Open();
        }
        else
        {
            // First, close the currently open menu
            for (int i = 0; i < playerPanelList.Length; i++)
            {
                if (playerPanelList[i].open)
                {
                    //CloseMenu(playerPanelList[i]);
                }
            }
            menu.Open();
        }
    }

    public void CloseMenu(Menu menu)
    {
        menu.Close();
    }
    //===Check Overall Functions===//
    private bool checkIfAllhaveSelectJob()
    {
        // if(isSinglePlayer)
        // {
        //     return true;
        // }
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach(GameObject player in players)
        {
            if(player.GetComponent<playerController>().playerJob == "None" && player.GetComponent<playerController>() != playerController.Instance)
            {
                return false;
            }
        }
        // for (int i = 0; i < jobList.Length; i++) 
        // {
        //     if(jobList[i].isselected == false)//have not yet select a job
        //     {
        //         Debug.Log(jobList[i].jobName + " have not yet selected");
        //         return false;
        //     }

        // }
        return true;
    }

    //===Laucher Functions===//
    public void onclick()//click test
    {
        playerName.text = "onclick";
    }
    public void menuclicktest()//open Menu test
    {
        UIManager.Instance.OpenMenu("TestMenu");
    }

    public void OpenPCCharacterBrief(JOb job)
    {
        PCBrief.SetActive(true);
        PCBrief.transform.Find("Visuals").Find("TitleText").GetComponent<TMP_Text>().text = job.jobName;
        PCBrief.transform.Find("Visuals").Find("NameText").GetComponent<TMP_Text>().text = job.playername;
        PCBrief.transform.Find("BriefText").GetComponent<TMP_Text>().text = job.brief;
    }

    public void OpenCharacterBrief(JOb job)
    {
        UIManager.Instance.CloseMenu("CharacterSelect");
        UIManager.Instance.OpenMenu("CharacterBrief");
        characterBrief.transform.Find("Title").Find("Text (TMP)").GetComponent<TMP_Text>().text = job.jobName;
        characterBrief.transform.Find("Brief").Find("Text (TMP)").GetComponent<TMP_Text>().text = job.brief;
        characterBrief.transform.Find("Brief").Find("Title").Find("Text (TMP)").GetComponent<TMP_Text>().text = job.playername;
        characterBrief.transform.Find("CharacterImage").GetComponent<Image>().sprite = Resources.Load<Sprite>("CharacterUI/Characters/" + job.playerImage);
        characterBrief.transform.Find("YesButton").GetComponent<Button>().onClick.AddListener(() => { jobSelect(job); });
    }
    public void CloseCharacterBrief()
    {
        characterBrief.transform.Find("YesButton").GetComponent<Button>().onClick.RemoveAllListeners();
        UIManager.Instance.CloseMenu("CharacterBrief");
        UIManager.Instance.OpenMenu("CharacterSelect");
        
        characterBrief.transform.Find("ReturnButton").gameObject.SetActive(false);
        characterBrief.transform.Find("ReturnPanel").gameObject.SetActive(false);
    }

    public void OpenCharacterInfo(CharacterInfoSimple characterInfo)
    {
        otherCharacterInfo.SetActive(true);
        otherCharacterInfo.transform.Find("Title").GetComponentInChildren<TMP_Text>().text = characterInfo.jobName;
        otherCharacterInfo.transform.Find("Brief").GetComponentsInChildren<TMP_Text>()[0].text = characterInfo.characterName;
        otherCharacterInfo.transform.Find("Brief").GetComponentsInChildren<TMP_Text>()[1].text = characterInfo.brief;
        otherCharacterInfo.transform.Find("CharacterImage").GetComponent<Image>().sprite = Resources.Load<Sprite>("CharacterUI/Characters/" + characterInfo.playerImage);
    }

    public void jobSelect(JOb job)
    {
        //listofgameObjectwithtag = GameObject.FindGameObjectsWithTag("Player");
        /*
        for(int i = 0;i < listofgameObjectwithtag.Length; i++)
        {
            if (listofgameObjectwithtag[i].GetComponent<PhotonView>().IsMine)
            {
                //pv.RPC(nameof(updateallplayerName), RpcTarget.All, listofgameObjectwithtag[i],name);
            }
        }
        */
        foreach(JOb j in jobList)
        {
            if(j.name == job.name)
            {
                if(j.isselected)
                {
                    characterBrief.transform.Find("ReturnButton").gameObject.SetActive(true);
                    characterBrief.transform.Find("ReturnPanel").gameObject.SetActive(true);
                    return;
                }
                else
                {
                    j.select();
                }
            }
            else
            {
                continue;
            }
        }
        pv.RPC(nameof(updateallplayerName), RpcTarget.AllBufferedViaServer, playerController.Instance.GetComponent<PhotonView>().ViewID ,job.jobName, job.playername,job.backgroundstory, job.skilltext, job.alibitext, job.secret, job.playerImage);
        //playerController.Instance.jobSelect(name);

        Debug.Log(playerController.Instance.playerJob);
        // if(ifselected == false)
        // {
            playerJob.text = job.jobName;
            playerName.text = job.playername;
            playerbk.text = job.backgroundstory;
            playersk.text = job.skilltext;
            playerAlibi.text = job.alibitext;
            playerSecret.text = job.secret;
            playerImageUI.sprite = Resources.Load<Sprite>("CharacterUI/Characters/" + "Round_" + job.playerImage);
            //ifselected = true;
        // }
        
        characterBrief.transform.Find("Brief").gameObject.SetActive(false);
        characterBrief.transform.Find("YesButton").gameObject.SetActive(false);
        characterBrief.transform.Find("NoButton").gameObject.SetActive(false);
        characterBrief.transform.Find("CharacterImage").GetComponent<RectTransform>().localPosition = new Vector3(0, characterBrief.transform.Find("CharacterImage").GetComponent<RectTransform>().localPosition.y, 0);
    
       
        //make all the player button inactive
        //pv.RPC(nameof(setbuttonInactive), RpcTarget.All, job.jobName); 
        

        if (checkIfAllhaveSelectJob())//if all the player have select job
        {
            Debug.Log("all player have select job");
           
            pv.RPC(nameof(MasterJobSelectToBackroundIntro), RpcTarget.MasterClient);
        }
        

    }
    public void jobselectForname(string oldJob, string newJob)
    {
        for (int i = 0; i < jobList.Length; i++)
        {
            if (jobList[i].jobName == newJob)
            {
                if (!jobList[i].isselected)
                {
                    jobList[i].select();//set this to be inactive
                }
                
            }
            else if (jobList[i].jobName == oldJob)
            {
                if (jobList[i].isselected)                
                {
                    jobList[i].unSelect();//set this to be active
                }
                
            }

        }
    }
    [PunRPC]
    public void MasterJobSelectToBackroundIntro()
    {
        pv.RPC(nameof(jobSelecttobackgroundintro), RpcTarget.AllViaServer);
    }
    [PunRPC]
    private void jobSelecttobackgroundintro()
    {
        if(ifMemVideoStart)
        {
            return;
        }
        Debug.Log("jobSelecttobackgroundintro");
        UIManager.Instance.OpenMenu("Info");
        UIManager.Instance.OpenMenu("InfoPC");
        UIManager.Instance.CloseMenu("CharacterBrief");
        UIManager.Instance.CloseMenu("CharacterSelect");
        UIManager.Instance.CloseMenu("CharacterSelectPC");

    }
    [PunRPC]
    private void setbuttonActivation(string oldJob, string newJob)
    {
        UIManager.Instance.jobselectForname(oldJob, newJob);
    }
    [PunRPC]
    private void updateallplayerName(int id,string jobname,string playername,string playerbackground,string skillText, string alibiText, string secretText, string playerImage)
    {
        //gb.GetComponent<playerController>().jobSelect(name);
        listofgameObjectwithtag = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < listofgameObjectwithtag.Length; i++)
        {
            if (listofgameObjectwithtag[i].GetComponent<PhotonView>().ViewID == id)
            {
                // if(listofgameObjectwithtag[i].GetComponent<playerController>().isselected == false)
                // {
                    string oldJob = listofgameObjectwithtag[i].GetComponent<playerController>().playerJob;
                    listofgameObjectwithtag[i].GetComponent<playerController>().jobSelect(jobname, playername, playerbackground, skillText, alibiText, secretText, playerImage);

                    //gameObjectNow = listofgameObjectwithtag[i];

                    // 
                    pv.RPC(nameof(AllJobButtonActivation), RpcTarget.AllBufferedViaServer);
                // }
                
            }
        }
    }
    //===movescene====//
    public void changenextSceneTest()
    {
        pv.RPC(nameof(changenextScene), RpcTarget.AllViaServer);
    }

    [PunRPC]
    private void changenextScene()
    {
        //Synchronize stage between players
        playerController.Instance.ChangeStage(gamestage.Investigate);
        
        if(PhotonNetwork.IsMasterClient && this.GetComponent<PhotonView>().IsMine)
        {
            playerController.Instance.AccusePlayer("Host");
        }
        PhotonNetwork.LoadLevel(2);
        
    }
    public void ChangeNextSceneAfter()
    {
        playerController.Instance.ChangeStage(gamestage.Investigate);
        PhotonNetwork.LoadLevel(2);
    }

    //====Panel stage change====//
    private void introToSelect()
    {
        if(video.isPaused && !ifintroend)
        {
            
            ifintroend = true;
            pv.RPC(nameof(closeintro), RpcTarget.AllBufferedViaServer);
           
        }
        
    }

    public void MemTechVideoStartPlay()
    {
        UIManager.Instance.CloseMenu("InfoPC");
        UIManager.Instance.OpenMenu("IntroBackground");
        ifMemVideoStart = true;
        pv.RPC(nameof(PlayerWaitMemVideo), RpcTarget.AllViaServer);
        videoPlay.PlayMemVid();
    }

    private void MemTechVideoEndPlay()
    {
        if(video.isPaused && ifMemVideoStart && !ifMemVideoEnd)
        {
            ifMemVideoEnd = true;
            pv.RPC(nameof(changenextScene), RpcTarget.AllBufferedViaServer);
        }
    }
    [PunRPC]
    private void PlayerWaitMemVideo()
    {
        ifMemVideoStart = true;
        UIManager.Instance.OpenMenu("BgLoad");
        UIManager.Instance.CloseMenu("Info");
    }

    [PunRPC]
    private void closeintro()
    {
        StartCoroutine(KeepLoading(1f));
        UIManager.Instance.CloseMenu("IntroBackground");
        UIManager.Instance.OpenMenu("CharacterSelectPC");

    }

    //Coroutine for loading screen for work around of the video error
    IEnumerator KeepLoading(float sec)
    {
        //UIManager.Instance.OpenMenu("BgLoad");
        yield return new WaitForSeconds(sec);
        UIManager.Instance.CloseMenu("BgLoad");
        UIManager.Instance.OpenMenu("CharacterSelect");
    }

    public void SuperAPMode(GameObject check)
    {
        superAP = !superAP;
        check.gameObject.SetActive(superAP);
    }


}

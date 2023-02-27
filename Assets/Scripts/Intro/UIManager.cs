using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using TMPro;
using Photon.Realtime;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.Video;
public class UIManager : MonoBehaviour
{
    //====Panel Objects====//
    public static UIManager Instance;
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
    [SerializeField] public TMP_Text playerrl;//player relationship
    [SerializeField] public Image playerImage;//player Image
    [SerializeField] public VideoPlayer video;
    public bool ifintroend = false;
    private GameObject[] listofgameObjectwithtag;
    private void Awake()
    {
        pv = GetComponent<PhotonView>();
        video.prepareCompleted += OnPrepareVideo;
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
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
                    if (pcPanelList[i].open)
                    {
                        pcPanelList[i].Close();
                    }
                    else
                    {
                        pcPanelList[i].Open();
                    }
                }
                
            }
        }
        else
        {
            for (int i = 0; i < playerPanelList.Length; i++)
            {
                if (playerPanelList[i].menuName == menuName)
                {
                    if (playerPanelList[i].open)
                    {
                        playerPanelList[i].Close();
                    }
                    else
                    {
                        playerPanelList[i].Open();
                    }
                    
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
        
        for (int i = 0; i < jobList.Length; i++) 
        {
            if(jobList[i].isselected == false)//have not yet select a job
            {
                return false;
            }

        }
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
        pv.RPC(nameof(updateallplayerName), RpcTarget.All, PhotonNetwork.NickName,job.jobName, job.playername,job.backgroundstory, job.playerImage);
        //playerController.Instance.jobSelect(name);
        Debug.Log(playerController.Instance.playerJob);
        playerJob.text = job.jobName;
        playerName.text = job.playername;
        playerbk.text = job.backgroundstory;
        playerrl.text = job.relationshiptext;
        playersk.text = job.skilltext;
        playerImage.sprite = Resources.Load<Sprite>("CharacterUI/" + job.playerImage);
        //make all the player button inactive
        pv.RPC(nameof(setbuttonInactive), RpcTarget.All, job.jobName);
        if (checkIfAllhaveSelectJob())//if all the player have select job
        {
           
            pv.RPC(nameof(jobSelecttobackgroundintro), RpcTarget.All);
        }
        

    }
    public void jobselectForname(string name)
    {
        for (int i = 0; i < jobList.Length; i++)
        {
            if (jobList[i].jobName == name)
            {
                if (!jobList[i].isselected)
                {
                    jobList[i].select();//set this to be inactive
                }
                
            }

        }
    }
    [PunRPC]
    private void jobSelecttobackgroundintro()
    {
        UIManager.Instance.OpenMenu("Info");
        UIManager.Instance.OpenMenu("InfoPC");
        UIManager.Instance.CloseMenu("CharacterSelect");
        UIManager.Instance.CloseMenu("CharacterSelectPC");

    }
    [PunRPC]
    private void setbuttonInactive(string name)
    {
        UIManager.Instance.jobselectForname(name);
    }
    [PunRPC]
    private void updateallplayerName(string name,string jobname,string playername,string playerbackground, string playerImage)
    {
        //gb.GetComponent<playerController>().jobSelect(name);
        listofgameObjectwithtag = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < listofgameObjectwithtag.Length; i++)
        {
            if (listofgameObjectwithtag[i].GetComponent<PhotonView>().Owner.NickName == name)
            {
                listofgameObjectwithtag[i].GetComponent<playerController>().jobSelect(jobname,playername,playerbackground,playerImage);
           }
        }
    }
    //===movescene====//
    public void changenextSceneTest()
    {

        pv.RPC(nameof(changenextScene), RpcTarget.All);
    }

    [PunRPC]
    private void changenextScene()
    {
        UIManager.Instance.CloseMenu("InfoPC");
        //Synchronize stage between players
        playerController.Instance.ChangeStage(PlayerManagerForAll.gamestage.Investigate);
        PhotonNetwork.LoadLevel(2);
        
    }

    //====Panel stage change====//
    private void introToSelect()
    {
        if(video.isPaused && !ifintroend)
        {
            
            ifintroend = true;
            pv.RPC(nameof(closeintro), RpcTarget.All);
           
        }
        
    }

    [PunRPC]
    private void closeintro()
    {
        UIManager.Instance.CloseMenu("PlayerBackground");
        UIManager.Instance.CloseMenu("PcBackground");
        UIManager.Instance.OpenMenu("CharacterSelect");
        UIManager.Instance.OpenMenu("CharacterSelectPC");

    }


}

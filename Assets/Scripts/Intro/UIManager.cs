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
    public PhotonView pv;
    //=====Launcher Objects=====//
    public string url;
    [SerializeField] public TMP_Text playerName;//player job
    [SerializeField] public TMP_Text playerJob;//player job
    [SerializeField] public VideoPlayer video;
    public bool ifintroend = false;
    private void Awake()
    {
        pv = GetComponent<PhotonView>();
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
        //===set up pc and player depend on whether it is master or not===//
        pcPanel.SetActive(PhotonNetwork.IsMasterClient);
        playerPanel.SetActive(!PhotonNetwork.IsMasterClient);
        video.url = url;
        video.Play();



        playerName.text = PhotonNetwork.NickName;
    }

    // Update is called once per frame
    void Update()
    {
        introToSelect();
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


    //===Laucher Functions===//
    public void onclick()//click test
    {
        playerName.text = "onclick";
    }
    public void menuclicktest()//open Menu test
    {
        UIManager.Instance.OpenMenu("TestMenu");
    }

    public void jobSelect(string name)
    {
        playerController.Instance.jobSelect(name);
        playerJob.text = name;
        UIManager.Instance.OpenMenu("Info");
        UIManager.Instance.CloseMenu("CharacterSelect");

    }
    //===movescene====//
    public void changenextSceneTest()
    {

        pv.RPC(nameof(changenextScene), RpcTarget.All);
    }

    [PunRPC]
    private void changenextScene()
    {
        PhotonNetwork.LoadLevel(2);
        playerController.Instance.introtoInvest();
        
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
        
    }


}

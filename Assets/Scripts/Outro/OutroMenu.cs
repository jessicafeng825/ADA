using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.Video;
using UnityEngine.UI;
using TMPro;

public class OutroMenu : MonoBehaviour
{
    [SerializeField]
    private Menu[] pcPanelList;

    [SerializeField]
    private Menu[] playerPanelList;

    [SerializeField]
    private GameObject PCMenu;

    [SerializeField]
    private GameObject playerMenu;

    [SerializeField] 
    private VideoPlayer video;

    private bool outroVidEnded;

    [SerializeField]
    private GameObject outroSlide;

    [SerializeField]
    private GameObject finalPanel;
    
    private Animator outroSlideAnimator;

    private GameObject yesButton;

    private GameObject noButton;

    private int characterCount;

    private int correctCount;

    private GameObject secretCountText;

    private GameObject characterImage;

    private GameObject characterSecret;
    void Awake()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            PCMenu.SetActive(true);
            playerMenu.SetActive(false);
            OpenMenu("OutroVideo");
        }
        else
        {
            PCMenu.SetActive(false);
            playerMenu.SetActive(true);
            OpenMenu("loading");
        }
    }
    private void Start() 
    {
        outroSlideAnimator = outroSlide.GetComponent<Animator>();
        yesButton = outroSlide.transform.Find("Secrets").Find("YesButton").gameObject;
        noButton = outroSlide.transform.Find("Secrets").Find("NoButton").gameObject;
        characterImage = outroSlide.transform.Find("Secrets").Find("ProfileImage").Find("CharacterImage").gameObject;
        characterSecret = outroSlide.transform.Find("Secrets").Find("Description").Find("Text (TMP)").gameObject;
        secretCountText = finalPanel.transform.Find("SecretCount").Find("Text (TMP)").gameObject;
    }

    private void OpenMenu(string menuName)
    {
        // if(PhotonNetwork.IsMasterClient)
        // {
            foreach(Menu menu in pcPanelList)
            {
                if(menu.name == menuName)
                {
                    menu.Open();
                }
                else
                {
                    menu.Close();
                }
            }
        //}
        // else
        // {
            foreach(Menu menu in playerPanelList)
            {
                if(menu.name == menuName)
                {
                    menu.Open();
                }
                else
                {
                    menu.Close();
                }
            }
        //}
        
    }

    private void CloseMenu(string menuName)
    {
        if(PhotonNetwork.IsMasterClient)
        {
            foreach(Menu menu in pcPanelList)
            {
                if(menu.name == menuName)
                {
                    menu.Close();
                }
            }
        }
        else
        {
            foreach(Menu menu in playerPanelList)
            {
                if(menu.name == menuName)
                {
                    menu.Close();
                }
            }
        }
    }

    private void Update() 
    {
        OnVideoEnd();
    }

    public void OnVideoEnd()
    {
        if(video.isPlaying == false && outroVidEnded == false)
        {
            outroVidEnded = true;
            OpenMenu("OutroSlide");
            LoadNextSlide();
        }
    }

    public void LoadNextSlide()
    {
        switch(characterCount)
        {
            case 0:
                characterImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("CharacterUI/Characters/Round_Lawyer");
                characterSecret.GetComponent<TextMeshProUGUI>().text = "Bishop Kaine had an affair with his wife Leila, which caused their marriage";
                break;
            case 1:
                characterImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("CharacterUI/Characters/Round_Street Kid");
                characterSecret.GetComponent<TextMeshProUGUI>().text = "Mutts has broken into the mansion before";
                break;
            case 2:
                characterImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("CharacterUI/Characters/Round_Nightclub Owner");
                characterSecret.GetComponent<TextMeshProUGUI>().text = "Izzy blacked mailed Bishop Kaine and a lot of other people";
                break;
            case 3:
                characterImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("CharacterUI/Characters/Round_Brawler");
                characterSecret.GetComponent<TextMeshProUGUI>().text = "Kian's sister Evie was murdered by the MemTech Industries";
                break;
            case 4:
                characterImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("CharacterUI/Characters/Round_Biohacker");
                characterSecret.GetComponent<TextMeshProUGUI>().text = "The Void was planning on bombing the announcement of the new MemTech product";
                break;
            case 5:
                characterImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("CharacterUI/Characters/Round_Security Guard");
                characterSecret.GetComponent<TextMeshProUGUI>().text = "Lucius contacted the Board before his murder, who then command him to kill Ava";
                break;
        }
        characterCount ++;
    }

    public void OnYesButton()
    {
        correctCount ++;
        if(correctCount < 6)
        {            
            outroSlideAnimator.SetTrigger("ShowSlide");
            LoadNextSlide();
        }
        else
        {
            OpenMenu("Final");
            secretCountText.GetComponent<TextMeshProUGUI>().text = correctCount.ToString();
        }
            
    }

    public void OnNoButton()
    {
        if(correctCount < 6)
        {
            outroSlideAnimator.SetTrigger("ShowSlide");
            LoadNextSlide();
        }
        else
        {
            OpenMenu("Final");
            secretCountText.GetComponent<TextMeshProUGUI>().text = correctCount.ToString();
        }
        
        
    }
}

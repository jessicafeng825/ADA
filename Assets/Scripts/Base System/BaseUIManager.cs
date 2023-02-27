using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;
using Photon.Pun;

public class BaseUIManager : Singleton<BaseUIManager>
{
    [SerializeField]
    private GameObject pcPanel, playerPanel;
    // use this for a while, till Hui finish the MenuManager part
    [SerializeField]
    private GameObject clueInfoNoPicPanel, clueInfoPicPanel;

    [SerializeField]
    private TMP_Text clueNameText, clueNamePicText;
        
    [SerializeField]
    private TMP_Text clueDescripText, clueDescripPicText;

    [SerializeField]
    private Image cluePicHolder;
    

    [SerializeField]
    private GameObject puzzleInfoMenu;
    private GameObject tempPuzzle;
    private Dictionary<string, GameObject> inScenePuzzles = new Dictionary<string, GameObject>();    
    // function: load all clue picture resources and add to dictionary for use

    public void Start()
    {
        pcPanel.SetActive(PhotonNetwork.IsMasterClient);
        playerPanel.SetActive(!PhotonNetwork.IsMasterClient);
    }
    #region Clue UI Related Functions
    // function: show clue information panel with no picture 
    public void ShowClueInfoNoPicture(string clueName, string clueDescrip)
    {
        clueNameText.text = clueName;
        clueDescripText.text = clueDescrip;
        clueInfoNoPicPanel.SetActive(true);
        StartCoroutine(showshowway(clueInfoNoPicPanel));
    }

    // function: show clue information panel with picture 
    public void ShowClueInfoWithPicture(string clueName, string clueDescrip)
    {
        clueNamePicText.text = clueName;
        clueDescripPicText.text = clueDescrip;
        cluePicHolder.sprite = ResourceManager.Instance.GetCluePic(clueName);
        StartCoroutine(showshowway(clueInfoPicPanel));
    }

    // function: hide clue information panel with no picture 
    public void HideCLueInfoNoPicture()
    {
        clueInfoNoPicPanel.SetActive(false);
    }

    // function: hide clue information panel with picture 
    public void HideClueInfoWithPicture()
    {
        clueInfoPicPanel.SetActive(false);
    }
    #endregion

    public void ShowPuzzleUI(string puzzleName)
    {
        if (inScenePuzzles.ContainsKey(puzzleName))
        {
            inScenePuzzles[puzzleName].SetActive(true);
        }
        else
        {
            tempPuzzle = Instantiate(ResourceManager.Instance.GetPuzzleInteraction(puzzleName));
            tempPuzzle.GetComponent<Transform>().SetParent(puzzleInfoMenu.GetComponent<Transform>(), false);
            inScenePuzzles.Add(puzzleName, tempPuzzle);
        }
        
        puzzleInfoMenu.SetActive(true);
    }

    public void HidePuzzleUI()
    {
        puzzleInfoMenu.SetActive(false);
    }

    //Spawn notification panel
    public void SpawnNotificationPanel(string title, string discription, int btnNum, float time)
    {
        
        GameObject tempNotification = Instantiate(ResourceManager.Instance.GetUIElement("NotificationPanel"));
        if(PhotonNetwork.IsMasterClient)
            tempNotification.transform.SetParent(pcPanel.transform);
        else
            tempNotification.transform.SetParent(playerPanel.transform);
        tempNotification.transform.localScale = new Vector3(1, 1, 1);
        tempNotification.transform.localPosition = new Vector3(0, 0, 0);
        tempNotification.transform.GetChild(1).GetChild(1).GetComponent<TMP_Text>().text = title;
        tempNotification.transform.GetChild(2).GetChild(1).GetComponent<TMP_Text>().text = discription;
        tempNotification.GetComponent<NotificationScript>().despawnTime = time;
        switch(btnNum)
        {
            case 2:
                tempNotification.transform.GetChild(3).gameObject.SetActive(true);
                tempNotification.transform.GetChild(4).gameObject.SetActive(false);
                break;
            case 1:
                tempNotification.transform.GetChild(3).gameObject.SetActive(false);
                tempNotification.transform.GetChild(4).gameObject.SetActive(true);
                break;
            case 0:
                tempNotification.transform.GetChild(3).gameObject.SetActive(false);
                tempNotification.transform.GetChild(4).gameObject.SetActive(false);
                break;
        }
    }

    // Just for temporary use to solve the UI bug
    IEnumerator showshowway(GameObject panel)
    {
        panel.SetActive(true);
        yield return new WaitForSeconds(0.05f);
        panel.SetActive(true);
    }
}

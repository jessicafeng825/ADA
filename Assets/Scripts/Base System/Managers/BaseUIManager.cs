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
    [SerializeField]
    private GameObject charaterPanel;
    // use this for a while, till Hui finish the MenuManager part
/*    [SerializeField]
    private GameObject clueInfoNoPicPanel, clueInfoPicPanel;

    [SerializeField]
    private TMP_Text clueNameText, clueNamePicText;
        
    [SerializeField]
    private TMP_Text clueDescripText, clueDescripPicText;

    [SerializeField]
    private Image cluePicHolder;*/

    [SerializeField]
    private GameObject clueInfoMenu;
    private GameObject tempClue;
    private Dictionary<string, GameObject> inSceneClues = new Dictionary<string, GameObject>();

    [SerializeField]
    private GameObject puzzleInfoMenu;
    private GameObject tempPuzzle;
    private Dictionary<string, GameObject> inScenePuzzles = new Dictionary<string, GameObject>();

    [SerializeField]
    private GameObject MemoryOverview;

    public void Start()
    {
        pcPanel.SetActive(PhotonNetwork.IsMasterClient);
        playerPanel.SetActive(!PhotonNetwork.IsMasterClient);
        InitializeCharacterUI();
    }
    #region Clue UI Related Functions
    public void ShowClueUI(string clueID)
    {
        if (inSceneClues.ContainsKey(clueID))
        {
            inSceneClues[clueID].SetActive(true);
        }
        else
        {
            tempClue = Instantiate(ResourceManager.Instance.GetClueInfo(clueID));
            tempClue.GetComponent<Transform>().SetParent(clueInfoMenu.GetComponent<Transform>(), false);
            inSceneClues.Add(clueID, tempPuzzle);
        }

        clueInfoMenu.SetActive(true);
    }

    public void HideClueUI()
    {
        clueInfoMenu.SetActive(false);
    }
    #endregion

    #region Puzzle UI Related Functions
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
    #endregion


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
    public void InitializeCharacterUI()
    {
        playerPanel.transform.Find("MainMenu").Find("InvestigationPanel").Find("CharacterButton").GetComponent<Button>().image.sprite = playerController.Instance.playerImage;
        playerPanel.transform.Find("MainMenu").Find("DiscussionPanel").Find("CharacterButton").GetComponent<Button>().image.sprite = playerController.Instance.playerImage;
        charaterPanel.transform.Find("CharacterButton").GetComponent<Button>().image.sprite = playerController.Instance.playerImage;
        charaterPanel.transform.Find("Description").GetChild(1).GetComponent<TMP_Text>().text = playerController.Instance.playerBackground;
        charaterPanel.transform.Find("Relationship").GetChild(1).GetComponent<TMP_Text>().text = "Relationship";
        charaterPanel.transform.Find("Skill").GetChild(1).GetComponent<TMP_Text>().text = "Skill";
    }
    public void UpdateAPUI(int num)
    {
        playerPanel.transform.Find("MainMenu").Find("InvestigationPanel").Find("APpoints").GetChild(1).GetComponent<TMP_Text>().text = num.ToString() + "AP";
    }

    public void ClickMemoryOverview()
    {
        if (MemoryOverview.activeSelf)
        {
            MemoryOverview.SetActive(false);
        }
        else MemoryOverview.SetActive(true);
    }

    //Zoom out to view the full map of the current lovated memory
    public void ClickMapOverview()
    {
        foreach (Transform room in playerController.Instance.currentMemory.transform)
        {
            if(room.GetComponent<Rooms>().midRoom)
                continue;
            OpenRoom(room.GetComponent<Rooms>());
        }
        Vector3 midPos = -playerController.Instance.currentMemory.GetChild(0).transform.localPosition;
        Vector3 midScale = Vector3.one * playerController.Instance.currentMemory.GetChild(0).transform.GetComponent<Rooms>().roomScale;
        playerController.Instance.currentMemory.localPosition = midPos;
        playerController.Instance.currentMemory.localScale = midScale;
    }
    public void CloseMapOverview()
    {
        foreach (Transform room in playerController.Instance.currentMemory.transform)
        {
            if(room.GetComponent<Rooms>().midRoom)
                continue;

            CloseRoom(room.GetComponent<Rooms>());
            if(room.GetComponent<Rooms>().roomName == playerController.Instance.currentRoom.roomName)
            {
                room.gameObject.SetActive(true);
                room.GetComponent<CanvasGroup>().alpha = 1;
                room.GetComponent<CanvasGroup>().blocksRaycasts = true;
                room.GetComponent<CanvasGroup>().interactable = true;
                room.transform.localScale = Vector3.one * playerController.Instance.currentRoom.roomScale;
                continue;
            }
        }
        Vector3 currentRoomPos = -playerController.Instance.currentRoom.transform.localPosition;
        playerController.Instance.currentMemory.localPosition = currentRoomPos;
        playerController.Instance.currentMemory.localScale = Vector3.one;
    }
    private void OpenRoom(Rooms room)
    {
        room.gameObject.SetActive(true);
        room.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        room.gameObject.transform.GetChild(2).gameObject.SetActive(false);
        room.GetComponent<CanvasGroup>().alpha = 1;
        room.GetComponent<CanvasGroup>().blocksRaycasts = false;
        room.GetComponent<CanvasGroup>().interactable = false;
        room.transform.localScale = Vector3.one;
    }
    private void CloseRoom(Rooms room)
    {
        room.gameObject.transform.GetChild(0).gameObject.SetActive(true);
        room.gameObject.transform.GetChild(2).gameObject.SetActive(true);
        room.gameObject.SetActive(false);
        room.GetComponent<CanvasGroup>().alpha = 0;
        room.GetComponent<CanvasGroup>().blocksRaycasts = false;
        room.GetComponent<CanvasGroup>().interactable = false;
        room.transform.localScale = Vector3.one;
    }


    // Just for temporary use to solve the UI bug
    IEnumerator showshowway(GameObject panel)
    {
        panel.SetActive(true);
        yield return new WaitForSeconds(0.05f);
        panel.SetActive(true);
    }
}

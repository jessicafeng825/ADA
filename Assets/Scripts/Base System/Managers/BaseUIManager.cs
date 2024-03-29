using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;
using Photon.Pun;
using System.Linq;

public class BaseUIManager : Singleton<BaseUIManager>
{
    [SerializeField]
    private GameObject pcPanel, playerPanel;
    [SerializeField]
    private GameObject charaterPanel;

    [SerializeField]
    private GameObject playerTimerPanel;

    [SerializeField]
    private GameObject joinAfterSelectPanel;

    [SerializeField]
    private GameObject transitionPanel;

    [SerializeField]
    private GameObject clueInfoMenu;
    private GameObject tempClue;
    private Dictionary<string, GameObject> inSceneClueInfos = new Dictionary<string, GameObject>();
    public Dictionary<string, GameObject> inBaseClueBtns = new Dictionary<string, GameObject>();


    [SerializeField]
    private GameObject puzzleInfoMenu;
    private GameObject tempPuzzle;
    private Dictionary<string, GameObject> inScenePuzzles = new Dictionary<string, GameObject>();
    public Dictionary<string, GameObject> inBasePuzzleBtns = new Dictionary<string, GameObject>();

    [SerializeField]
    private GameObject MemoryOverview;

    [SerializeField]
    private GameObject thisRoundCollectPanel;

    [SerializeField]
    private TextMeshProUGUI APText;

    [SerializeField]
    private GameObject closeMapOverviewButton;

    // UI Effects for new clues and puzzles
    [SerializeField]
    private TMP_Text openClueMenuNewText, openPuzzleMenuNewText, roomText;

    [SerializeField]
    private GameObject otherCharacterInfo;

    public void Start()
    {
        pcPanel.SetActive(PhotonNetwork.IsMasterClient);
        //playerPanel.SetActive(!PhotonNetwork.IsMasterClient);
        if(PhotonNetwork.CurrentRoom.CustomProperties["gameRunning"].Equals(true) || playerController.Instance.playerJob == "None")
        {            
            transitionPanel.SetActive(false);
            PhotonNetwork.IsMessageQueueRunning = true;
            joinAfterSelectPanel.SetActive(true);
        }
        else
        {
            joinAfterSelectPanel.SetActive(false);
        }

        InitializeCharacterUI();

        roomText.text = PhotonNetwork.CurrentRoom.Name;
    }

    #region Clue UI Related Functions
    public void ShowClueUI(string clueID)
    {
        if (inSceneClueInfos.ContainsKey(clueID))
        {
            inSceneClueInfos[clueID].SetActive(true);
        }
        else
        {
            tempClue = Instantiate(ResourceManager.Instance.GetClueInfo(clueID));
            tempClue.GetComponent<Transform>().SetParent(clueInfoMenu.GetComponent<Transform>(), false);
            inSceneClueInfos.Add(clueID, tempClue);
        }
        playerTimerPanel.SetActive(false);
        clueInfoMenu.SetActive(true);
    }

    public void HideClueUI()
    {
        playerTimerPanel.SetActive(true);
        clueInfoMenu.SetActive(false);
    }

    public void AddClueBtn(string clueID, GameObject clueContent)
    {
        inBaseClueBtns.Add(clueID, clueContent);
    }

    public void SetClueShared(string clueID)
    {
        inBaseClueBtns[clueID].GetComponent<ClueBtn>().SetSharedMark();
    }

    public void BaseNewClueEffectsCheck()
    {
        openClueMenuNewText.gameObject.SetActive(false);
        //openClueMenuNewText.GetComponent<TMP_Text>().alpha = 1;

        foreach (GameObject clueBtn in inBaseClueBtns.Values.ToList())
        {
            if (!clueBtn.GetComponent<ClueBtn>().isViewed)
            {
                openClueMenuNewText.gameObject.SetActive(true);
            }
        }
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
        
        playerTimerPanel.SetActive(false);
        puzzleInfoMenu.SetActive(true);
    }

    public void HidePuzzleUI()
    {
        playerTimerPanel.SetActive(true);
        puzzleInfoMenu.SetActive(false);
    }

    public void AddPuzzleBtns(string puzzleID, GameObject puzzleContent)
    {
        inBasePuzzleBtns.Add(puzzleID, puzzleContent);
    }

    public void RemovePuzzleBtns(string puzzleID)
    {
        if(inBasePuzzleBtns.ContainsKey(puzzleID))
        {
            Destroy(inBasePuzzleBtns[puzzleID]);            
            inBasePuzzleBtns.Remove(puzzleID);
        }
        if(inScenePuzzles.ContainsKey(puzzleID))
        {
            Destroy(inScenePuzzles[puzzleID]);
            inScenePuzzles.Remove(puzzleID);
        }
            
    }

    public bool CheckHavePuzzle(string puzzleID)
    {
        if (inBasePuzzleBtns.ContainsKey(puzzleID))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool CheckDecoderUnlocked(string puzzleID)
    {
        if(!inScenePuzzles.ContainsKey(puzzleID))
        {
            return false;
        }
        else if (inScenePuzzles[puzzleID].GetComponent<NumpadPuzzle>().decoderActivated)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void BaseNewPuzzleEffectsCheck()
    {
        openPuzzleMenuNewText.gameObject.SetActive(false);
        //openPuzzleMenuNewText.GetComponent<TMP_Text>().alpha = 1;

        if (inBasePuzzleBtns.Count != 0)
        {
            foreach (GameObject puzzleBtn in inBasePuzzleBtns.Values.ToList())
            {
                if (!puzzleBtn.GetComponent<PuzzleBtn>().isViewed)
                {
                    openPuzzleMenuNewText.gameObject.SetActive(true);
                }
            }
        }
    }
    #endregion

    #region Character Menu Related Functions
    public void OpenCharacterInfo(CharacterInfoSimple characterInfo)
    {
        otherCharacterInfo.SetActive(true);
        otherCharacterInfo.transform.Find("Title").GetComponentInChildren<TMP_Text>().text = characterInfo.jobName;
        otherCharacterInfo.transform.Find("Brief").GetComponentsInChildren<TMP_Text>()[0].text = characterInfo.characterName;
        otherCharacterInfo.transform.Find("Brief").GetComponentsInChildren<TMP_Text>()[1].text = characterInfo.brief;
        otherCharacterInfo.transform.Find("CharacterImage").GetComponent<Image>().sprite = Resources.Load<Sprite>("CharacterUI/Characters/" + characterInfo.playerImage);
    }
       
    #endregion


    #region Pop-up UI Related Functions
    //Spawn Yes/No panel
    public NotificationScript SpawnNotificationPanel(string title, string discription, int btnNum, float time)
    {
        
        GameObject tempNotification = Instantiate(ResourceManager.Instance.GetUIElement("NotificationPanel"));
        if (PhotonNetwork.IsMasterClient)
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
        return tempNotification.GetComponent<NotificationScript>();
    }
    //Spawn interest point choice panel
    public IPCollectPanel SpawnInterestPointPanel(string title, List<bool> collected)
    {
        GameObject tempIPPanel = Instantiate(ResourceManager.Instance.GetUIElement("IPCollectPopupPanel"));

        tempIPPanel.transform.SetParent(playerPanel.transform);
        tempIPPanel.transform.localScale = new Vector3(1, 1, 1);
        tempIPPanel.transform.localPosition = new Vector3(0, 0, 0);
        tempIPPanel.transform.Find("Title").GetChild(1).GetComponent<TMP_Text>().text = title;
        for(int i = 0; i < tempIPPanel.transform.Find("Choices").childCount; i++)
        {
            if(i < collected.Count)
            {
                if(!collected[i])
                {
                    tempIPPanel.transform.Find("Choices").GetChild(i).gameObject.SetActive(true);
                }
                else
                {
                    tempIPPanel.transform.Find("Choices").GetChild(i).gameObject.SetActive(false);
                }
            }
            else
            {
                tempIPPanel.transform.Find("Choices").GetChild(i).gameObject.SetActive(false);
            }
            
        }
        return tempIPPanel.GetComponent<IPCollectPanel>();
    }


    #endregion
    public void InitializeCharacterUI()
    {
        // if(playerController.Instance.playerUpperImage == null || playerController.Instance.playerRoundImage)
        // {            
        //     playerController.Instance.playerUpperImage = Resources.Load<Sprite>("CharacterUI/Characters/" + "Upper_" + playerController.Instance.playerName);
        //     playerController.Instance.playerRoundImage = Resources.Load<Sprite>("CharacterUI/Characters/" + "Round_" + playerController.Instance.playerName);
        // }
        playerPanel.transform.Find("MainMenu").Find("InvestigationPanel").Find("CharacterButton").GetChild(0).GetComponent<Image>().sprite = playerController.Instance.playerUpperImage;
        playerPanel.transform.Find("MainMenu").Find("DiscussionPanel").Find("CharacterButton").GetChild(0).GetComponent<Image>().sprite = playerController.Instance.playerRoundImage;
        
        charaterPanel.transform.Find("BackgroundPanel").GetComponentInChildren<TMP_Text>().text = playerController.Instance.playerBackground;
        charaterPanel.transform.Find("SkillPanel").GetComponentInChildren<TMP_Text>().text = playerController.Instance.skillText;
        charaterPanel.transform.Find("SecretPanel").GetComponentInChildren<TMP_Text>().text = playerController.Instance.secretText;
    }
    public void UpdateAPUI(int num)
    {
        APText.text = num.ToString() + "\r\nAP";
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
        if(playerController.Instance.currentMemory.localPosition == -playerController.Instance.currentMemory.transform.Find("Midpoint").transform.localPosition)
        {
            closeMapOverviewButton.SetActive(false);
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
        else
        {
            closeMapOverviewButton.SetActive(true);
            foreach (Transform room in playerController.Instance.currentMemory.transform)
            {
                if(room.GetComponent<Rooms>().midRoom)
                    continue;
                OpenRoom(room.GetComponent<Rooms>());
            }
            Vector3 midPos = -playerController.Instance.currentMemory.transform.Find("Midpoint").transform.localPosition;
            Vector3 midScale = Vector3.one * playerController.Instance.currentMemory.GetChild(0).transform.GetComponent<Rooms>().roomScale;
            playerController.Instance.currentMemory.localPosition = midPos;
            playerController.Instance.currentMemory.localScale = midScale;
        }
        
    }
    private void OpenRoom(Rooms room)
    {
        if(room.isHidden)
            return;
        room.gameObject.SetActive(true);
        //room.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        room.gameObject.transform.GetChild(2).gameObject.SetActive(false);
        room.GetComponent<CanvasGroup>().alpha = 1;
        room.GetComponent<CanvasGroup>().blocksRaycasts = false;
        room.GetComponent<CanvasGroup>().interactable = false;
        room.transform.localScale = Vector3.one;
    }
    private void CloseRoom(Rooms room)
    {
        //room.gameObject.transform.GetChild(0).gameObject.SetActive(true);
        room.gameObject.transform.GetChild(2).gameObject.SetActive(true);
        room.gameObject.SetActive(false);
        room.GetComponent<CanvasGroup>().alpha = 0;
        room.GetComponent<CanvasGroup>().blocksRaycasts = false;
        room.GetComponent<CanvasGroup>().interactable = false;
        room.transform.localScale = Vector3.one;
    }
    //Update the things collected in this round
    public void CollectedThisRoundUI(ipType type, string title)
    {
        if(!thisRoundCollectPanel.activeSelf)
        {
            thisRoundCollectPanel.SetActive(true);
            thisRoundCollectPanel.transform.GetChild(0).gameObject.SetActive(true);
            thisRoundCollectPanel.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = type.ToString() + ": " + title;
        }
        else
        {
            GameObject temp = thisRoundCollectPanel.transform.GetChild(0).gameObject;
            GameObject newCollected = Instantiate(temp, thisRoundCollectPanel.transform);
            newCollected.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = type.ToString() + ": " + title;
        }
        // for(int i = 0; i < thisRoundCollectPanel.transform.childCount; i++)
        // {
        //     if(thisRoundCollectPanel.transform.GetChild(i).gameObject.activeSelf)
        //     {
        //         continue;
        //     }
        //     else
        //     {
        //         thisRoundCollectPanel.transform.GetChild(i).gameObject.SetActive(true);
        //         thisRoundCollectPanel.transform.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = type.ToString() + ": " + title;
        //         break;
        //     }
        // }
    }
    public void CloseCollectedUI()
    {
        thisRoundCollectPanel.SetActive(false);
        for(int i = 1; i < thisRoundCollectPanel.transform.childCount; i++)
        {
            Destroy(thisRoundCollectPanel.transform.GetChild(i).gameObject);
        }
    }
}

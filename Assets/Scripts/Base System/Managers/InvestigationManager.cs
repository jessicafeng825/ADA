using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;
using UnityEngine;


public class InvestigationManager : Singleton<InvestigationManager>
{
    #region Parameters: Movement

    
    [SerializeField]
    private GameObject PCMap;

    private List<PCMapRoom> PCMapRooms = new List<PCMapRoom>();

    //New map movement
    [SerializeField]
    private Transform startMemory;

    [SerializeField]
    private Rooms startRoom;
    #endregion

    #region Parameters: Clue & Puzzle Base

    // Player Clue Base Part
    [SerializeField]
    private GameObject ClueBase;
    private GameObject tempClue;

    // Player Puzzle Base Part
    [SerializeField]
    private GameObject PuzzleBase;
    private GameObject tempPuzzle;
    private Dictionary<string, GameObject> inBasePuzzleBtns = new Dictionary<string, GameObject>();
    #endregion

    // Synchronize Interest Point Status Part
    [SerializeField]
    private PhotonView pv;
    [SerializeField]
    private List<GameObject> interestPointList;
    private Dictionary<string, GameObject> interestPoints = new Dictionary<string, GameObject>();

    #region Parameters: Teleport & Memory Unlock
    [SerializeField]
    private List<GameObject> MemoryUI_List;
    private Dictionary<string, GameObject> MemoryUI_Dic = new Dictionary<string, GameObject>();

    [SerializeField]
    private List<Button> unlockedMemoryInOverview;
    private Dictionary<string, Button> unlockedMemoryInOverviewDic = new Dictionary<string, Button>();

    [SerializeField]
    private List<Button> teleportBtnList;

    #endregion

    private void Start()
    {
        //The distance every two area

        pv = GetComponent<PhotonView>();
        //PreloadInterestPoints();
        playerController.Instance.maxAP = GetPlayerInitialAP();
        playerController.Instance.currentAP = playerController.Instance.maxAP;
        playerController.Instance.Change_currentAP(0);
        playerController.Instance.Change_maxAP(playerController.Instance.maxAP);

        InitializeMemoryUIDic();
        InitializeMemoryInOverviewDic();

        playerController.Instance.currentRoom = startRoom;
        playerController.Instance.currentMemory = startMemory;

        //Initialize PC Map
        PCMapInitialize();
    }
    private void OnEnable() 
    {
        playerController.Instance.currentAP = playerController.Instance.maxAP;
        playerController.Instance.Change_currentAP(0);
    }

    private void Update()
    {
        if (playerController.Instance.currentAP == 0 && playerController.Instance.stageNow == PlayerManagerForAll.gamestage.Investigate)
        {
            Debug.Log("No AP");
            playerController.Instance.ChangeStage(PlayerManagerForAll.gamestage.Dissussion);
            if(PhotonNetwork.IsMasterClient)
                return;
            BaseUIManager.Instance.SpawnNotificationPanel("0AP Remained", "Waiting for others to finish investigation...", 0, -1f);
            if (CheckAllPlayer())
            {
                Debug.Log("Change Stage to Disscussion");
                pv.RPC(nameof(MasterChangeStage), RpcTarget.MasterClient);
                pv.RPC(nameof(ChangeStageDialog), RpcTarget.All, "0AP Remained", "Investigation has ended!");
            }
        }
    }

    //Move related functions
    #region Movement Related Functions

    public void MoveRoomDialog(Rooms room)
    {
        BaseUIManager.Instance.SpawnNotificationPanel(room.roomName, "Use 1AP to move to " + room.roomName +"?", 2, -1f);
        NotificationScript.yesButtonEvent.AddListener(() => MoveRoom(room));
        //NotificationScript.yesButtonEvent.AddListener(() => playerController.Instance.Change_currentAP(-1));
    }
    private void MoveRoom(Rooms room)
    {
        Rooms oldRoom = playerController.Instance.currentRoom;

        //Update PC Map room player count
        pv.RPC(nameof(PCMapUpdate), RpcTarget.MasterClient, playerController.Instance.currentMemory.transform.name, oldRoom.name, -1);
        pv.RPC(nameof(PCMapUpdate), RpcTarget.MasterClient, playerController.Instance.currentMemory.transform.name, room.name, 1);

        //Update player's current room
        playerController.Instance.currentRoom = room;
        StartCoroutine(RoomCoroutine(oldRoom, room, 0.5f));
    }
    private void ActivateRoom(Rooms room)
    {
        room.gameObject.SetActive(true);
        room.GetComponent<CanvasGroup>().interactable = true;
        room.GetComponent<CanvasGroup>().blocksRaycasts = true;
    }
    private void DeActivateRoom(Rooms room)
    {
        room.gameObject.SetActive(false);
        room.GetComponent<CanvasGroup>().interactable = false;
        room.GetComponent<CanvasGroup>().blocksRaycasts = false;        
    }
    IEnumerator RoomCoroutine(Rooms oldroom, Rooms newroom, float sec)
    {
        ActivateRoom(newroom);
        float time = 0f;
        //Memory map starting position
        Vector2 startPos = playerController.Instance.currentMemory.localPosition;

        //Original old room scale & alpha
        Vector3 oldstartScale = newroom.transform.localScale;
        float oldalpha = 1f;
        
        //Original new room scale & alpha
        Vector3 newstartScale = newroom.transform.localScale;
        float newalpha = 0f;
        while(time <= sec)
        {
            playerController.Instance.currentMemory.localPosition = Vector2.Lerp(startPos, -newroom.transform.localPosition, time/sec);
            oldroom.transform.localScale = Vector3.Lerp(oldstartScale, new Vector3(oldroom.roomScale, oldroom.roomScale, oldroom.roomScale), time/sec);
            oldroom.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(oldalpha, 0f, time/sec);
            newroom.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(newalpha, 1f, time/sec);
            newroom.transform.localScale = Vector3.Lerp(newstartScale, new Vector3(newroom.roomScale, newroom.roomScale, newroom.roomScale), time/sec);
            time +=Time.deltaTime;
            yield return null;
        }
        playerController.Instance.currentMemory.localPosition = -newroom.transform.localPosition;
        oldroom.transform.localScale = new Vector3(oldroom.roomScale, oldroom.roomScale, oldroom.roomScale);
        oldroom.GetComponent<CanvasGroup>().alpha = 0f;
        DeActivateRoom(oldroom);
        newroom.transform.localScale = new Vector3(newroom.roomScale, newroom.roomScale, newroom.roomScale);
        newroom.GetComponent<CanvasGroup>().alpha = 1f;
    }
    [PunRPC]
    public void PCMapUpdate(Memory memory, string room, int num)
    {
        foreach(PCMapRoom r in PCMapRooms)
        {
            if(r.memory == memory)
            {
                if(r.roomName == room)
                {
                    r.PlayerCount += num;
                    if(r.PlayerCount == 0)
                        r.transform.Find("Number").gameObject.SetActive(false);
                    else
                        r.transform.Find("Number").gameObject.SetActive(true);
                        r.transform.Find("Number").GetChild(0).GetComponent<TextMeshProUGUI>().text = r.GetComponent<PCMapRoom>().PlayerCount.ToString();
                    break;                        
                }
            }
        }
    }

    public void PCMapInitialize()
    {
        foreach(Transform mem in PCMap.transform)
        {
            foreach(Transform r in mem)
            {
                PCMapRooms.Add(r.GetComponent<PCMapRoom>());
            }
        }
    }

    #endregion

    #region Clue Related Functions
    // function: when player click on interest point, add a clue to their clue base
    public void AddCluePrefab(string clueID)
    {
        tempClue = Instantiate(ResourceManager.Instance.GetClueBtn(clueID));
        tempClue.GetComponent<Transform>().SetParent(ClueBase.GetComponent<Transform>(), true);
        playerController.Instance.Change_currentAP(-1);
        ResourceManager.Instance.allClueCount --;
        pv.RPC(nameof(SyncClueCount), RpcTarget.All, ResourceManager.Instance.allClueCount);
    }
    [PunRPC]
    public void SyncClueCount(int n)
    {
        ResourceManager.Instance.allClueCount = n;
    }

    #endregion

    #region Puzzle Related Functions
    public void AddPuzzlePrefab(string puzzleName)
    {
        tempPuzzle = Instantiate(ResourceManager.Instance.GetPuzzleBtn(puzzleName));
        tempPuzzle.GetComponent<Transform>().SetParent(PuzzleBase.GetComponent<Transform>(), true);
        inBasePuzzleBtns.Add(puzzleName, tempPuzzle);
        
        playerController.Instance.Change_currentAP(-1);
    }

    // Call when puzzle solved, update sprite
    public void UpdatePuzzleBtnSolved(string puzzleName)
    {
        Debug.Log(puzzleName);
        inBasePuzzleBtns[puzzleName].GetComponent<PuzzleBtn>().ShowSolvedMark();
    }

    #endregion

    #region Unlock Memory & Teleport Related Functions

    private void InitializeMemoryUIDic()
    {
        foreach (GameObject memoryUI in MemoryUI_List)
        {
            MemoryUI_Dic.Add(memoryUI.name, memoryUI);
        }
    }

    private void InitializeMemoryInOverviewDic()
    {
        foreach (Button memoryOverview in unlockedMemoryInOverview)
        {
            unlockedMemoryInOverviewDic.Add(memoryOverview.name, memoryOverview);
        }
    }

    public void UnlockMemoryInOverview(Memory memory)
    {
        pv.RPC(nameof(UnlockMemorySynchronize), RpcTarget.All, memory);
    }

    [PunRPC]
    private void UnlockMemorySynchronize(Memory memory)
    {
        unlockedMemoryInOverviewDic[memory.ToString()].gameObject.SetActive(true);
    }

    public void UnlockTeleport(Memory fromMemory, Memory toMemory)
    {
        pv.RPC(nameof(UnlockRelatedTeleport), RpcTarget.All, fromMemory, toMemory);
    }

    [PunRPC]
    private void UnlockRelatedTeleport(Memory fromMemory, Memory toMemory)
    {
        TeleportInfo info;
        foreach (Button teleportBtn in teleportBtnList)
        {
            info = teleportBtn.GetComponent<TeleportInfo>();
            if ((info.teleportFromMemory == fromMemory && info.teleportToMemory == toMemory) ||
                (info.teleportFromMemory == toMemory && info.teleportToMemory == fromMemory))
            {
                teleportBtn.gameObject.SetActive(true);
            }
        }

    }
/*    [PunRPC]
    private void UnlockTeleportSynchronize(Memory fromMemory, Memory toMemory)
    {
        MemoryUpdate(teleportDic[fromMemory].listContent, teleportDic[toMemory].listContent, fromMemory, toMemory);
    }

    private void MemoryUpdate(List<Button> TeleportFromList, List<Button> TeleportToList, Memory fromMemory, Memory toMemory)
    {
        foreach (Button teleport in TeleportFromList)
        {
            Debug.Log("name: " + teleport.name);
            Debug.Log(teleport.GetComponent<TeleportInfo>().teleportFromMemory);
            Debug.Log("tom " + toMemory);
            if (teleport.GetComponent<TeleportInfo>().teleportFromMemory == toMemory)
            {
                teleport.gameObject.SetActive(true);
            }
        }

        foreach (Button teleport in TeleportToList)
        {
            if (teleport.GetComponent<TeleportInfo>().teleportToMemory == fromMemory)
            {
                teleport.gameObject.SetActive(true);
            }
        }
    }*/
    public void SpawnTelepoetDialog(string title, string content, Memory fromMemory, Memory toMemory)
    {
        BaseUIManager.Instance.SpawnNotificationPanel(title, content, 2, -1f);
        NotificationScript.yesButtonEvent.AddListener(() => TeleportToFrom(fromMemory, toMemory));
        NotificationScript.yesButtonEvent.AddListener(() => playerController.Instance.Change_currentAP(-1));
    }
    public void TeleportToFrom(Memory fromMemory, Memory toMemory)
    {
        MemoryUI_Dic[fromMemory.ToString()].SetActive(false);
        MemoryUI_Dic[toMemory.ToString()].SetActive(true);
        playerController.Instance.currentMemory = MemoryUI_Dic[toMemory.ToString()].GetComponent<Transform>();
        foreach(Transform room in playerController.Instance.currentMemory)
        {
            if(room.gameObject.name == "FirstRoom")
            {
                playerController.Instance.currentRoom = room.GetComponent<Rooms>();
            }
        }
        Component[] tempRooms = playerController.Instance.currentMemory.GetComponentsInChildren<Rooms>();
        foreach(Rooms room in tempRooms)
        {
            if (room.firstRoominMemory)
            {
                Debug.Log("First room: " + room.roomName);
                playerController.Instance.currentRoom = room;
                break;
            }
        }
    }

    #endregion

    #region AP (Action Points) Related Functions
    public int GetPlayerInitialAP()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            return 0;
        }
        else if (playerController.Instance.playerJob == "")
        {
            return 4;
        }
        else
        {
            return 3;
        }
    }

    private bool CheckAllPlayer()
    {
        GameObject[] playerList = GameObject.FindGameObjectsWithTag("Player");

        foreach(var player in playerList)
        {
            if (player.GetComponent<playerController>().stageNow != PlayerManagerForAll.gamestage.Dissussion)
            {
                return false;
            }
        }

        return true;
    }

    #endregion

    #region Interest Points Related Functions
    private void PreloadInterestPoints()
    {
        foreach (GameObject interestPoint in interestPointList)
        {
            interestPoints.Add(interestPoint.name, interestPoint);
        }
    }
    // Add interest point to the dictionary
    public void AddInterestPoint(string name, GameObject interestPoint)
    {
        interestPoints.Add(name, interestPoint);
    }

    public void SynchronizeInterestPoint(string ipName)
    {
        pv.RPC(nameof(UpdateGivenIPCNT), RpcTarget.All, ipName);
    }

    [PunRPC]
    private void UpdateGivenIPCNT(string ipName)
    {
        interestPoints[ipName].GetComponent<InterestPointInfo>().changeIP_Current(1);
    }

    // functions to synchronize whether interest points are active or not
    public void SynchronizeInterestPointStatus(string ipName)
    {
        pv.RPC(nameof(UpdateIPFullyCollected), RpcTarget.All, ipName);
    }

    [PunRPC]
    private void UpdateIPFullyCollected(string ipName)
    {
        interestPoints[ipName].SetActive(false);
        MemoryUI_Dic[Memory.BishopMemory.ToString()].GetComponent<MemoryInfo>().interestPointCount--;
    }

    // Activate the interest point based on name, when some puzzles solved
    public void ActiveInterestPoint(string ipName)
    {

    }
    

    #endregion

    #region Interest Stage Change Related Functions
    [PunRPC]
    public void ChangeStageDialog(string title, string description)
    {
        Debug.Log("Change Stage Dialog");
        BaseUIManager.Instance.SpawnNotificationPanel(title, description, 1, 3f);
    }

    [PunRPC]
    public void MasterChangeStage()
    {
        TimerManager.Instance.SwitchStage("0AP Remained");
    }
    #endregion
}

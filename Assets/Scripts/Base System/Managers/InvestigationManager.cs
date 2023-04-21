using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;
using UnityEngine;


public class InvestigationManager : Singleton<InvestigationManager>
{
    #region Parameters: InterestPoint Scan

    [SerializeField]
    private GameObject mapScanner;
    [SerializeField]
    private float scanDistance = 30;
    
    [SerializeField]
    private GameObject playerMapArea;
    private bool isScanning;

    #endregion
    #region Parameters: Movement
    
    [SerializeField]
    private GameObject PCMap;

    public List<PCMapRoom> PCMapRooms = new List<PCMapRoom>();

    //New map movement
    [SerializeField]
    public Transform startMemory;

    [SerializeField]
    public Rooms startRoom;
    #endregion

    #region Parameters: Clue & Puzzle Base

    // Player Clue Base Part
    [SerializeField]
    private GameObject ClueBases;
    private GameObject tempClue;

    // Player Puzzle Base Part
    [SerializeField]
    private GameObject PuzzleBases;
    private GameObject tempPuzzle;
    private Dictionary<string, GameObject> inBasePuzzleBtns = new Dictionary<string, GameObject>();
    #endregion

    // Synchronize Interest Point Status Part
    [SerializeField]
    private PhotonView pv;
    private Dictionary<string, GameObject> interestPoints = new Dictionary<string, GameObject>();
    [SerializeField]
    private int allInterestPointCount = 0;

    #region Parameters: Teleport & Memory Unlock
    [SerializeField]
    private GameObject memoryTitleText;

    [SerializeField]
    private List<GameObject> MemoryUI_List;
    private Dictionary<string, GameObject> MemoryUI_Dic = new Dictionary<string, GameObject>();

    [SerializeField]
    private List<Button> unlockedMemoryInOverview;
    private Dictionary<string, Button> unlockedMemoryInOverviewDic = new Dictionary<string, Button>();

    [SerializeField]
    private List<Button> teleportBtnList;

    #endregion
    #region Parameters: AP
    [SerializeField]
    private int playerTutorialAP;
    [SerializeField]
    private int playerNormalAP;
    #endregion


    private void Awake() 
    {
        
        ExitGames.Client.Photon.Hashtable setScene = new ExitGames.Client.Photon.Hashtable();
        setScene.Add("gameRunning", true);
        PhotonNetwork.CurrentRoom.SetCustomProperties(setScene);
        playerController.Instance.currentRoom = startRoom;
        playerController.Instance.currentRoomName = startRoom.roomName;
        playerController.Instance.currentMemory = startMemory;
        playerController.Instance.currentMemoryName = startMemory.GetComponent<MemoryInfo>().memory.ToString();
        playerController.Instance.SyncRoomMemory(startRoom.roomName, startMemory.GetComponent<MemoryInfo>().memory.ToString());
    }
    private void Start()
    {
        

        pv = GetComponent<PhotonView>();
        //PreloadInterestPoints();
        playerController.Instance.maxAP = GetPlayerInitialAP();
        playerController.Instance.currentAP = playerController.Instance.maxAP;
        playerController.Instance.Change_currentAP(0);
        playerController.Instance.Change_maxAP(playerController.Instance.maxAP);

        InitializeMemoryUIDic();
        InitializeMemoryInOverviewDic();


        //Initialize PC Map
        PCMapInitialize();

    }
    private void OnEnable() 
    {
        playerController.Instance.maxAP = GetPlayerInitialAP();
        playerController.Instance.currentAP = playerController.Instance.maxAP;
        playerController.Instance.Change_currentAP(0);
    }

    private void Update()
    {
        if(!PhotonNetwork.IsMasterClient)
        {
            //For scanning interest point
            if(Input.touchCount > 0 && !isScanning)
            {
                foreach(Touch touch in Input.touches)
                {
                    if(touch.fingerId == 0)
                    {
                        if(touch.phase == TouchPhase.Began)
                        {
                            StartCoroutine(StartScanWaitSecond(touch.position, 1f));
                        }
                    }
                }
            }
            //For scanning interest point to test on PC
            else if(Input.touchCount == 0 && !isScanning)
            {
                if(Input.GetMouseButtonDown(0))
                {
                    StartCoroutine(StartScanWaitSecond(Input.mousePosition, 1f));
                }
            }
        }
        


        if (playerController.Instance.currentAP == 0 && playerController.Instance.stageNow == gamestage.Investigate)
        {
            playerController.Instance.ChangeStage(gamestage.Discussion);
            if(PhotonNetwork.IsMasterClient)
                return;
            BaseUIManager.Instance.SpawnNotificationPanel("0AP Remained", "Waiting for others to finish investigation...", 1, 2f);
            if (CheckAllPlayer())
            {
                StartCoroutine(WaitSecondForChangeStage(1f));
                //pv.RPC(nameof(ChangeStageDialog), RpcTarget.All, "0AP Remained", "Investigation has ended!");
            }
        }
    }
    IEnumerator StartScanWaitSecond(Vector3 scanPos, float second)
    {
        isScanning = true;
        GameObject scanTemp = Instantiate(mapScanner, scanPos, Quaternion.identity);
        scanTemp.transform.SetParent(playerController.Instance.currentRoom.transform);
        scanTemp.transform.localScale = new Vector3(1, 1, 1);
        foreach(InterestPointInfo IP in playerController.Instance.currentMemory.GetComponent<MemoryInfo>().interestPoints)
        {
            Debug.Log("IP: " + IP.name + " is in room");
            if(IP.locatedRoom == playerController.Instance.currentRoom)
            {
                if(Vector3.Distance(IP.transform.position, scanPos) <= scanDistance)
                {
                    IP.GetComponent<CanvasGroup>().alpha = 1;
                    IP.GetComponent<CanvasGroup>().blocksRaycasts = true;
                    IP.GetComponent<CanvasGroup>().interactable = true;
                    Debug.Log("IP: " + IP.name + " is in range " + "Distance: " + Vector3.Distance(IP.transform.position, scanPos) + " Scan Distance: " + scanDistance);
                }
                else
                {
                    Debug.Log("IP: " + IP.name + " is not in range " + "Distance: " + Vector3.Distance(IP.transform.position, scanPos) + " Scan Distance: " + scanDistance);
                }
            }
        }
        Debug.Log("Touch Position: " + scanPos);
        yield return new WaitForSeconds(second);
        Destroy(scanTemp);
        isScanning = false;
    }
    //Move related functions
    #region Movement Related Functions

    public void MoveRoom(Rooms room)
    {
        
        Rooms oldRoom = playerController.Instance.currentRoom;
        playerController.Instance.SyncRoomMemory(room.name, playerController.Instance.currentMemoryName);
        //Update PC Map room player count
        Memory tempMemory = playerController.Instance.currentMemory.GetComponent<MemoryInfo>().memory;
        // pv.RPC(nameof(PCMapUpdatePlayerCount), RpcTarget.MasterClient, tempMemory, oldRoom.name, tempMemory, room.name);
        pv.RPC(nameof(MasterPCMapPLayerCount), RpcTarget.MasterClient);
        //Update player's current room
        playerController.Instance.currentRoom = room;
        StartCoroutine(RoomCoroutine(oldRoom, room, 0.5f));
    }
    private void ActivateRoom(Rooms room)
    {
        room.gameObject.SetActive(true);
        room.firstRoominMemory = true;
    }
    private void DeActivateRoom(Rooms room)
    {
        room.gameObject.SetActive(false);
        room.firstRoominMemory = false; 
    }
    IEnumerator RoomCoroutine(Rooms oldroom, Rooms newroom, float sec)
    {
        ActivateRoom(newroom);
        oldroom.GetComponent<CanvasGroup>().interactable = false;
        oldroom.GetComponent<CanvasGroup>().blocksRaycasts = false;        
        float time = 0f;
        //Memory map starting position
        Vector2 startPos = playerController.Instance.currentMemory.localPosition;

        //Original old room scale & alpha
        Vector3 oldstartScale = oldroom.transform.localScale;
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
        newroom.GetComponent<CanvasGroup>().interactable = true;
        newroom.GetComponent<CanvasGroup>().blocksRaycasts = true;
        DeActivateRoom(oldroom);
        newroom.transform.localScale = new Vector3(newroom.roomScale, newroom.roomScale, newroom.roomScale);
        newroom.GetComponent<CanvasGroup>().alpha = 1f;
    }
    [PunRPC]
    public void PCMapUpdatePlayerCount(Memory oldMemory, string oldRoom, Memory newMemory, string newRoom)
    {
        foreach(PCMapRoom r in PCMapRooms)
        {
            if(r.memory == oldMemory)
            {
                if(r.roomName == oldRoom)
                {
                    r.PlayerCount --;
                    if(r.PlayerCount == 0)
                        r.transform.Find("Number").gameObject.SetActive(false);
                    
                    r.transform.Find("Number").GetChild(0).GetComponent<TextMeshProUGUI>().text = r.GetComponent<PCMapRoom>().PlayerCount.ToString();
                                         
                }
            }
            if(r.memory == newMemory)
            {
                if(r.roomName == newRoom)
                {
                    r.PlayerCount ++;
                    if(r.PlayerCount > 0)
                        r.transform.Find("Number").gameObject.SetActive(true);

                    r.transform.Find("Number").GetChild(0).GetComponent<TextMeshProUGUI>().text = r.GetComponent<PCMapRoom>().PlayerCount.ToString();
                                        
                }
            }
        }
    }
    [PunRPC]
    public void MasterPCMapPLayerCount()
    {
        playerController[] players = new playerController[6];
        players = GameObject.FindObjectsOfType<playerController>();
        foreach(PCMapRoom r in PCMapRooms)
        {
            r.PlayerCount = 0;
            foreach(playerController p in players)
            {
                if(p.playerJob == "Host")
                    continue;
                if(p.currentMemoryName == r.memory.ToString() && p.currentRoomName == r.roomName)
                {
                    r.PlayerCount ++;
                    Debug.Log(r.roomName + " has "+ r.PlayerCount);
                }
            }
            r.transform.Find("Number").GetChild(0).GetComponent<TextMeshProUGUI>().text = r.GetComponent<PCMapRoom>().PlayerCount.ToString();
            if(r.PlayerCount > 0)
                r.transform.Find("Number").gameObject.SetActive(true);
            else
                r.transform.Find("Number").gameObject.SetActive(false);
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
    public void AddCluePrefab(string clueID, Memory memory)
    {
        tempClue = Instantiate(ResourceManager.Instance.GetClueBtn(clueID));
        tempClue.GetComponent<ClueBtn>().collectedAt = memory;
        string m = memory.ToString();
        foreach(Transform child in ClueBases.transform)
        {
            if(child.name.Contains(m))
            {
                child.gameObject.SetActive(true);
                if(child.name == m + "Content")
                    tempClue.GetComponent<Transform>().SetParent(child, true);
            }
        }
        
        tempClue.transform.localScale = new Vector3(1f, 1f, 1f);
        BaseUIManager.Instance.AddClueBtn(clueID, tempClue);

        ResourceManager.Instance.allClueCount --;
        pv.RPC(nameof(SyncClueCount), RpcTarget.All, ResourceManager.Instance.allClueCount);
        DisconnectHandler.Instance.SaveDisconnectedPlayerStatus();
        tempClue = null;
    }
    [PunRPC]
    public void SyncClueCount(int n)
    {
        ResourceManager.Instance.allClueCount = n;
    }

    #endregion

    #region Puzzle Related Functions
    public void AddPuzzlePrefab(string puzzleName, Memory memory)
    {
        tempPuzzle = Instantiate(ResourceManager.Instance.GetPuzzleBtn(puzzleName));
        tempPuzzle.GetComponent<PuzzleBtn>().collectedAt = memory;
        string m = memory.ToString();
        foreach(Transform child in PuzzleBases.transform)
        {
            if(child.name.Contains(m))
            {
                child.gameObject.SetActive(true);
                if(child.name == m + "Content")
                    tempPuzzle.GetComponent<Transform>().SetParent(child, true);
            }
        }
        tempPuzzle.transform.localScale = new Vector3(1f, 1f, 1f);
        inBasePuzzleBtns.Add(puzzleName, tempPuzzle);
        BaseUIManager.Instance.AddPuzzleBtns(puzzleName, tempPuzzle);

        
        DisconnectHandler.Instance.SaveDisconnectedPlayerStatus();
        tempPuzzle = null;
    }

    // Call when puzzle solved, update sprite
    public void UpdatePuzzleBtnSolved(string puzzleName)
    {
        inBasePuzzleBtns[puzzleName].GetComponent<PuzzleBtn>().ShowSolvedMark();
    }
    public void UnlockDoor(Memory memory, string targetRoom)
    {
        pv.RPC(nameof(MasterUnlockDoor), RpcTarget.MasterClient, memory, targetRoom);
    }
    [PunRPC]
    public void MasterUnlockDoor(Memory memory, string targetRoom)
    {
        pv.RPC(nameof(UnlockDoorSynchronize), RpcTarget.AllBufferedViaServer, memory, targetRoom);
    }
    [PunRPC]
    public void UnlockDoorSynchronize(Memory memory, string targetRoom)
    {
        List<DoorInfo> doors = MemoryUI_Dic[memory.ToString()].GetComponent<MemoryInfo>().Doors;
        List<Rooms> rooms = MemoryUI_Dic[memory.ToString()].GetComponent<MemoryInfo>().Rooms;
        foreach(DoorInfo door in doors)
        {
            if(door.targetRoom.name == targetRoom)
            {
                door.gameObject.SetActive(true);
            }
        }
        foreach(Rooms room in rooms)
        {
            if(room.roomName == targetRoom)
            {
                //room.gameObject.SetActive(true);
                pv.RPC(nameof(ShowHiddenRoomPC), RpcTarget.MasterClient, memory, targetRoom);
                room.isHidden  = false;
            }
        }
    }

    //Show hidden room on PC map
    [PunRPC]
    public void ShowHiddenRoomPC(Memory memory, string room)
    {
        foreach(Transform child in PCMap.transform)
        {
            if(child.name == memory.ToString())
            {
                foreach(Transform r in child)
                {          
                    if(r.name == room)
                    {
                        r.gameObject.GetComponent<CanvasGroup>().alpha = 1f;
                    }
                }
            }
        }
    }


    public void TransferPuzzleSynchronize(string puzzleID, string playerJob, Memory memory)
    {
        Destroy(inBasePuzzleBtns[puzzleID].gameObject);
        inBasePuzzleBtns.Remove(puzzleID);
        pv.RPC(nameof(TransferPuzzle), RpcTarget.All, puzzleID, playerJob, memory);
    }

    [PunRPC]
    public void TransferPuzzle(string puzzleID, string playerJob, Memory memory)
    {
        if (playerController.Instance.playerJob == playerJob)
        {
            AddPuzzlePrefab(puzzleID, memory);
            BaseUIManager.Instance.SpawnNotificationPanel("Receive Puzzle", "You receive a puzzle from other players", 1, -1f);
        }
    }

    #endregion

    #region Unlock Memory & Teleport Related Functions

    private void InitializeMemoryUIDic()
    {
        foreach (GameObject memoryUI in MemoryUI_List)
        {
            MemoryUI_Dic.Add(memoryUI.GetComponent<MemoryInfo>().memory.ToString(), memoryUI);
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
        pv.RPC(nameof(UnlockMemorySynchronize), RpcTarget.AllBufferedViaServer, memory);
    }

    [PunRPC]
    private void UnlockMemorySynchronize(Memory memory)
    {
        unlockedMemoryInOverviewDic[memory.ToString()].gameObject.SetActive(true);
        if(PhotonNetwork.IsMasterClient)
        {
            foreach(Transform child in PCMap.transform)
            {
                if(child.name == memory.ToString())
                {
                    child.gameObject.SetActive(true);
                    break;
                }
            }
        }
    }

    public void UnlockTeleport(Memory fromMemory, Memory toMemory)
    {
        pv.RPC(nameof(MasterUnlockRelatedTeleport), RpcTarget.MasterClient, fromMemory, toMemory);
    }
    [PunRPC]
    public void MasterUnlockRelatedTeleport(Memory fromMemory, Memory toMemory)
    {
        pv.RPC(nameof(UnlockRelatedTeleport), RpcTarget.AllBufferedViaServer, fromMemory, toMemory);
    }

    [PunRPC]
    private void UnlockRelatedTeleport(Memory fromMemory, Memory toMemory)
    {
        SoundManager.Instance.PlaySoundEffect("VOE_NewAreaUnlocked");
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

    public void UnlockAllTeleport()
    {
        pv.RPC(nameof(UnlockAllTeleportRPC), RpcTarget.All);
    }
    [PunRPC]
    private void UnlockAllTeleportRPC()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            foreach(Transform child in PCMap.transform)
            {
                if(child.name == "BishopMemory")
                {
                    child.gameObject.SetActive(false);
                    continue;
                }
                else
                    child.gameObject.SetActive(true);
            }
        }
        foreach (Button teleportBtn in teleportBtnList)
        {
            teleportBtn.gameObject.SetActive(true);
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
        NotificationScript tempNoti = BaseUIManager.Instance.SpawnNotificationPanel(title, content, 2, -1f);
        tempNoti.AddFunctiontoYesButton(() => TeleportToFrom(fromMemory, toMemory, false), true);
    }
    public void TeleportToFrom(Memory fromMemory, Memory toMemory, bool force)
    {
        if(playerController.Instance.currentAP <= 0 && !force)
        {
            BaseUIManager.Instance.SpawnNotificationPanel("No Action Point!", "You don't have any action point left!", 1, 3f);
            return;
        }
        if(!force)
        {
            playerController.Instance.Change_currentAP(-1);
        }        
        MemoryUI_Dic[fromMemory.ToString()].SetActive(false);
        MemoryUI_Dic[toMemory.ToString()].SetActive(true);
        memoryTitleText.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = toMemory.ToString();

        //Change current memory
        playerController.Instance.currentMemory = MemoryUI_Dic[toMemory.ToString()].GetComponent<Transform>();

        //Change current
        //
        Component[] tempRooms = playerController.Instance.currentMemory.GetComponentsInChildren<Rooms>();
        foreach(Rooms room in tempRooms)
        {
            if (room.firstRoominMemory)
            {
                Rooms originalRoom = playerController.Instance.currentRoom;
                playerController.Instance.currentRoom = room;
                playerController.Instance.SyncRoomMemory(room.name, toMemory.ToString());
                // pv.RPC(nameof(PCMapUpdatePlayerCount), RpcTarget.MasterClient, fromMemory, originalRoom.name, toMemory, room.name);
                pv.RPC(nameof(MasterPCMapPLayerCount), RpcTarget.MasterClient);
                
                break;
            }
        }


        

    }

    public void ExitTutorialChangeStage()
    {
        StartCoroutine(WaitSecondForTeleport(1f));
        teleportBtnList[0].gameObject.SetActive(true);
        if(PhotonNetwork.IsMasterClient)
            TimerManager.Instance.SwitchStage(gamestage.Discussion);
    }
    IEnumerator WaitSecondForTeleport(float sec)
    {
        yield return new WaitForSeconds(sec);
        PCMap.transform.GetChild(0).gameObject.SetActive(false);
        PCMap.transform.GetChild(1).gameObject.SetActive(true);
        pv.RPC(nameof(TeleportEveryone), RpcTarget.OthersBuffered, Memory.BishopMemory, Memory.LawyerOffice);
    }
    [PunRPC]
    public void TeleportEveryone(Memory fromMemory, Memory toMemory)
    {
        TeleportToFrom(fromMemory, toMemory, true);
    }
    [PunRPC]
    public void TeleportSynonPCMap(Memory memory)
    {
        UnlockMemoryInOverview(memory);
    }

    #endregion

    #region AP (Action Points) Related Functions
    public int GetPlayerInitialAP()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            return 0;
        }
        if(UIManager.Instance.superAP)
        {
            return 20;
        }
        else if(playerController.Instance.currentMemory.GetComponent<MemoryInfo>().memory == Memory.BishopMemory && PhotonNetwork.CurrentRoom.CustomProperties["gameRunning"].Equals(false))
        {
            if(playerController.Instance.playerJob == "Security Guard")
            {
                return playerTutorialAP + 1;
            }
            else
                return playerTutorialAP;
        }
        else
        {   if(playerController.Instance.playerJob == "Security Guard")
            {
                return playerNormalAP + 1;
            }
            else
                return playerNormalAP;
        }
    }

    private bool CheckAllPlayer()
    {
        GameObject[] playerList = GameObject.FindGameObjectsWithTag("Player");

        foreach(var player in playerList)
        {
            if (player.GetComponent<playerController>().stageNow != gamestage.Discussion)
            {
                return false;
            }
        }

        return true;
    }

    #endregion

    #region Interest Points Related Functions
    
    // Add interest point to the dictionary
    public void AddInterestPoint(string name, GameObject interestPoint)
    {
        interestPoints.Add(name, interestPoint);
        allInterestPointCount += 1;
    }
    // functions to synchronize the item count of one interest points
    public void SynchronizeInterestPoint(string ipName, int itemNum)
    {
        pv.RPC(nameof(MasterUpdateGivenIPCNT), RpcTarget.MasterClient, ipName, itemNum);
    }
    [PunRPC]
    private void MasterUpdateGivenIPCNT(string ipName, int itemNum)
    {
        pv.RPC(nameof(UpdateGivenIPCNT), RpcTarget.AllBufferedViaServer, ipName, itemNum);
    }

    [PunRPC]
    private void UpdateGivenIPCNT(string ipName, int itemNum)
    {
        interestPoints[ipName].GetComponent<InterestPointInfo>().changeIP_Current(1);
        interestPoints[ipName].GetComponent<InterestPointInfo>().itemCollected(itemNum);
    }

    //Temporary disable or enable interest point when someone is collecting items
    public void SwitchInterestPointActive(string ipName, bool active)
    {
        pv.RPC(nameof(switchInterestPointActiveRPC), RpcTarget.AllBufferedViaServer, ipName, active);
    }

    [PunRPC]
    private void switchInterestPointActiveRPC(string ipName, bool active)
    {
        interestPoints[ipName].transform.Find("PB Loop - Radial Material").gameObject.SetActive(!active);
        interestPoints[ipName].GetComponent<Button>().interactable = active;
    }

    // functions to synchronize whether interest points are active or not depends on if there are items left
    public void SynchronizeInterestPointStatus(string ipName, Memory memory)
    {
        pv.RPC(nameof(MasterUpdateIPFullyCollected), RpcTarget.MasterClient, ipName, memory);
    }
    [PunRPC]
    private void MasterUpdateIPFullyCollected(string ipName, Memory memory)
    {
        pv.RPC(nameof(UpdateIPFullyCollected), RpcTarget.AllBufferedViaServer, ipName, memory);
    }

    [PunRPC]
    private void UpdateIPFullyCollected(string ipName, Memory memory)
    {
        interestPoints[ipName].SetActive(false);
        allInterestPointCount --;
        // minus interest point count from memory
        if(MemoryUI_Dic[memory.ToString()].GetComponent<MemoryInfo>().UpdateInterestPointCount(-1) && PhotonNetwork.IsMasterClient)
        {
            // if all interest points are collected, change stage
            ExitTutorialChangeStage();
        }
        else if(allInterestPointCount == 0 && PhotonNetwork.IsMasterClient)
        {
            Debug.Log("All interest points are collected");
            TimerManager.Instance.SwitchStage(gamestage.Accusation);
        }
    }

    public void SynchronizeActivateInterestPoint(string ipName)
    {
        pv.RPC(nameof(ActivateInterestPoint), RpcTarget.AllBufferedViaServer, ipName);
    }

    [PunRPC]
    private void ActivateInterestPoint(string ipName)
    {
        interestPoints[ipName].SetActive(true);
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
    public void MasterChangeStage(gamestage stage)
    {
        TimerManager.Instance.SwitchStage(stage);
    }
    public void AskMasterChangeStage(gamestage stage)
    {
        pv.RPC(nameof(MasterChangeStage), RpcTarget.MasterClient, stage);
    }
    IEnumerator WaitSecondForChangeStage(float sec)
    {
        yield return new WaitForSeconds(sec);
        pv.RPC(nameof(MasterChangeStage), RpcTarget.MasterClient, gamestage.Discussion);
    }
    #endregion
}

using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine;

public enum Memory
{
    None, Mansion, StreetCorner, LawyerOffice, NightClub, VoidBase
}

public class InvestigationManager : Singleton<InvestigationManager>
{
    // Player Movement Part
    [SerializeField]
    private GameObject fullMap;
    private float mapGap;
    private string currentMemoryMap;
    //New map movement
    [SerializeField]
    private Transform activeMemory;
    private string currentRoomName;


    // Player Clue Base Part
    [SerializeField]
    private GameObject ClueBase;
    private GameObject tempClue;

    // Player Puzzle Base Part
    [SerializeField]
    private GameObject PuzzleBase;
    private GameObject tempPuzzle;
    private Dictionary<string, GameObject> inBasePuzzleBtns = new Dictionary<string, GameObject>();

    // Synchronize Interest Point Status Part
    [SerializeField]
    private PhotonView pv;
    [SerializeField]
    private List<GameObject> interestPointList;
    private Dictionary<string, GameObject> interestPoints = new Dictionary<string, GameObject>();

    [SerializeField]
    private List<Button> unlockedMemoryInOverview;
    private Dictionary<string, Button> unlockedMemoryInOverviewDic = new Dictionary<string, Button>();

    [SerializeField]
    private List<teleportList> teleportLists;

    [System.Serializable]
    private class teleportList
    {
        public string teleportListName;
        public List<Button> listContent;
    }

    private void Start()
    {
        //The distance every two area
        mapGap = Vector2.Distance(fullMap.transform.Find("Area01").transform.position, fullMap.transform.Find("Area02").transform.position);
        Debug.Log(mapGap);

        pv = GetComponent<PhotonView>();
        PreloadInterestPoints();
        playerController.Instance.maxAP = GetPlayerInitialAP();
        playerController.Instance.currentAP = playerController.Instance.maxAP;
        playerController.Instance.Change_currentAP(0);
        playerController.Instance.Change_maxAP(playerController.Instance.maxAP);

        foreach(Button memoryOverview in unlockedMemoryInOverview)
        {
            unlockedMemoryInOverviewDic.Add(memoryOverview.name, memoryOverview);
        }

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
        BaseUIManager.Instance.SpawnNotificationPanel("Move Area?", "Use 1AP to move area?", 2, -1f);
        NotificationScript.yesButtonEvent.AddListener(() => MoveRoom(room));
        //NotificationScript.yesButtonEvent.AddListener(() => playerController.Instance.Change_currentAP(-1));
    }
    private void MoveRoom(Rooms room)
    {
        Debug.Log("Attempt move to " + room.roomName);
        if(currentRoomName == room.roomName)
        {
            Debug.Log("Failed to move to " + room.roomName + " because it is already in this room");
            return;
        }
        else
        {
            StartCoroutine(RoomCoroutine(room, 0.5f));
        }
        
    }
    public void MoveDialog(string direction)
    {
        BaseUIManager.Instance.SpawnNotificationPanel("Move Area?", "Use 1AP to move area?", 2, -1f);
        NotificationScript.yesButtonEvent.AddListener(() => MoveMap(direction));
        NotificationScript.yesButtonEvent.AddListener(() => playerController.Instance.Change_currentAP(-1));
    }
    private void MoveMap(string direction)
    {
        switch(direction)
        {
            case "L":
                StartCoroutine(MapCoroutine(new Vector2(fullMap.transform.position.x + mapGap, fullMap.transform.position.y), 0.5f));
                break;
            case "R":
                StartCoroutine(MapCoroutine(new Vector2(fullMap.transform.position.x - mapGap, fullMap.transform.position.y), 0.5f));
                break;
            case "U":
                StartCoroutine(MapCoroutine(new Vector2(fullMap.transform.position.x, fullMap.transform.position.y - mapGap), 0.5f));
                break;
            case "D":
                StartCoroutine(MapCoroutine(new Vector2(fullMap.transform.position.x, fullMap.transform.position.y + mapGap), 0.5f));
                break;
        }
    }
    IEnumerator RoomCoroutine(Rooms room, float sec)
    {
        float time = 0f;
        Vector2 startPos = activeMemory.localPosition;
        while(time <= sec)
        {
            activeMemory.localPosition = Vector2.Lerp(startPos, -room.transform.localPosition, time/sec);
            time +=Time.deltaTime;
            yield return null;
        }
        activeMemory.localPosition = -room.transform.localPosition;
        currentRoomName = room.roomName;
    }
    IEnumerator MapCoroutine(Vector2 targetPos, float sec)
    {
        float time = 0f;
        Vector2 startPos = fullMap.transform.position;
        while(time <= sec)
        {
            fullMap.transform.position = Vector2.Lerp(startPos, targetPos, time/sec);
            time +=Time.deltaTime;
            yield return null;
        }
        fullMap.transform.position = targetPos;
    }
    #endregion

    #region Clue Related Functions
    // function: when player click on interest point, add a clue to their clue base
    public void AddCluePrefab(string clueName)
    {
        tempClue = (GameObject)Instantiate(ResourceManager.Instance.GetClueBtn(clueName));
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
    public void UnlockMemoryInOverview(Memory memory)
    {
        pv.RPC(nameof(UnlockMemorySynchronize), RpcTarget.All, memory);
    }

    [PunRPC]
    private void UnlockMemorySynchronize(Memory memory)
    {
        unlockedMemoryInOverviewDic[memory.ToString()].gameObject.SetActive(true);
    }

    public void UnlockTeleport(int fromMemory, int toMemory)
    {
        pv.RPC(nameof(UnlockTeleportSynchronize), RpcTarget.All, fromMemory, toMemory);
    }

    [PunRPC]
    private void UnlockTeleportSynchronize(int fromMemory, int toMemory)
    {
        MemoryUpdate(teleportLists[fromMemory - 1].listContent, teleportLists[toMemory - 1].listContent, fromMemory, toMemory);
    }

    private void MemoryUpdate(List<Button> TeleportFromList, List<Button> TeleportToList, int fromMemoryID, int toMemoryID)
    {
        foreach (Button teleport in TeleportFromList)
        {
            if (teleport.GetComponent<TeleportInfo>().teleportToMemory.name == "Memory" + toMemoryID)
            {
                teleport.gameObject.SetActive(true);
            }
        }

        foreach (Button teleport in TeleportToList)
        {
            if (teleport.GetComponent<TeleportInfo>().teleportToMemory.name == "Memory" + fromMemoryID)
            {
                teleport.gameObject.SetActive(true);
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

    public void SynchronizeInterestPoint(string ipName)
    {
        pv.RPC("UpdateGivenIPCNT", RpcTarget.All, ipName);
    }

    [PunRPC]
    private void UpdateGivenIPCNT(string ipName)
    {
        interestPoints[ipName].GetComponent<InterestPointInfo>().changeIP_Current(1);
    }

    // functions to synchronize whether interest points are active or not
    public void SynchronizeInterestPointStatus(string ipName)
    {
        pv.RPC("UpdateIPFullyCollected", RpcTarget.All, ipName);
    }

    [PunRPC]
    private void UpdateIPFullyCollected(string ipName)
    {
        interestPoints[ipName].SetActive(false);
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

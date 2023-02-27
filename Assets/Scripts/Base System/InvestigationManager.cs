using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine;

public class InvestigationManager : Singleton<InvestigationManager>
{

    // Player Clue Base Part
    [SerializeField]
    private GameObject ClueBase;
    private GameObject tempClue;

    // Player Puzzle Base Part
    [SerializeField]
    private GameObject PuzzleBase;
    private GameObject tempPuzzle;
    private Dictionary<string, GameObject> inBasePuzzleBtns = new Dictionary<string, GameObject>();

    [SerializeField]
    private PhotonView pv;
    [SerializeField]
    private List<GameObject> interestPointList;
    private Dictionary<string, GameObject> interestPoints = new Dictionary<string, GameObject>();

    private void Start()
    {
        pv = GetComponent<PhotonView>();
        PreloadInterestPoints();
        playerController.Instance.maxAP = GetPlayerInitialAP();
        playerController.Instance.currentAP = playerController.Instance.maxAP;
        playerController.Instance.Change_currentAP(0);
        playerController.Instance.Change_maxAP(playerController.Instance.maxAP);

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
            return 1;
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

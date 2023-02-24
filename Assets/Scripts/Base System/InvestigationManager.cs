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
        //playerController.Instance.playerJob

    }

    private void Update()
    {
        /*if (playerController.Instance.currentAP == 0)
        {
            playerController.Instance.stageNow = PlayerManagerForAll.gamestage.Dissussion;
            if (CheckAllPlayer())
            {
                playerController.Instance.ChangeStage(PlayerManagerForAll.gamestage.Dissussion);
            }
        }*/
    }

    #region Clue Related Functions
    // function: when player click on interest point, add a clue to their clue base
    public void AddCluePrefab(string clueName)
    {
        tempClue = (GameObject)Instantiate(ResourceManager.Instance.GetClueBtn(clueName));
        tempClue.GetComponent<Transform>().SetParent(ClueBase.GetComponent<Transform>(), true);
        //playerController.Instance.Cost_currentAP(1);
    }

    #endregion


    #region Puzzle Related Functions
    public void AddPuzzlePrefab(string puzzleName)
    {
        tempPuzzle = (GameObject)Instantiate(ResourceManager.Instance.GetPuzzleBtn(puzzleName));
        tempPuzzle.GetComponent<Transform>().SetParent(PuzzleBase.GetComponent<Transform>(), true);
        inBasePuzzleBtns.Add(puzzleName, tempPuzzle);
        
        //playerController.Instance.Cost_currentAP(1);
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
        if (playerController.Instance.playerJob == "")
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
        var photonViews = FindObjectsOfType<PhotonView>();
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
        Debug.Log(interestPoints[ipName].name);
        interestPoints[ipName].GetComponent<InterestPointInfo>().changeIP_Current(1);
    }

    public void AddInterestPoint(string ipName, Vector2 location)
    {

    }
}

using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine;

public class InvestigationManager : Singleton<InvestigationManager>
{

    // Player Clue Base Part
    [SerializeField]
    private GameObject ClueBase;
    private GameObject tempClue;
    private Dictionary<string, GameObject> cluePrefabs = new Dictionary<string, GameObject>();

    // Player Puzzle Base Part
    [SerializeField]
    private GameObject PuzzleBase;
    private GameObject tempPuzzle;
    private Dictionary<string, GameObject> puzzlePrefabs = new Dictionary<string, GameObject>();

    // Player Action Point Part
    [SerializeField]
    //private GameObject APUI;


    private void Start()
    {
        LoadAllCluePrefabs();
        //LoadAllPuzzlePrefabs();
        //playerController.Instance.playerJob
    }

    private void Update()
    {
        if (playerController.Instance.currentAP == 0)
        {
            playerController.Instance.stageNow = PlayerManagerForAll.gamestage.Dissussion;
            if (CheckAllPlayer())
            {
                playerController.Instance.ChangeStage(PlayerManagerForAll.gamestage.Dissussion);
            }
        }
        
    }

    #region Clue Related Functions
    public void LoadAllCluePrefabs()
    {
        foreach(GameObject cluePrefab in Resources.LoadAll("CluePrefabs/"))
        {
            cluePrefabs.Add(cluePrefab.name, cluePrefab);
        }
    }

    // function: when player click on interest point, add a clue to their clue base
    public void AddCluePrefab(string clueName)
    {
        tempClue = (GameObject)Instantiate(cluePrefabs[clueName]);
        tempClue.GetComponent<Transform>().SetParent(ClueBase.GetComponent<Transform>(), true);
        currentAP--;
    }

    #endregion

    #region Puzzle Related Functions
    public void LoadAllPuzzlePrefabs()
    {
        foreach(GameObject puzzlePrefab in Resources.LoadAll("PuzzlePrefabs/"))
        {
            puzzlePrefabs.Add(puzzlePrefab.name, puzzlePrefab);
        }
    }

    public void AddPuzzlePrefab(string puzzleName)
    {
        tempPuzzle = (GameObject)Instantiate(puzzlePrefabs[puzzleName]);
        tempPuzzle.GetComponent<Transform>().SetParent(PuzzleBase.GetComponent<Transform>(), true);
        currentAP--;
    }

    public void RemovePuzzlePrefab(string puzzleName)
    {
        PuzzleBase.transform.Find(puzzleName);
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

    public void AddInterestPoint(string ipName, Vector2 location)
    {

    }
}

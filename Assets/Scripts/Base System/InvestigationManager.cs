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

    private void Start()
    {
        LoadAllCluePrefabs();
    }

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
    }

    public void AddInterestPoint(string ipName, Vector2 location)
    {

    }
}

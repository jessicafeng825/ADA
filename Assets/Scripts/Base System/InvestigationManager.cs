using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine;

public class InvestigationManager : Singleton<InvestigationManager>
{
    [SerializeField]
    private GameObject ClueBase;
    private GameObject newClue;
    private GameObject tempClue;
    private Object[] cluePrefabs;

    private void Start()
    {
        LoadAllCluePrefabs();
    }

    public void LoadAllCluePrefabs()
    {
        cluePrefabs = Resources.LoadAll("CluePrefabs/");
    }

    public void AddCluePrefab(int clueID)
    {
        tempClue = (GameObject)Instantiate(cluePrefabs[clueID]);
        tempClue.GetComponent<Transform>().SetParent(ClueBase.GetComponent<Transform>(), true); 
    }

    public void AddInterestPoint(string ipName, Vector2 location)
    {

    }
}

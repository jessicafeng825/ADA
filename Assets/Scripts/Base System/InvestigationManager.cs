using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine;

public class InvestigationManager : Singleton<InvestigationManager>
{
    [SerializeField]
    private GameObject ClueBase;
    private GameObject tempClue;
    //private Object[] cluePrefabs;

    private Dictionary<string, GameObject> cluePrefabs = new Dictionary<string, GameObject>();

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

    public void AddCluePrefab(string clueName)
    {
        tempClue = (GameObject)Instantiate(cluePrefabs[clueName]);
        tempClue.GetComponent<Transform>().SetParent(ClueBase.GetComponent<Transform>(), true); 
    }

    public void AddInterestPoint(string ipName, Vector2 location)
    {

    }
}

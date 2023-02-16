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

    public void AddCluePrefab(string clueName)
    {
        newClue = Resources.Load<GameObject>("CluePrefabs/" + clueName);
        tempClue = (GameObject)Instantiate(newClue);
        tempClue.GetComponent<Transform>().SetParent(ClueBase.GetComponent<Transform>(), true); 
    }

    public void AddInterestPoint(string ipName, Vector2 location)
    {

    }
}

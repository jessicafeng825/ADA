using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : Singleton<ResourceManager>
{
    private Dictionary<string, GameObject> clueBtnDic = new Dictionary<string, GameObject>();
    private Dictionary<string, GameObject> clueInfoDic = new Dictionary<string, GameObject>();

    public int allClueCount;
    private Dictionary<string, GameObject> puzzleBtnDic = new Dictionary<string, GameObject>();
    private Dictionary<string, GameObject> puzzleInteractionDic = new Dictionary<string, GameObject>();

    private Dictionary<string, GameObject> uiElements = new Dictionary<string, GameObject>();

    void Start()
    {
        LoadAllClueBtns();
        LoadAllClueInfos();

        LoadAllPuzzleBtns();
        LoadAllPuzzleInteractions();

        LoadUIElements();
    }

    private void LoadAllClueBtns()
    {
        foreach (GameObject cluePrefab in Resources.LoadAll("CluesRelated/ClueBtns/"))
        {
            allClueCount++;
            clueBtnDic.Add(cluePrefab.GetComponent<ClueBtn>().GetClueID(), cluePrefab);
        }
        Debug.Log("All clue count: " + allClueCount);
    }

    public GameObject GetClueBtn(string clueID)
    {
        return clueBtnDic[clueID];
    }

    private void LoadAllClueInfos()
    {
        foreach (GameObject clueInfo in Resources.LoadAll("CluesRelated/ClueInfos/"))
        {
            clueInfoDic.Add(clueInfo.GetComponent<ClueInfo>().GetClueID(), clueInfo);
        }
    }

    public GameObject GetClueInfo(string clueID)
    {
        return clueInfoDic[clueID];
    }

    private void LoadAllPuzzleBtns()
    {
        foreach (GameObject puzzlePrefab in Resources.LoadAll("PuzzlesRelated/PuzzleBtns/"))
        {
            puzzleBtnDic.Add(puzzlePrefab.GetComponent<PuzzleBtn>().GetPuzzleID(), puzzlePrefab);
        }
    }

    public GameObject GetPuzzleBtn(string puzzleID)
    {
        return puzzleBtnDic[puzzleID];
    }

    private void LoadAllPuzzleInteractions()
    {
        foreach (GameObject puzzleInteraction in Resources.LoadAll("PuzzlesRelated/PuzzleInteractions/"))
        {
            puzzleInteractionDic.Add(puzzleInteraction.name, puzzleInteraction);
        }
    }

    public GameObject GetPuzzleInteraction(string puzzleName)
    {
        return puzzleInteractionDic[puzzleName];
    }
    //For instantiating UI elements
    private void LoadUIElements()
    {
        foreach (GameObject ui in Resources.LoadAll("UI/"))
        {
            uiElements.Add(ui.name, ui);
        }
    }
    public GameObject GetUIElement(string uiName)
    {
        return uiElements[uiName];
    }
}

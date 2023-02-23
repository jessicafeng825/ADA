using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : Singleton<ResourceManager>
{
    private Dictionary<string, GameObject> clueInBagDic = new Dictionary<string, GameObject>();
    private Dictionary<string, GameObject> puzzleInBagDic = new Dictionary<string, GameObject>();
    private Dictionary<string, Sprite> cluePicsDic = new Dictionary<string, Sprite>();
    private Dictionary<string, GameObject> puzzleInteractionDic = new Dictionary<string, GameObject>();

    void Start()
    {
        LoadAllClueBtns();
        LoadAllPics();

        LoadAllPuzzleBtns();
        LoadAllPuzzleInteractions();
    }

    private void LoadAllClueBtns()
    {
        foreach (GameObject cluePrefab in Resources.LoadAll("CluesRelated/ClueBtns/"))
        {
            clueInBagDic.Add(cluePrefab.name, cluePrefab);
        }
    }

    public GameObject GetClueBtn(string clueName)
    {
        return clueInBagDic[clueName];
    }

    private void LoadAllPuzzleBtns()
    {
        foreach (GameObject puzzlePrefab in Resources.LoadAll("PuzzlesRelated/PuzzleBtns/"))
        {
            puzzleInBagDic.Add(puzzlePrefab.name, puzzlePrefab);
        }
    }

    public GameObject GetPuzzleBtn(string puzzleName)
    {
        return puzzleInBagDic[puzzleName + "Btn"];
    }

    private void LoadAllPics()
    {
        foreach (Sprite cluePic in Resources.LoadAll<Sprite>("CluesRelated/CluePics/"))
        {
            cluePicsDic.Add(cluePic.name, cluePic);
        }
    }

    public Sprite GetCluePic(string clueName)
    {
        return cluePicsDic[clueName];
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
}

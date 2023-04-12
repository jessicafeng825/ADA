using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PuzzleBtn : MonoBehaviour
{
    [SerializeField]
    private string puzzleID;
    public Memory collectedAt;
    public bool isViewed;
    private GameObject solvedMark;
    [SerializeField]
    private GameObject newClueMark;

    private void Start()
    {
        solvedMark = this.transform.Find("solvedmark").gameObject;
        GetComponent<Button>().onClick.AddListener(ShowPuzzle);
        newClueMark.SetActive(true);
    }

    private void ShowPuzzle()
    {
        BaseUIManager.Instance.ShowPuzzleUI(puzzleID);
        newClueMark.gameObject.SetActive(false);
        isViewed = true;
    }

    public void ShowSolvedMark()
    {
        solvedMark.SetActive(true);
    }
    public string GetPuzzleID()
    {
        return puzzleID;
    }
}

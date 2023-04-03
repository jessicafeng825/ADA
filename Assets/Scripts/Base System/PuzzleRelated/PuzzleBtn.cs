using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PuzzleBtn : MonoBehaviour
{
    [SerializeField]
    private string puzzleID;
    private GameObject solvedMark;

    private void Start()
    {
        solvedMark = this.transform.Find("solvedmark").gameObject;
        GetComponent<Button>().onClick.AddListener(ShowPuzzle);
    }

    private void ShowPuzzle()
    {
        BaseUIManager.Instance.ShowPuzzleUI(puzzleID);
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

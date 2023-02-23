using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PuzzleBtn : MonoBehaviour
{
    [SerializeField]
    private string puzzleName;

    [SerializeField]
    private GameObject solvedMark;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(ShowPuzzle);
    }

    private void ShowPuzzle()
    {
        BaseUIManager.Instance.ShowPuzzleUI(puzzleName);
    }

    public void ShowSolvedMark()
    {
        solvedMark.SetActive(true);
    }
}

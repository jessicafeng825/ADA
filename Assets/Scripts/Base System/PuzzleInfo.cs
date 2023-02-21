using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PuzzleInfo : MonoBehaviour
{
    [SerializeField]
    private string puzzleName;
    [SerializeField]
    private bool isCompleted, provideClue, unlockArea;
    [SerializeField]
    private string clueProvided, areaUnlocked;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(ShowPuzzle);
    }

    private void ShowPuzzle()
    {
        BaseUIManager.Instance.ShowPuzzleUI(puzzleName);
    }

    public void SetCompleted()
    {
        isCompleted = true;
    }
}

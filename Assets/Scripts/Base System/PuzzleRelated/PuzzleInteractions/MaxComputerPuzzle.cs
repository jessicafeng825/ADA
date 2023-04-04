using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MaxComputerPuzzle : PuzzleInfo
{
    private bool email01Collected;
    private bool email02Collected;
    private bool codeCollected;

    [SerializeField]
    private GameObject email01Button;

    [SerializeField]
    private GameObject email02Button;

    [SerializeField]
    private GameObject codeButton;

    [SerializeField]
    private string email01ClueID;

    [SerializeField]
    private string email02ClueID;

    [SerializeField]
    private string codeClueID;
    
    private void Start()
    {

        this.transform.Find("Btn_close").GetComponent<Button>().onClick.AddListener(HideThisUI);
        email01Button.GetComponent<Button>().onClick.AddListener(delegate{collectClue(email01ClueID);});
        email02Button.GetComponent<Button>().onClick.AddListener(delegate{collectClue(email02ClueID);});
        codeButton.GetComponent<Button>().onClick.AddListener(delegate{collectClue(codeClueID);});
        
    }

    public void collectClue(string clue)
    {
        if(clue == email01ClueID && !email01Collected)
        {
            Debug.Log("email01 collected");
            InvestigationManager.Instance.AddCluePrefab(clue, collectedAt);
            BaseUIManager.Instance.SpawnNotificationPanel("New Clue!", "You got 1 new clues!", 1, -1f);
            email01Collected = true;
        }
        else if(clue == email02ClueID && !email02Collected)
        {
            Debug.Log("email02 collected");
            InvestigationManager.Instance.AddCluePrefab(clue, collectedAt);
            BaseUIManager.Instance.SpawnNotificationPanel("New Clue!", "You got 1 new clues!", 1, -1f);
            email02Collected = true;
        }
        else if(clue == codeClueID && !codeCollected)
        {
            Debug.Log("code collected");
            InvestigationManager.Instance.AddCluePrefab(clue, collectedAt);
            BaseUIManager.Instance.SpawnNotificationPanel("New Clue!", "You got 1 new clues!", 1, -1f);
            codeCollected = true;
        }
        else
        {
            Debug.Log("Nothing");
        }
        if(email01Collected && email02Collected && codeCollected)
        {
            // Hide UI, Mark this puzzle with "solved";
            InvestigationManager.Instance.UpdatePuzzleBtnSolved(puzzleID);
            isSolved = true;
        }
    }

    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class CaesarCipher : PuzzleInfo
{
    [SerializeField]
    private Button submitBtn, closeBtn;
    [SerializeField]
    private TMPro.TMP_InputField enteredAnswer;
    [SerializeField]
    private string answer;

    private void Start()
    {
        submitBtn.onClick.AddListener(ClickSubmit);
        closeBtn.onClick.AddListener(HideThisUI);
    }

    private bool CheckAnswer()
    {
        if (answer == enteredAnswer.text)
        {
            return true;
        }
        else return false;
    }

    private void ClickSubmit()
    {
        if (!isSolved)
        {
            isSolved = true;
            if (CheckAnswer())
            {
                // entered answer correct
                Debug.Log("correct");
                // TODO: Hide UI, Mark this puzzle with "solved";
                InvestigationManager.Instance.UpdatePuzzleBtnSolved(puzzleName);
                // Trigger puzzle effect (unlock or clue)
                PuzzleSolveEffect();
            }
            else
            {
                // entered answer incorrect
                Debug.Log("entered answer is wrong");
            }
        }
    }

    private void PuzzleSolveEffect()
    {
        if (provideClue)
        {
            // give clue
            InvestigationManager.Instance.AddCluePrefab(clueProvided);
        }
        else if (unlockArea)
        {
            // unlock area
        }
    }

    private void HideThisUI()
    {
        gameObject.SetActive(false);
        BaseUIManager.Instance.HidePuzzleUI();
    }
}

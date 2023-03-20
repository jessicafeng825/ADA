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
            enteredAnswer.text = "correct";
            isSolved = true;
            return true;
        }
        else
        {
            enteredAnswer.text = "incorrect";
            return false;
        }
    }

    private void ClickSubmit()
    {
        if (!isSolved)
        {
            if (CheckAnswer())
            {
                // entered answer correct
                Debug.Log("correct");
                // Hide UI, Mark this puzzle with "solved";
                InvestigationManager.Instance.UpdatePuzzleBtnSolved(puzzleID);
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
}

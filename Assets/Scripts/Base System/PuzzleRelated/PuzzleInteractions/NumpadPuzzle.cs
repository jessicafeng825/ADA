using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NumpadPuzzle : PuzzleInfo
{
    [SerializeField]
    private TextMeshProUGUI answerText;

    [SerializeField]
    private string answer;

    private string enteredNum = "";

    public void NumEnter(int num)
    {
        if(num == -1 && enteredNum.Length > 0)
        {
            enteredNum.Remove(enteredNum.Length - 1);
        }
        if(num >= 0 && num <= 9 && enteredNum.Length < answer.Length)
        {
            enteredNum += num.ToString();
        }
        answerText.text = enteredNum;
    }
    
}

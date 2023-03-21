using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NumpadPuzzle : PuzzleInfo
{
    [SerializeField]
    private GameObject answerText;

    [SerializeField]
    private string answer;

    private string enteredNum = "";

    private void Start()
    {
        answerText.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = new string('0', answer.Length);
    }
    public void NumEnter(int num)
    {
        if(num == -1 && enteredNum.Length > 0)
        {
            enteredNum = enteredNum.Remove(enteredNum.Length - 1, 1);
            if(enteredNum.Length == 0)
            {
                answerText.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = new string('0', answer.Length);
                Debug.Log("empty");
                return;
            }
            answerText.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = enteredNum;
        }
        else if(num >= 0 && num <= 9 && enteredNum.Length < answer.Length)
        {
            enteredNum += num.ToString();
            answerText.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = enteredNum;
        }
    }
    public void CheckAnswer()
    {
        if(enteredNum == answer)
        {
            Debug.Log("correct");
            isSolved = true;
        }
        else
        {
            Debug.Log("incorrect");
        }
    }
    
}

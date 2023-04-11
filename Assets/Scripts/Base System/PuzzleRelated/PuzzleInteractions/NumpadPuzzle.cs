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
    private GameObject descriptionText;

    [SerializeField]
    private GameObject numpad;

    [SerializeField]
    private Color normalNumpadColor;

    [SerializeField]
    private Color incorrectNumpadColor;
    
    
    [SerializeField]
    private Color correctNumpadColor;

    [SerializeField]
    private string answer;

    private string enteredNum = "";

    private bool denying;
    public bool decoderActivated;

    private void Start()
    {

        this.transform.Find("Btn_close").GetComponent<Button>().onClick.AddListener(HideThisUI);
        answerText.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = new string('0', answer.Length);
        
    }
    void OnEnable()
    {        
        denying = false;
        numpad.GetComponent<Image>().color = normalNumpadColor;
        answerText.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = new string('0', answer.Length);
        enteredNum = enteredNum.Remove(0, enteredNum.Length);
    }


    public void NumEnter(int num)
    {
        if(denying)
            return;
        if(num == -1 && enteredNum.Length > 0)
        {
            enteredNum = enteredNum.Remove(enteredNum.Length - 1, 1);
            if(enteredNum.Length == 0)
            {
                answerText.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = new string('0', answer.Length);
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
    public bool CheckAnswer()
    {
        if(enteredNum == answer)
        {
            answerText.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "ACCESS GRANTED";
            numpad.GetComponent<Image>().color = correctNumpadColor;
            foreach(Transform child in numpad.transform.Find("NumberButtons"))
            {
                child.GetComponent<Image>().color = correctNumpadColor;
            }
            if (puzzleID == "Elevator Passcode")
            {
                BaseUIManager.Instance.SpawnNotificationPanel("Teleport Unlocked", "Teleport to the new area is unlocked in investigation!", 1, -1f);
            }
            else if(puzzleID == "Decoder")
            {
                descriptionText.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = "ACITIVATED!";
                BaseUIManager.Instance.SpawnNotificationPanel("Decoder Activated", "Decoder ca be used to unlock dynamic passcodes!", 1, -1f);
                decoderActivated = true;
            }
            isSolved = true;
            return true;
        }
        else
        {
            StartCoroutine(WrongResult("ACCESS DENIED"));
            return false;
        }
    }
    public void SubmitAnswer()
    {
        if (!isSolved)
        {
            if (CheckAnswer())
            {
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

    IEnumerator WrongResult(string result)
    {
        denying = true;
        answerText.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = result;
        numpad.GetComponent<Image>().color = incorrectNumpadColor;
        yield return new WaitForSeconds(1f);
        denying = false;
        numpad.GetComponent<Image>().color = normalNumpadColor;
        answerText.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = new string('0', answer.Length);
        enteredNum = enteredNum.Remove(0, enteredNum.Length);
    }
    
}

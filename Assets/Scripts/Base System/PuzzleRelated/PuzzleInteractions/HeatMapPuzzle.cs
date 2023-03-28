using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HeatMapPuzzle : PuzzleInfo
{
    [SerializeField]
    private GameObject answerText;

    [SerializeField]
    private string answer;

    [SerializeField]
    private GameObject enterbtnFinger;

    [SerializeField]
    private GameObject[] fingerPrints = new GameObject[6];

    [SerializeField]
    private GameObject numberButtons;

    private GameObject[] numbers= new GameObject[10];

    private string enteredNum = "";

    private bool denying;

    private void Start()
    {

        this.transform.Find("Btn_close").GetComponent<Button>().onClick.AddListener(HideThisUI);
        answerText.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = new string('0', answer.Length);
        if(fingerPrints != null)
        {
            for(int i = 0; i < numbers.Length; i++)
            {
                numbers[i] = numberButtons.transform.Find(i.ToString() + "btn").gameObject;
                Debug.Log(numbers[i]);
            }
            for(int i = 0; i < answer.Length; i++)
            {
                Debug.Log(answer[i].ToString());
                int temp;
                int.TryParse(answer[i].ToString(), out temp);
                fingerPrints[i].SetActive(true);
                fingerPrints[i].transform.SetParent(numbers[temp].transform);
                fingerPrints[i].transform.localPosition = new Vector3(Random.Range(-30, 30), Random.Range(-30, 30), 0);
                fingerPrints[i].transform.localRotation = Quaternion.Euler(0, 0, Random.Range(-40, 40));
            }
        }
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
            isSolved = true;
            return true;
        }
        else
        {
            StartCoroutine(ShowResult("ACCESS DENIED"));
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

    IEnumerator ShowResult(string result)
    {
        denying = true;
        answerText.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = result;
        yield return new WaitForSeconds(1f);
        denying = false;
        answerText.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = new string('0', answer.Length);
        enteredNum = enteredNum.Remove(0, enteredNum.Length);
    }
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HeatMapPuzzle : PuzzleInfo
{

    [SerializeField]
    private GameObject hintText;

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

    
    [SerializeField]
    private GameObject scanButton;
    private Animator heatMapAnim;


    private string enteredNum = "";

    private bool denying;

    private bool buttonPressed;
    private float scanTime;

    private bool show;

    private bool enable;
    private void Start()
    {
        heatMapAnim = GetComponent<Animator>();
        this.transform.Find("Btn_close").GetComponent<Button>().onClick.AddListener(HideThisUI);
        this.transform.Find("Btn_close").GetComponent<Button>().onClick.AddListener(delegate{ScriptEnableSwitch(false);});
        answerText.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = new string('0', answer.Length);
        if(fingerPrints != null)
        {
            for(int i = 0; i < numbers.Length; i++)
            {
                numbers[i] = numberButtons.transform.Find(i.ToString() + "btn").gameObject;
            }
            for(int i = 0; i < answer.Length; i++)
            {
                int temp;
                int.TryParse(answer[i].ToString(), out temp);
                fingerPrints[i].SetActive(true);
                fingerPrints[i].transform.SetParent(numbers[temp].transform);
                fingerPrints[i].transform.localPosition = new Vector3(Random.Range(-30, 30), Random.Range(-30, 30), 0);
                fingerPrints[i].transform.localRotation = Quaternion.Euler(0, 0, Random.Range(-40, 40));
            }
        }
    }
    void Update()
    {
        if(!enable)
        {            
            if(playerController.Instance.playerJob != "Lawyer")
            {
                hintText.SetActive(true);
                scanButton.SetActive(false);
                return;
            }
            else
            {
                hintText.SetActive(false);
                scanButton.SetActive(true);
            }
            enable = true;
        }
    }
    public void ScriptEnableSwitch(bool b)
    {
        enable = b;
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
            BaseUIManager.Instance.SpawnNotificationPanel("Unlocked", "A sound of a door unlocking", 1, 3f);
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
    public void ScanStart()
    {
        if(!show)
        {
            show = true;
            StartCoroutine(ScanTimer(3f));
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

    IEnumerator ScanTimer(float time)
    {
        heatMapAnim.SetBool("ScanBool", true);

        while(scanTime <= time)
        {
            scanTime += Time.deltaTime;
            for(int i = 0; i < answer.Length; i++)
            {
                fingerPrints[i].GetComponent<CanvasGroup>().alpha = Mathf.Lerp(0, 1, scanTime / time);
            }
            enterbtnFinger.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(0, 1, scanTime / time);

            yield return null;
        }
        for(int i = 0; i < answer.Length; i++)
        {
            fingerPrints[i].GetComponent<CanvasGroup>().alpha = 1;
        }
        enterbtnFinger.GetComponent<CanvasGroup>().alpha = 1;
        heatMapAnim.SetBool("ScanBool", false);
    }
    
}

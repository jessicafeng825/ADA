using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DecoderCombine : PuzzleInfo
{

    [SerializeField]
    private GameObject hintText;

    [SerializeField]
    private GameObject resultImage;

    [SerializeField]
    private GameObject questionImage;

    private GameObject Btn_close;

    

    [SerializeField]
    private GameObject combineButton;
    private Animator combineAnim;
    private bool combining;
    private bool enable;
    private void Start()
    {
        combineAnim = transform.Find("CombineImages").GetComponent<Animator>();
        Btn_close = transform.Find("Btn_close").gameObject;
        Btn_close.GetComponent<Button>().onClick.AddListener(HideThisUI);
        Btn_close.GetComponent<Button>().onClick.AddListener(delegate{ScriptEnableSwitch(false);});
    }
    void Update()
    {
        if(!enable)
        {
            //Check if characcter exists
            if(!CheckCharacter("Street Kid"))
            {
                combineButton.SetActive(true);
            }            
            else if(playerController.Instance.playerJob != "Street Kid")
            {
                combineButton.SetActive(false);
            }
            else
            {
                combineButton.SetActive(true);
            }
            enable = true;
        }
    }
    void OnEnable()
    {
        hintText.GetComponent<TextMeshProUGUI>().text = "A part of some sort of device. It seems to be incomplete. Maybe someone who knows better about these small gadgets can figure it out...";
        enable = false;
    }
    void OnDisable()
    {
        if(isSolved)
        {
            BaseUIManager.Instance.RemovePuzzleBtns("Device Part - 1");
            BaseUIManager.Instance.RemovePuzzleBtns("Device Part - 2");
            BaseUIManager.Instance.RemovePuzzleBtns("Device Part - 3");
            BaseUIManager.Instance.HidePuzzleUI();
        }
    }
    
    public void ScriptEnableSwitch(bool b)
    {
        enable = b;
    }  
    
    public void CombineStart()
    {
        if(!combining)
        {
            StartCoroutine(CombineTimer(3f));
            return;
        }
        
    }
    public int CheckDecoderPartCount()
    {
        int count = 0;
        if(BaseUIManager.Instance.CheckHavePuzzle("Device Part - 1"))
        {
            count++;
        }
        if(BaseUIManager.Instance.CheckHavePuzzle("Device Part - 2"))
        {
            count++;
        }
        if(BaseUIManager.Instance.CheckHavePuzzle("Device Part - 3"))
        {
            count++;
        }
        return count;
    }


    IEnumerator CombineTimer(float time)
    {
        if(CheckDecoderPartCount() < 3)
        {
            hintText.GetComponent<TextMeshProUGUI>().text = "Still missing " + (3 - CheckDecoderPartCount()).ToString() + " parts...";
            yield return new WaitForSeconds(2f);
            hintText.GetComponent<TextMeshProUGUI>().text = "A part of some sort of device. It seems to be incomplete. Maybe someone who knows better about these small gadgets can figure it out...";
            yield break;
        }
        Btn_close.GetComponent<Button>().interactable = false;
        combining = true;
        combineAnim.SetTrigger("CombineTrigger");
        yield return new WaitForSeconds(2f);
        
        combining = false;
        resultImage.SetActive(true);
        questionImage.SetActive(false);
        hintText.GetComponent<TextMeshProUGUI>().text = "Parts Combined! It's a decoder!";
        InvestigationManager.Instance.UpdatePuzzleBtnSolved(puzzleID);
        PuzzleSolveEffect();

        
        yield return new WaitForSeconds(2f);
        BaseUIManager.Instance.RemovePuzzleBtns("Device Part - 1");
        BaseUIManager.Instance.RemovePuzzleBtns("Device Part - 2");
        BaseUIManager.Instance.RemovePuzzleBtns("Device Part - 3");
        BaseUIManager.Instance.HidePuzzleUI();
        Btn_close.GetComponent<Button>().interactable = true;
        
    }
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CyberneticArm : PuzzleInfo
{

    [SerializeField]
    private GameObject hintText;

    [SerializeField]
    private GameObject resultImage;

    [SerializeField]
    private GameObject questionImage;

    

    [SerializeField]
    private GameObject punchButton;

    private float punchTime;
    private bool show;
    private bool enable;
    private void Start()
    {
        
        this.transform.Find("Btn_close").GetComponent<Button>().onClick.AddListener(HideThisUI);
        this.transform.Find("Btn_close").GetComponent<Button>().onClick.AddListener(delegate{ScriptEnableSwitch(false);});
    }
    void Update()
    {
        if(!enable)
        {
            //Check if characcter exists
            if(!CheckCharacter("Biohacker"))
            {
                punchButton.SetActive(true);
            }   
            else if(playerController.Instance.playerJob != "Cybernetic Brawler")
            {
                punchButton.SetActive(false);
            }
            else
            {
                punchButton.SetActive(true);
            }
            enable = true;
        }
    }
    public void ScriptEnableSwitch(bool b)
    {
        enable = b;
    }  
    
    public void ScanStart()
    {
        if(!show)
        {
            show = true;
            StartCoroutine(ScanTimer(3f));
        }
        
    }

    IEnumerator ScanTimer(float time)
    {
        

        while(punchTime <= time)
        {
            punchTime += Time.deltaTime;
            yield return null;
        }
        resultImage.SetActive(true);
        questionImage.SetActive(false);
        hintText.GetComponent<TextMeshProUGUI>().text = "Scan Complete";
        PuzzleSolveEffect();
    }
    
}

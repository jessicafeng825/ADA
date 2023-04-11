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
    private void Start()
    {
        
        this.transform.Find("Btn_close").GetComponent<Button>().onClick.AddListener(HideThisUI);
    }
    void OnEnable()
    {
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
    }
    
    
    public void PunchStart()
    {
        if(!show)
        {
            show = true;
            StartCoroutine(PunchTimer(0.05f));
        }
        
    }

    IEnumerator PunchTimer(float time)
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

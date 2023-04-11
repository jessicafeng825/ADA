using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EmotionScan : PuzzleInfo
{

    [SerializeField]
    private GameObject hintText;

    [SerializeField]
    private GameObject resultImage;

    [SerializeField]
    private GameObject questionImage;

    

    [SerializeField]
    private GameObject scanButton;
    private Animator emotionCanAnim;

    private float scanTime;
    private bool show;
    private void Start()
    {
        emotionCanAnim = GetComponent<Animator>();
        this.transform.Find("Btn_close").GetComponent<Button>().onClick.AddListener(HideThisUI);
    }
    void OnEnable()
    {
        //Check if characcter exists
        if(!CheckCharacter("Nightclub Owner"))
        {
            scanButton.SetActive(true);
        }            
        else if(playerController.Instance.playerJob != "Nightclub Owner")
        {
            scanButton.SetActive(false);
        }
        else
        {
            scanButton.SetActive(true);
        }
    }
    
    public void ScanStart()
    {
        if(!show)
        {
            StartCoroutine(ScanTimer(3f));
        }
        
    }

    IEnumerator ScanTimer(float time)
    {
        emotionCanAnim.SetBool("ScanBool", true);

        while(scanTime <= time)
        {
            scanTime += Time.deltaTime;
            yield return null;
        }
        show = true;
        resultImage.SetActive(true);
        questionImage.SetActive(false);
        hintText.GetComponent<TextMeshProUGUI>().text = "Scan Complete";
        PuzzleSolveEffect();
        emotionCanAnim.SetBool("ScanBool", false);
    }
    
}

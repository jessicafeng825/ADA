using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MemoryReconstruct : PuzzleInfo
{
    [SerializeField]
    private GameObject fingerButton;
    
    [SerializeField]
    private Color32 nromalColor, correctColor, wrongColor;

    
    [SerializeField]
    private GameObject loadingScreen;

    [SerializeField]
    private GameObject descriptionText;

    [SerializeField]
    private GameObject resultImage;

    [SerializeField]
    private List<string> neededJob;

    private string enteredNum = "";

    private bool check;

    private bool buttonPressed;
    private float pressingTime;

    private void Start()
    {
        this.transform.Find("Btn_close").GetComponent<Button>().onClick.AddListener(HideThisUI);
    }
 
    public void btnDown(float time)
    {
        if(isSolved)
            return;
        buttonPressed = true;
        StartCoroutine(PressTimer(6f));
    }
    
    public void btnUp()
    {
        Debug.Log("up");
        if(isSolved)
            return;
        descriptionText.SetActive(true);
        loadingScreen.transform.GetChild(0).Find("LoadingLabel").GetComponent<TextMeshProUGUI>().text = "Verifying...";
        loadingScreen.transform.GetChild(0).Find("LoadingBar").gameObject.SetActive(true);
        loadingScreen.SetActive(false);
        check = false;
        buttonPressed = false;
        pressingTime = 0;
    }
    

    
    public bool CheckAnswer()
    {
        check = true;
        bool hasCharacter = false;
        foreach(string s in neededJob)
        {
            if(CheckCharacter(s))
                hasCharacter = true;
            else
                continue;
        }
        if(!hasCharacter)
        {
            loadingScreen.transform.GetChild(0).Find("LoadingLabel").GetComponent<TextMeshProUGUI>().text = "Success";
            loadingScreen.transform.GetChild(0).Find("LoadingBar").gameObject.SetActive(false);
            resultImage.SetActive(true);
            descriptionText.SetActive(false);
            fingerButton.GetComponent<Image>().color = correctColor;
            fingerButton.GetComponent<Button>().enabled = false;
            isSolved = true;
            PuzzleSolveEffect();
            return true;
        }
        else if(neededJob.Contains(playerController.Instance.playerJob))
        {
            loadingScreen.transform.GetChild(0).Find("LoadingLabel").GetComponent<TextMeshProUGUI>().text = "Success";
            loadingScreen.transform.GetChild(0).Find("LoadingBar").gameObject.SetActive(false);
            resultImage.SetActive(true);
            descriptionText.SetActive(false);
            fingerButton.GetComponent<Image>().color = correctColor;
            fingerButton.GetComponent<Button>().enabled = false;
            isSolved = true;
            PuzzleSolveEffect();
            return true;
        }
        else
        {
            StartCoroutine(WrongResult());
            return false;
        }
    }
    IEnumerator PressTimer(float time)
    {
        while(buttonPressed && !check)
        {
            pressingTime += Time.deltaTime;
            if(pressingTime >= 0 && pressingTime < 1)
            {
                descriptionText.SetActive(false);
                loadingScreen.SetActive(true);
                loadingScreen.transform.GetChild(0).Find("LoadingLabel").GetComponent<TextMeshProUGUI>().text = "Verifying...";
            }
            else if(pressingTime >= 1 && pressingTime < 3)
            {
                loadingScreen.transform.GetChild(0).Find("LoadingLabel").GetComponent<TextMeshProUGUI>().text = "Extracting Memory...";
            }
            else if(pressingTime >= 3 && pressingTime < time)
            {
                loadingScreen.transform.GetChild(0).Find("LoadingLabel").GetComponent<TextMeshProUGUI>().text = "Reconstructing...";
            }
            else if(pressingTime >= time)
            {
                CheckAnswer();
            }
            yield return null;
        }
    }
    IEnumerator WrongResult()
    {
        loadingScreen.transform.GetChild(0).Find("LoadingLabel").GetComponent<TextMeshProUGUI>().text = "Failed";
        loadingScreen.transform.GetChild(0).Find("LoadingBar").gameObject.SetActive(false);
        fingerButton.GetComponent<Image>().color = wrongColor;
        yield return new WaitForSeconds(1f);
        
        descriptionText.SetActive(true);
        loadingScreen.transform.GetChild(0).Find("LoadingLabel").GetComponent<TextMeshProUGUI>().text = "Loading...";
        loadingScreen.transform.GetChild(0).Find("LoadingBar").gameObject.SetActive(true);
        loadingScreen.SetActive(false);
        fingerButton.GetComponent<Image>().color = nromalColor;
    }
    
}

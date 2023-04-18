using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private GameObject[]pages;
    [SerializeField] private GameObject[] page00Steps;
    [SerializeField] private GameObject[] page01Steps;
    [SerializeField] private GameObject[] page02Steps;
    [SerializeField] private GameObject[] page03Steps;
    [SerializeField] private GameObject[] page04Steps;
    [SerializeField] private GameObject[] page05Steps;
    [SerializeField] private GameObject[] page06Steps;
    [SerializeField] private GameObject[] currentSteps = new GameObject[10];

    [SerializeField] private GameObject LeftButton;
    [SerializeField] private GameObject RightButton;
    [SerializeField] private int currentPage = 0;    
    [SerializeField] private int currentStep = 0;

    private void Start()
    {       
        OpenTutorial();
    }
    private void OnEnable() 
    {
        LeftButton.SetActive(false);
        currentSteps = page00Steps;
    }
    public void NextStep()
    {
        LeftButton.SetActive(true);
        GetCurrentSteps();
        if(currentPage == pages.Length - 1)
        {
            if(currentStep == currentSteps.Length - 1)
            {
                RightButton.SetActive(false);
            }
        }
        else
        {
            RightButton.SetActive(true);
        }
        if(currentStep == currentSteps.Length)
        {
            pages[currentPage].SetActive(false);
            pages[currentPage + 1].SetActive(true);
            currentPage++;
            GetCurrentSteps();
            currentStep = 0;
            return;
        }
        foreach(GameObject step in currentSteps)
        {
            step.SetActive(false);
        }
        currentSteps[currentStep].SetActive(true);
        currentStep++;
    }

    public void PreviousStep()
    {
        RightButton.SetActive(true);
        GetCurrentSteps();
        if(currentPage == 0)
        {
            if(currentStep == 1)
            {
                LeftButton.SetActive(false);
            }
        }
        else
        {
            LeftButton.SetActive(true);
        }
        if(currentStep == 0 && currentPage != 0)
        {
            pages[currentPage].SetActive(false);
            pages[currentPage - 1].SetActive(true);
            currentPage--;
            GetCurrentSteps();
            currentStep = currentSteps.Length;
            if(currentPage == 0)
            {
                if(currentStep == 0)
                {
                    LeftButton.SetActive(false);
                }
            }
            return;
        }
        foreach(GameObject step in currentSteps)
        {
            step.SetActive(false);
        }
        currentStep--;
        if(currentStep > 0)
            currentSteps[currentStep - 1].SetActive(true);
        
    }

    public void GetCurrentSteps()
    {
        switch(currentPage)
        {
            case 0:
                currentSteps = page00Steps;
                break;
            case 1:
                currentSteps = page01Steps;
                break;
            case 2: 
                currentSteps = page02Steps;
                break;
            case 3: 
                currentSteps = page03Steps;
                break;
            case 4:
                currentSteps = page04Steps;
                break;
            case 5:
                currentSteps = page05Steps;
                break;
            case 6:
                currentSteps = page06Steps;
                break;
        }
    }
    

    

    public void CloseTutorial()
    {
        foreach(GameObject step in page00Steps)
        {
            step.SetActive(false);
        }
        foreach(GameObject step in page01Steps)
        {
            step.SetActive(false);
        }
        foreach(GameObject step in page02Steps)
        {
            step.SetActive(false);
        }
        foreach(GameObject step in page03Steps)
        {
            step.SetActive(false);
        }
        foreach(GameObject step in page04Steps)
        {
            step.SetActive(false);
        }
        foreach(GameObject step in page05Steps)
        {
            step.SetActive(false);
        }
        foreach(GameObject step in page06Steps)
        {
            step.SetActive(false);
        }
        foreach(GameObject page in pages)
        {
            page.SetActive(false);
        }
        pages[0].SetActive(true);
        currentPage = 0;
        currentSteps = page00Steps;
        currentStep = 0;
        gameObject.SetActive(false);
        LeftButton.SetActive(false);
        RightButton.SetActive(true);
    }

    public void OpenTutorial()
    {
        gameObject.SetActive(true);
        Debug.Log("Tutorial Opened");
    }
}

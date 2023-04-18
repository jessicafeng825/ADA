using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private GameObject[] tutorialSteps;
    [SerializeField] private GameObject Page01LeftButton;
    [SerializeField] private GameObject Page06RightButton;
    
    private int currentStep = 0;

    private void Start()
    {
        tutorialSteps[currentStep].SetActive(true);
        Page01LeftButton.SetActive(false);
    }

    public void NextStep()
    {
        
        currentStep++;
        tutorialSteps[currentStep].SetActive(true);
        if(currentStep > 0)
        {
            Page01LeftButton.SetActive(true);
        }
        if(currentStep > tutorialSteps.Length - 2)
        {
            Page06RightButton.SetActive(false);
        }
    }

    public void PreviousStep()
    {
        
        tutorialSteps[currentStep].SetActive(false);
        currentStep--;
        if(currentStep < 1)
        {
            Page01LeftButton.SetActive(false);
        }
        if(currentStep < tutorialSteps.Length - 1)
        {
            Page06RightButton.SetActive(true);
        }
    }

    public void CloseTutorial()
    {
        foreach(GameObject step in tutorialSteps)
        {
            step.SetActive(false);
        }
        tutorialSteps[0].SetActive(true);
        currentStep = 0;
        gameObject.SetActive(false);
        Page01LeftButton.SetActive(false);
        Page06RightButton.SetActive(true);
    }

    public void OpenTutorial()
    {
        gameObject.SetActive(true);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HiddenButton : PuzzleInfo
{
    [SerializeField]
    private GameObject hintText;

    private void Start()
    {        
        this.transform.Find("Btn_close").GetComponent<Button>().onClick.AddListener(HideThisUI);
    }
   
    
    
    public void ButtonPressed()
    {
        if(!isSolved)
        {
            hintText.GetComponent<TextMeshProUGUI>().text = "Buzzing sounds can be heard from the cameras in the room.";
            InvestigationManager.Instance.UpdatePuzzleBtnSolved(puzzleID);
            PuzzleSolveEffect();
        }
        
    }

    
}

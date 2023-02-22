using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class BaseUIManager : Singleton<BaseUIManager>
{
    // use this for a while, till Hui finish the MenuManager part
    [SerializeField]
    private GameObject clueInfoNoPicPanel, clueInfoPicPanel;

    [SerializeField]
    private TMP_Text clueNameText, clueNamePicText;
        
    [SerializeField]
    private TMP_Text clueDescripText, clueDescripPicText;

    [SerializeField]
    private Image cluePicHolder;
    private Dictionary<string, Sprite> cluePicsDic = new Dictionary<string, Sprite>();

    [SerializeField]
    private GameObject puzzleInfoMenu;
    private Dictionary<string, GameObject> puzzleInteractionDic = new Dictionary<string, GameObject>();
    private GameObject tempPuzzle;

    private void Start()
    {
        // load all pictures at start
        LoadAllPics();
        LoadAllPuzzleInteractions();
    }

    // function: load all clue picture resources and add to dictionary for use
    private void LoadAllPics()
    {
        foreach(Sprite cluePic in Resources.LoadAll<Sprite>("CluesRelated/CluePics/"))
        {
            cluePicsDic.Add(cluePic.name, cluePic);
        }
    }

    private void LoadAllPuzzleInteractions()
    {
        foreach(GameObject puzzleInteraction in Resources.LoadAll("PuzzlesRelated/PuzzleInteractions/"))
        {
            puzzleInteractionDic.Add(puzzleInteraction.name, puzzleInteraction);
        }
    }

    #region Clue UI Related Functions
    // function: show clue information panel with no picture 
    public void ShowClueInfoNoPicture(string clueName, string clueDescrip)
    {
        clueInfoNoPicPanel.SetActive(true);
        clueNameText.text = clueName;
        clueDescripText.text = clueDescrip;
    }

    // function: show clue information panel with picture 
    public void ShowClueInfoWithPicture(string clueName, string clueDescrip)
    {
        clueInfoPicPanel.SetActive(true);
        clueNamePicText.text = clueName;
        clueDescripPicText.text = clueDescrip;
        cluePicHolder.sprite = cluePicsDic[clueName];
    }

    // function: hide clue information panel with no picture 
    public void HideCLueInfoNoPicture()
    {
        clueInfoNoPicPanel.SetActive(false);
    }

    // function: hide clue information panel with picture 
    public void HideClueInfoWithPicture()
    {
        clueInfoPicPanel.SetActive(false);
    }
    #endregion

    public void ShowPuzzleUI(string puzzleName)
    {
        tempPuzzle = (GameObject)Instantiate(puzzleInteractionDic[puzzleName]);
        //tempPuzzle.transform.position = Camera.main.WorldToScreenPoint(0f,0f,0f);
        tempPuzzle.GetComponent<Transform>().SetParent(puzzleInfoMenu.GetComponent<Transform>(), false);
        puzzleInfoMenu.SetActive(true);
    }
}

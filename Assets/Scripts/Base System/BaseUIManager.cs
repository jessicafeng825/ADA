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


    private void Start()
    {
        // load all pictures at start
        LoadAllPics();
    }

    // function: load all clue picture resources and add to dictionary for use
    private void LoadAllPics()
    {
        foreach(Sprite cluePic in Resources.LoadAll<Sprite>("CluePics/"))
        {
            cluePicsDic.Add(cluePic.name, cluePic);
        }
    }

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
}

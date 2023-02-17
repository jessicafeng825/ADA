using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class BaseUIManager : Singleton<BaseUIManager>
{
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
        LoadAllPics();
    }

    private void LoadAllPics()
    {
        foreach(Sprite cluePic in Resources.LoadAll<Sprite>("CluePics/"))
        {
            cluePicsDic.Add(cluePic.name, cluePic);
        }
    }

    public void ShowClueInfoNoPicture(string clueName, string clueDescrip)
    {
        clueInfoNoPicPanel.SetActive(true);
        clueNameText.text = clueName;
        clueDescripText.text = clueDescrip;
    }

    public void ShowClueInfoWithPicture(string clueName, string clueDescrip)
    {
        clueInfoPicPanel.SetActive(true);
        clueNamePicText.text = clueName;
        clueDescripPicText.text = clueDescrip;
        cluePicHolder.sprite = cluePicsDic[clueName];
    }

    public void HideCLueInfoNoPicture()
    {
        clueInfoNoPicPanel.SetActive(false);
    }

    public void HideClueInfoWithPicture()
    {
        clueInfoPicPanel.SetActive(false);
    }
}

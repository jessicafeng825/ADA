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
    private Image cluePic;

    private void Start()
    {
        
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
        cluePic = Resources.Load<Image>("CluePics/" + clueName);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ClueInfo : MonoBehaviour
{
    // Clue Information that would be shown in UI
    [SerializeField]
    private int clueID;
    [SerializeField]
    private string clueName, clueDescrip;
    [SerializeField]
    private bool hasPicture;

    private void Start()
    {
        this.GetComponent<Button>().onClick.AddListener(OpenClue);
    }

    private void OpenClue()
    {
        if (hasPicture)
        {
            BaseUIManager.Instance.ShowClueInfoWithPicture(clueID, clueName, clueDescrip);
        }
        else
            BaseUIManager.Instance.ShowClueInfoNoPicture(clueName, clueDescrip);
    }
}

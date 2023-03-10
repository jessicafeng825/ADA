using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ClueInfo : MonoBehaviour
{
    // Clue Information that would be shown in UI
    [SerializeField]
    private string clueName, clueTitle, clueDescrip;
    [SerializeField]
    private bool hasPicture;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(OpenClue);
    }

    private void OpenClue()
    {
        if (hasPicture)
        {
            BaseUIManager.Instance.ShowClueInfoWithPicture(clueName, clueTitle, clueDescrip);
        }
        else
            BaseUIManager.Instance.ShowClueInfoNoPicture(clueName, clueTitle, clueDescrip);
    }
}

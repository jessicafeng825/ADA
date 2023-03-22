using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ClueInfo : MonoBehaviour
{
    [SerializeField]
    private string clueID;
    [SerializeField]
    private Button closeBtn, shareBtn;
    private bool isShared;

    private void Start()
    {
        closeBtn.onClick.AddListener(HideThisClueInfo);
        shareBtn.onClick.AddListener(ShareThisClue);
    }

    public string GetClueID()
    {
        return clueID;
    }
    private void HideThisClueInfo()
    {
        gameObject.SetActive(false);
        BaseUIManager.Instance.HideClueUI();
    }

    private void ShareThisClue()
    {
        if (!isShared)
        {
            DetectiveBoardManager.Instance.ShareClue(clueID);
            isShared = true;
        } 
    }
}

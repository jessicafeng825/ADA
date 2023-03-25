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
        closeBtn = this.transform.Find("Btn_close").GetComponent<Button>();
        shareBtn = this.transform.Find("Btn_share").GetComponent<Button>();
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
        if (!isShared && playerController.Instance.currentClueSharedNum < 3)
        {
            playerController.Instance.currentClueSharedNum ++;
            DetectiveBoardManager.Instance.ShareClue(clueID);
            isShared = true;
            Debug.Log("share");
        }
        else
        {
            //TODO: UI "you have shared this clue"
        }
    }
}

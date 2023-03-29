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
        NotificationScript tempNoti = BaseUIManager.Instance.SpawnNotificationPanel("Sharing Clue", "Are you sure you want to share this clue?", 2, -1f);
        tempNoti.AddFunctiontoYesButton(() => ExecuteClueShare(), true);
    }

    private void ExecuteClueShare()
    {
        if (isShared)
        {
            BaseUIManager.Instance.SpawnNotificationPanel("Clue Shared", "You have already shared this clue", 1, -1f);
        }
        else if (playerController.Instance.currentClueSharedNum >= DetectiveBoardManager.Instance.GetClueShareLimit())
        {
            BaseUIManager.Instance.SpawnNotificationPanel("Exceed Share Limit", "You have shared " + playerController.Instance.currentClueSharedNum + " clues for this round!", 1, -1f);
        }
        else
        {
            playerController.Instance.currentClueSharedNum++;
            DetectiveBoardManager.Instance.ShareClue(clueID);
            isShared = true;
            BaseUIManager.Instance.SpawnNotificationPanel("Share Completed", "The clue is shared to the detective board", 1, -1f);
        }
    }
}

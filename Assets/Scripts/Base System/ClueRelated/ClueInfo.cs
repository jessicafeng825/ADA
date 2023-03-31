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
        if (isShared)
        {
            BaseUIManager.Instance.SpawnNotificationPanel("Clue Already Shared", "You have already shared this clue", 1, -1f);
        }
        else
        {
            NotificationScript tempNoti = BaseUIManager.Instance.SpawnNotificationPanel("Sharing Clue", "You have shared: <b>" + playerController.Instance.currentClueSharedNum + "/" + DetectiveBoardManager.Instance.GetClueShareLimit() + "</b> clues for this round!", 2, -1f);
            tempNoti.AddFunctiontoYesButton(() => ExecuteClueShare(), true);
        }
    }

    private void ExecuteClueShare()
    {
        if (playerController.Instance.currentClueSharedNum >= DetectiveBoardManager.Instance.GetClueShareLimit())
        {
            BaseUIManager.Instance.SpawnNotificationPanel("Exceed Share Limit", "You can't share more than <b>" + DetectiveBoardManager.Instance.GetClueShareLimit() + "</b> clues for this round!", 1, -1f);
        }
        else
        {
            playerController.Instance.currentClueSharedNum++;
            DetectiveBoardManager.Instance.ShareClue(clueID);
            isShared = true;
            BaseUIManager.Instance.SpawnNotificationPanel("Sharing Completed", "The clue is shared to the detective board", 1, -1f);
        }
    }
}

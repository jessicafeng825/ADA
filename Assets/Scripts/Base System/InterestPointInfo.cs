using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine;

public class InterestPointInfo : MonoBehaviour
{
    
    [SerializeField]
    public Memory memory;

    [SerializeField]
    private List<InterestPoint> collectableList;
    [SerializeField]
    private int cnt_current = 0;

    [System.Serializable]
    public class InterestPoint
    {
        public string id;
        public ipType type;
    }

    public enum ipType
    {
        Clue, Puzzle, Item
    }

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(SpawnPopup);
    }
    public void SpawnPopup()
    {
        NotificationScript tempNoti = BaseUIManager.Instance.SpawnNotificationPanel("Use Action Points?", "Use the action point for investigate the interest point?", 2, -1f);
        tempNoti.AddFunctiontoYesButton(AddCollectable, false);
    }

    private void AddCollectable()
    {
        if (cnt_current < collectableList.Count)
        {
            if (collectableList[cnt_current].type == ipType.Clue)
            {
                BaseUIManager.Instance.SpawnNotificationPanel("Clue Found!", "You have found a clue!", 1, 3f);
                InvestigationManager.Instance.AddCluePrefab(collectableList[cnt_current].id, memory);
            }
            else if (collectableList[cnt_current].type == ipType.Puzzle)
            {
                BaseUIManager.Instance.SpawnNotificationPanel("Puzzle Found!", "You have found a puzzle!", 1, 3f);
                InvestigationManager.Instance.AddPuzzlePrefab(collectableList[cnt_current].id);
            }
                

            // If the current clue is the last one, inactivate the interest point
            if (cnt_current == collectableList.Count - 1)
            {
                InvestigationManager.Instance.SynchronizeInterestPointStatus(name, memory);
            }

            // Tell im to synchronize
            InvestigationManager.Instance.SynchronizeInterestPoint(name);
        }
    }

    public void changeIP_Current(int change)
    {
        cnt_current += change;
    }
}

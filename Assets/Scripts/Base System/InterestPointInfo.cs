using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine;

public class InterestPointInfo : MonoBehaviour
{
    [SerializeField]
    private List<InterestPoint> collectableList;
    [SerializeField]
    private int cnt_current = 0;

    [System.Serializable]
    public class InterestPoint
    {
        public string name;
        public bool isClue;
    }

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(SpawnPopup);
    }
    public void SpawnPopup()
    {
        BaseUIManager.Instance.SpawnNotificationPanel("Use Action Points?", "Use the action point for investigate the interest point?", 2, -1f);
        NotificationScript.yesButtonEvent.AddListener(AddCollectable);
    }

    private void AddCollectable()
    {
        if (cnt_current < collectableList.Count)
        {
            if (collectableList[cnt_current].isClue)
            {
                BaseUIManager.Instance.SpawnNotificationPanel("Clue Found!", "You have found a clue!", 1, 3f);
                InvestigationManager.Instance.AddCluePrefab(collectableList[cnt_current].name);
            }
            else
            {
                BaseUIManager.Instance.SpawnNotificationPanel("Puzzle Found!", "You have found a puzzle!", 1, 3f);
                InvestigationManager.Instance.AddPuzzlePrefab(collectableList[cnt_current].name);
            }
                

            // If the current clue is the last one, inactivate the interest point
            if (cnt_current == collectableList.Count - 1)
            {
                Debug.Log("disappear!");
                InvestigationManager.Instance.SynchronizeInterestPointStatus(name);
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

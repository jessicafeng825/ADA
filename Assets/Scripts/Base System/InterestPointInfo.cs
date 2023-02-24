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
        GetComponent<Button>().onClick.AddListener(AddCollectable);
    }

    private void AddCollectable()
    {
        if (cnt_current < collectableList.Count)
        {
            if (collectableList[cnt_current].isClue)
                InvestigationManager.Instance.AddCluePrefab(collectableList[cnt_current].name);
            else
                InvestigationManager.Instance.AddPuzzlePrefab(collectableList[cnt_current].name);

            // If the current clue is the last one, inactivate the interest point
            if (cnt_current == collectableList.Count - 1)
            {
                Debug.Log("disappear!");
                InactiveIPAllCollected();
            }

            // Tell im to synchronize
            InvestigationManager.Instance.SynchroniSeInterestPoint(name);
        }
    }

    public void changeIP_Current(int change)
    {
        cnt_current += change;
    }

    private void InactiveIPAllCollected()
    {
        gameObject.SetActive(false);
    }
}

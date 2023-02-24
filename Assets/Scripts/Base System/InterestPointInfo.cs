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

            // Tell im to synchronize
            InvestigationManager.Instance.SynchronizeInterestPoint(name);

        }
        else
        {
            Debug.Log("No other collectable");
            // show alert message
        }
    }

    public void changeIP_Current(int change)
    {
        cnt_current += change;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class InterestPointInfo : MonoBehaviour
{
    [SerializeField]
    private List<string> clueList;
    private int cnt_currentClue = 0;

    private void Start()
    {
        this.GetComponent<Button>().onClick.AddListener(AddClue);
    }

    private void AddClue()
    {
        if (cnt_currentClue < clueList.Count)
        {
            InvestigationManager.Instance.AddCluePrefab(clueList[cnt_currentClue]);
            cnt_currentClue++;
        }
        else
            Debug.Log("there's no clues");
    }
}

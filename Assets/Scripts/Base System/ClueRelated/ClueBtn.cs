using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ClueBtn : MonoBehaviour
{
    // Clue Information that would be shown in UI
    [SerializeField]
    private string clueID;
    [SerializeField]
    private GameObject newClueMark, clueSharedMark;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(OpenClue);
        newClueMark.SetActive(true);
    }

    private void OpenClue()
    {
        BaseUIManager.Instance.ShowClueUI(clueID);
        newClueMark.gameObject.SetActive(false);
    }

    public string GetClueID()
    {
        return clueID;
    }

    public void SetClueID(string givenClueID)
    {
        clueID = givenClueID;
    }

    public void SetSharedMark()
    {
        clueSharedMark.SetActive(true);
    }
}

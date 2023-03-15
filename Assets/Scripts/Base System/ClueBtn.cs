using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ClueBtn : MonoBehaviour
{
    // Clue Information that would be shown in UI
    [SerializeField]
    private string clueID;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(OpenClue);
    }

    private void OpenClue()
    {
        BaseUIManager.Instance.ShowClueUI(clueID);
    }

    public string GetClueID()
    {
        return clueID;
    }
}

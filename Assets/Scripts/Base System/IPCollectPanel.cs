using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class IPCollectPanel : MonoBehaviour
{
    private UnityEvent[] choiceEvent =  new UnityEvent[5];
    private Button[] choiceButtons = new Button[5];
    static public IPCollectPanel Instance;
    private void Awake()
    {
        choiceButtons = this.transform.Find("Choices").GetComponentsInChildren<Button>();
        for(int i = 0; i < choiceEvent.Length; i++)
        {
            choiceEvent[i] = new UnityEvent();
        }
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(Instance.gameObject);
            Instance = this;
            ResetButtons();
        }
        
    }
    public void ChoiceButton(int i)
    {
        choiceEvent[i].Invoke();
        this.gameObject.SetActive(false);
    }
    public void AddFunctiontoChoiceButton(UnityAction function, string title, int i)
    {
        choiceButtons[i].transform.Find("Text (TMP)").GetComponent<TMP_Text>().text = title;
        if(choiceEvent[i] != null)
            choiceEvent[i].RemoveAllListeners();
        choiceEvent[i].AddListener(function);
    }

    public void ClosePanel()
    {
        ResetButtons();
        Destroy(this.gameObject);
    }


    public void ResetButtons()
    {
       for(int i = 0; i < choiceEvent.Length; i++)
       {
            choiceEvent[i].RemoveAllListeners();
       }
    }

}

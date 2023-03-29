using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NotificationScript : MonoBehaviour
{
    private UnityEvent yesButtonEvent = new UnityEvent();
    private UnityEvent noButtonEvent = new UnityEvent();
    static public NotificationScript Instance;
    public float despawnTime = 10f;

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(Instance.gameObject);
            Instance = this;
        }
        StartCoroutine(Despawn());
    }
    public void ButtonYes()
    {
        GetComponent<Animator>().SetTrigger("isDisappearing");
        yesButtonEvent.Invoke();
        ResetButtons();
        Destroy(gameObject);
    }
    public void ButtonNo()
    {
        GetComponent<Animator>().SetTrigger("isDisappearing");
        noButtonEvent.Invoke();
        ResetButtons();
        Destroy(gameObject);
    }
    public void AddFunctiontoYesButton(UnityAction function, bool reset)
    {
        if(reset)
        {
            yesButtonEvent.RemoveAllListeners();
        }
        yesButtonEvent.AddListener(function);
    }
    public void AddFunctiontoNoButton(UnityAction function)
    {
        noButtonEvent.RemoveAllListeners();
        noButtonEvent.AddListener(function);
    }
    public void ResetButtons()
    {
        yesButtonEvent.RemoveAllListeners();
        noButtonEvent.RemoveAllListeners();
    }

    private IEnumerator Despawn()
    {
        GetComponent<Animator>().SetTrigger("isAppearing");
        if (despawnTime != -1)
        {
            yield return new WaitForSeconds(despawnTime);
            GetComponent<Animator>().SetTrigger("isDisappearing");
            yield return new WaitForSeconds(1f);
            Destroy(gameObject);
        }
        else
        {
            yield break;
        }
    
    }
}

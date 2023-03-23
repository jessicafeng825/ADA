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
        if(Instance == null)
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
        yesButtonEvent.Invoke();
        ResetButtons();
        Destroy(this.gameObject);
    }
    public void ButtonNo()
    {
        noButtonEvent.Invoke();
        ResetButtons();
        Destroy(this.gameObject);
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

    IEnumerator Despawn()
    {
        float timer = 0;
        
        while(timer < despawnTime)
        {
            if(despawnTime == -1)
            {
                yield break;
            }
            timer += Time.deltaTime;
            yield return null;
        }
        if(despawnTime != -1)
        {
            Destroy(this.gameObject);
        }
    }
}

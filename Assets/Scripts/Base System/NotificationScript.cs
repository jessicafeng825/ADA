using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NotificationScript : MonoBehaviour
{
    static public UnityEvent yesButtonEvent = new UnityEvent();
    static public UnityEvent noButtonEvent = new UnityEvent();
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
            Debug.Log("Destroy old");
            Destroy(Instance.gameObject);
            Instance = this;
        }
        StartCoroutine(Despawn());
        
    }
    public void ButtonYes()
    {
        yesButtonEvent.Invoke();
        yesButtonEvent.RemoveAllListeners();
        Destroy(this.gameObject);
    }
    public void ButtonNo()
    {
        noButtonEvent.Invoke();
        noButtonEvent.RemoveAllListeners();
        Destroy(this.gameObject);
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
            Debug.Log("Despawn");
            Destroy(this.gameObject);
        }
    }
}

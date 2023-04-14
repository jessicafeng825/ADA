using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleMenu : MonoBehaviour
{
  
    private Animator titleAnimator;

    [SerializeField]
    private GameObject titleHostDescription;

    [SerializeField]
    private GameObject titlePlayerDescription;
    // Start is called before the first frame update
    void Start()
    {
        titleAnimator = GetComponent<Animator>();
    }
    private void OnDisable()
    {
        titleHostDescription.SetActive(false);
        titlePlayerDescription.SetActive(false);
    }
    
    public void OpenHostDescript()
    {
        titlePlayerDescription.SetActive(false);
        titleHostDescription.SetActive(true);
        titleAnimator.SetTrigger("HostDescript");
    }

    public void OpenPlayerDescript()
    {
        titleHostDescription.SetActive(false);
        titlePlayerDescription.SetActive(true);
        titleAnimator.SetTrigger("PlayerDescript");
    }
}

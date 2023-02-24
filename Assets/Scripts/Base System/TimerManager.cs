using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class TimerManager : MonoBehaviour
{
    public static TimerManager Instance;
    
    [SerializeField]
    private GameObject timerPanel;
    
    [SerializeField]
    private float investigateTime;
    [SerializeField]
    private float discussTime;
    private float currentStageTimer;
    private float gamePhaseTimer;
    private PlayerManagerForAll.gamestage publicStageNow;
    private bool timeout;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        publicStageNow = PlayerManagerForAll.gamestage.Investigate;
        gamePhaseTimer = investigateTime;
    }

    private void Update() 
    {        
        if(gamePhaseTimer <= 0 && !timeout)
        {
            StartCoroutine(TimerPause(2));
        }
        else if(gamePhaseTimer > 0 && !timeout)
        {
            timerPanel.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = (gamePhaseTimer/60).ToString("00") + ":" + Mathf.Floor(gamePhaseTimer%60).ToString("00");
            gamePhaseTimer -= Time.deltaTime;
        }
    }
    private void PublicStageChange()
    {
        switch(publicStageNow)
        {
            case PlayerManagerForAll.gamestage.Investigate:
                //playerController.Instance.investtoDicuss();
                timerPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Discussion";
                publicStageNow = PlayerManagerForAll.gamestage.Dissussion;
                currentStageTimer = discussTime;
                break;
            case PlayerManagerForAll.gamestage.Dissussion:
                //playerController.Instance.discusstoInvest();
                timerPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Investigation";
                publicStageNow = PlayerManagerForAll.gamestage.Investigate;
                currentStageTimer = investigateTime;
                break;
        }
        
        gamePhaseTimer = currentStageTimer;
    }
    public void SkipStageButton()
    {
        timeout = true;
        timerPanel.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "00:00";
        StartCoroutine(TimerPause(2));
    }
    IEnumerator TimerPause(float sec)
    {
        bool activeButton = false;
        timeout = true;
        switch(publicStageNow)
        {
            case PlayerManagerForAll.gamestage.Investigate:
                timerPanel.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Investigation Ended";
                activeButton = true;
                timerPanel.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = "End Discussion";
                break;
            case PlayerManagerForAll.gamestage.Dissussion:
                timerPanel.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Discussion Ended";
                activeButton = false;
                timerPanel.transform.GetChild(3).gameObject.SetActive(false);
                timerPanel.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = "End Investigation";
                break;
            default:
                activeButton = false;
                break;
        }
        timerPanel.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "00:00";
        timerPanel.transform.GetChild(3).gameObject.SetActive(false);
        yield return new WaitForSeconds(sec);
        timerPanel.transform.GetChild(3).gameObject.SetActive(activeButton);
        timerPanel.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
        PublicStageChange();
        timeout = false;
    }

}

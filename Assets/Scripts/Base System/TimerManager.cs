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
    private GameObject playerTimerPanel;
    
    [SerializeField]
    private float investigateTime;
    [SerializeField]
    private float discussTime;
    private float currentStageTimer;
    private float gamePhaseTimer;
    private PlayerManagerForAll.gamestage publicStageNow;
    private bool timeout;
    private PhotonView pv;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        pv = GetComponent<PhotonView>();
        publicStageNow = PlayerManagerForAll.gamestage.Investigate;
        gamePhaseTimer = investigateTime;
    }
    

    private void Update()
    { 
        if(!PhotonNetwork.IsMasterClient)
        {
            return;
        }
        if(gamePhaseTimer <= 0 && !timeout)
        {
            SwitchStage();
        }
        else if(gamePhaseTimer > 0 && !timeout)
        {
            string time = (gamePhaseTimer/60).ToString("00") + ":" + Mathf.Floor(gamePhaseTimer%60).ToString("00");
            timerPanel.transform.GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>().text = time;
            gamePhaseTimer -= Time.deltaTime;
            pv.RPC(nameof(SyncTimer), RpcTarget.Others, publicStageNow.ToString(), time);
            
        }
    }
    private void PublicStageChange()
    {
        switch(publicStageNow)
        {
            case PlayerManagerForAll.gamestage.Investigate:
                pv.RPC(nameof(InvestigationManagerSwitch), RpcTarget.All, false);
                timerPanel.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = "Discussion";
                publicStageNow = PlayerManagerForAll.gamestage.Dissussion;
                currentStageTimer = discussTime;
                break;
            case PlayerManagerForAll.gamestage.Dissussion:
                pv.RPC(nameof(InvestigationManagerSwitch), RpcTarget.All, true);
                timerPanel.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = "Investigation";
                publicStageNow = PlayerManagerForAll.gamestage.Investigate;
                currentStageTimer = investigateTime;
                break;
        }
        
        gamePhaseTimer = currentStageTimer;
    }
    public void SkipStageButton()
    {
        timeout = true;
        timerPanel.transform.GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>().text = "00:00";
        SwitchStage();
    }
    public void SwitchStage()
    {
        StartCoroutine(TimerPauseCoroutine(2));
    }
    [PunRPC]
    public void InvestigationManagerSwitch(bool active)
    {
        InvestigationManager.Instance.gameObject.SetActive(active);
    }
    [PunRPC]
    public void SyncTimer(string stage, string time)
    {
        playerTimerPanel.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = stage;
        playerTimerPanel.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = time;
    }
    [PunRPC]
    public void TimeupDialog(string title, string description)
    {
        Debug.Log("Time up Dialog");
        BaseUIManager.Instance.SpawnNotificationPanel(title, description, 1, 3f);
    }
    IEnumerator TimerPauseCoroutine(float sec)
    {
        bool activeButton = false;
        timeout = true;
        switch(publicStageNow)
        {
            case PlayerManagerForAll.gamestage.Investigate:
                pv.RPC(nameof(TimeupDialog), RpcTarget.All, "Time is up!", "Investigation has ended");
                activeButton = true;
                timerPanel.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "End Discussion";
                break;
            case PlayerManagerForAll.gamestage.Dissussion:
                pv.RPC(nameof(TimeupDialog), RpcTarget.All, "Time is up!", "Discussion has ended");
                activeButton = false;
                break;
            default:
                activeButton = false;
                break;
        }
        timerPanel.transform.GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>().text = "00:00";
        timerPanel.transform.GetChild(1).gameObject.SetActive(false);
        yield return new WaitForSeconds(sec);
        timerPanel.transform.GetChild(1).gameObject.SetActive(activeButton);
        PublicStageChange();
        timeout = false;
    }

}

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
    private GameObject PCMapPanel;
    [SerializeField]
    private GameObject DetectiveBoardPanel;
    
    [SerializeField]
    private GameObject timerPanel;
    private GameObject TimerTitle;
    private GameObject EndButton;
    
    [SerializeField]
    private GameObject playerTimerPanel;
    
    [SerializeField]
    private GameObject playerPanel;
    private GameObject investigationPanel;
    private GameObject discussionPanel;
    
    [SerializeField]
    private GameObject transtitionPanel;
    
    [SerializeField]
    private float investigateTime;
    [SerializeField]
    private float discussTime;
    private float currentStageTimer;
    private float gamePhaseTimer;
    private PlayerManagerForAll.gamestage publicStageNow;
    private bool timeout;
    private PhotonView pv;
    private string time;
    private string lastUpdateTime;

    private int roundCount;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        pv = GetComponent<PhotonView>();
        publicStageNow = PlayerManagerForAll.gamestage.Investigate;
        gamePhaseTimer = investigateTime;

        investigationPanel = playerPanel.transform.Find("MainMenu").Find("InvestigationPanel").gameObject;
        discussionPanel = playerPanel.transform.Find("MainMenu").Find("DiscussionPanel").gameObject;

        TimerTitle = timerPanel.transform.Find("TimerTitle").gameObject;
        EndButton = timerPanel.transform.Find("EndButton").gameObject;

        if(PhotonNetwork.IsMasterClient)
            SwitchStage(PlayerManagerForAll.gamestage.Investigate);
    }
    

    private void Update()
    { 
        if(!PhotonNetwork.IsMasterClient)
        {
            return;
        }
        if(gamePhaseTimer <= 0 && !timeout)
        {
            if(publicStageNow == PlayerManagerForAll.gamestage.Investigate)
            {
                SwitchStage(PlayerManagerForAll.gamestage.Discussion);
            }
            else if(publicStageNow == PlayerManagerForAll.gamestage.Discussion)
            {
                SwitchStage(PlayerManagerForAll.gamestage.Investigate);
            }
        }
        else if(gamePhaseTimer > 0 && !timeout)
        {
            time = (gamePhaseTimer/60).ToString("00") + ":" + Mathf.Floor(gamePhaseTimer%60).ToString("00");
            timerPanel.transform.GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>().text = time;
            gamePhaseTimer -= Time.deltaTime;
            if(time != lastUpdateTime)
            {
                //Debug.Log(time);
                pv.RPC(nameof(SyncTimer), RpcTarget.Others, publicStageNow.ToString(), time);
                lastUpdateTime = time;
            }
        }
    }
    private void PublicStageChange(PlayerManagerForAll.gamestage nextStage)
    {
        switch(nextStage)
        {
            case PlayerManagerForAll.gamestage.Investigate:
                pv.RPC(nameof(InvestigationManagerSwitch), RpcTarget.All, true);
                pv.RPC(nameof(ResetClueSharedLimitSynchronize), RpcTarget.All);
                TimerTitle.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Investigation";
                //Open and close Map and detective board
                PCMapPanel.SetActive(true);
                DetectiveBoardPanel.SetActive(false);
                currentStageTimer = investigateTime;
                break;
            case PlayerManagerForAll.gamestage.Discussion:
                pv.RPC(nameof(InvestigationManagerSwitch), RpcTarget.All, false);
                TimerTitle.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Discussion";
                //Open and close Map and detective board
                PCMapPanel.SetActive(false);
                DetectiveBoardPanel.SetActive(true);
                currentStageTimer = discussTime;
                break;
        }
        pv.RPC(nameof(ChangeAllPlayerStage), RpcTarget.All, nextStage);
        publicStageNow = nextStage;
        
        gamePhaseTimer = currentStageTimer;
    }
    public void SkipStageButton()
    {
        timeout = true;
        TimerTitle.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "00:00";
        switch (publicStageNow)
        {
            case PlayerManagerForAll.gamestage.Investigate:
                SwitchStage(PlayerManagerForAll.gamestage.Discussion);
                break;
            case PlayerManagerForAll.gamestage.Discussion:
                SwitchStage(PlayerManagerForAll.gamestage.Investigate);
                break;
        }
    }
    public void SwitchStage(PlayerManagerForAll.gamestage nextStage)
    {
        playerController.Instance.currentClueSharedNum = 0;
        timeout = true;
        pv.RPC(nameof(SwitchStageVisualRPC), RpcTarget.All, 2f, nextStage);
        StartCoroutine(TimerPauseCoroutine(2f, nextStage));
    }
    public void TimeupEndTransition(PlayerManagerForAll.gamestage stage)
    {
        transtitionPanel.gameObject.SetActive(true);
    }

    #region RPC
    [PunRPC]
    public void InvestigationManagerSwitch(bool active)
    {
        InvestigationManager.Instance.gameObject.SetActive(active);
        discussionPanel.gameObject.SetActive(!active);
    }

    [PunRPC]
    public void SyncTimer(string stage, string time)
    {
        playerTimerPanel.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = stage;
        playerTimerPanel.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = time;
    }

    [PunRPC]
    private void ChangeAllPlayerStage(PlayerManagerForAll.gamestage stage)
    {
        GameObject[] playerList = GameObject.FindGameObjectsWithTag("Player");

        foreach(var player in playerList)
        {
            player.GetComponent<playerController>().stageNow = stage;
        }
    }

    [PunRPC]
    public void SwitchStageVisualRPC(float sec, PlayerManagerForAll.gamestage nextStage)
    {
        if(NotificationScript.Instance != null)
            Destroy(NotificationScript.Instance.gameObject);
        StartCoroutine(SwitchStageVisualCoroutine(sec, nextStage));
    }

    [PunRPC]
    private void ResetClueSharedLimitSynchronize()
    {
        playerController.Instance.currentClueSharedNum = 0;
    }

    #endregion

    
    IEnumerator SwitchStageVisualCoroutine(float sec, PlayerManagerForAll.gamestage nextStage)
    {
        transtitionPanel.gameObject.SetActive(true);
        switch(nextStage)
        {
            case PlayerManagerForAll.gamestage.Investigate:
                roundCount++;
                transtitionPanel.transform.Find("Round").GetChild(0).GetComponent<TextMeshProUGUI>().text = "Round " + roundCount;
                TimerTitle.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Investigation";
                if(!PhotonNetwork.IsMasterClient)
                    transtitionPanel.transform.Find("Title").GetChild(0).GetComponent<TextMeshProUGUI>().text = "Investigation\r\n-\r\n" + playerController.Instance.currentMemory.GetComponent<MemoryInfo>().memory;
                else
                    transtitionPanel.transform.Find("Title").GetChild(0).GetComponent<TextMeshProUGUI>().text = "Investigation";
                break;
            case PlayerManagerForAll.gamestage.Discussion:
                BaseUIManager.Instance.CloseCollectedUI();
                TimerTitle.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Discussion";
                transtitionPanel.transform.Find("Title").GetChild(0).GetComponent<TextMeshProUGUI>().text = "Discussion";
                break;
        }
        yield return new WaitForSeconds(sec);
        transtitionPanel.gameObject.SetActive(false);

    }
    IEnumerator TimerPauseCoroutine(float sec, PlayerManagerForAll.gamestage nextStage)
    {
        timeout = true;
        //Change button text on PC
        switch(nextStage)
        {
            case PlayerManagerForAll.gamestage.Investigate:
                EndButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "End Investigation";
                break;
            case PlayerManagerForAll.gamestage.Discussion:
                EndButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "End Discussion";
                break;
            default:
                break;
        }
        TimerTitle.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "00:00";
        EndButton.gameObject.SetActive(false);
        yield return new WaitForSeconds(sec);
        EndButton.gameObject.SetActive(true);
        PublicStageChange(nextStage);
        timeout = false;
    }

}

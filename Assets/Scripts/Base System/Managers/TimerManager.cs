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
    private GameObject accusationPanel;
    private GameObject characterPanel;
    private GameObject cluePanel;
    private GameObject puzzlePanel;
    
    [SerializeField]
    private GameObject transtitionPanel;
    
    [SerializeField]
    private float investigateTime;
    [SerializeField]
    private float discussTime;
    [SerializeField]
    private float accuseTime;
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
        accusationPanel = playerPanel.transform.Find("MainMenu").Find("PlayerAccusationPanel").gameObject;

        characterPanel = playerPanel.transform.Find("CharacterMenu").gameObject;
        cluePanel = playerPanel.transform.Find("ClueMenu").gameObject;
        puzzlePanel = playerPanel.transform.Find("PuzzleMenu").gameObject;

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
            else if(publicStageNow == PlayerManagerForAll.gamestage.Accusation)
            {
                Debug.Log("Accusation Time Up");
            }
        }
        else if(gamePhaseTimer > 0 && !timeout)
        {
            time = Mathf.Floor(gamePhaseTimer/60).ToString("00") + ":" + Mathf.Floor(gamePhaseTimer%60).ToString("00");
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
            case PlayerManagerForAll.gamestage.Accusation:
                pv.RPC(nameof(AccusationSwitch), RpcTarget.All);
                TimerTitle.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Accusation";
                //Open and close Map and detective board
                PCMapPanel.SetActive(false);
                DetectiveBoardPanel.SetActive(true);
                currentStageTimer = accuseTime;
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
        SoundManager.Instance.PlayBGM(nextStage.ToString());
        playerController.Instance.currentClueSharedNum = 0;
        timeout = true;
        pv.RPC(nameof(SwitchStageVisualRPC), RpcTarget.All, 3f, nextStage);
        StartCoroutine(TimerPauseCoroutine(3f, nextStage));
    }
    
    private void CloseAllMenuonSwitch()
    {
        playerTimerPanel.SetActive(true);
        characterPanel.GetComponent<DeactivateChild>().CloseThisMenu();
        cluePanel.GetComponent<DeactivateChild>().CloseThisMenu();
        puzzlePanel.GetComponent<DeactivateChild>().CloseThisMenu();
    }

    #region RPC
    [PunRPC]
    public void InvestigationManagerSwitch(bool active)
    {
        InvestigationManager.Instance.gameObject.SetActive(active);
        playerPanel.transform.Find("MainMenu").gameObject.SetActive(true);
        investigationPanel.gameObject.SetActive(active);
        discussionPanel.gameObject.SetActive(!active);
        CloseAllMenuonSwitch();
    }
    [PunRPC]
    public void AccusationSwitch()
    {
        InvestigationManager.Instance.gameObject.SetActive(false);
        playerPanel.transform.Find("MainMenu").gameObject.SetActive(true);
        investigationPanel.gameObject.SetActive(false);
        discussionPanel.gameObject.SetActive(false);
        accusationPanel.SetActive(true);
        CloseAllMenuonSwitch();
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
                {
                    string tempTitle;
                    if(playerController.Instance.currentMemory.GetComponent<MemoryInfo>().memory == Memory.BishopMemory)
                    {
                        tempTitle = "<b>Investigation</b>\r\n-\r\n" + "<u>Bishop's Memory\r\n</u>" + "\"Moment of Discovery\"";
                    }
                    else
                    {
                        tempTitle = "<b>Investigation</b>\r\n-\r\n" + "<u>Ava's Memory\r\n</u>" + "\"" + playerController.Instance.currentMemory.GetComponent<MemoryInfo>().memory.ToString()+ "\"";
                    }
                    transtitionPanel.transform.Find("Title").GetChild(0).GetComponent<TextMeshProUGUI>().text = tempTitle;
                } 
                else
                    transtitionPanel.transform.Find("Title").GetChild(0).GetComponent<TextMeshProUGUI>().text = "Investigation";
                break;
            case PlayerManagerForAll.gamestage.Discussion:
                BaseUIManager.Instance.CloseCollectedUI();
                TimerTitle.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Discussion";
                transtitionPanel.transform.Find("Title").GetChild(0).GetComponent<TextMeshProUGUI>().text = "<b>Discussion</b>";
                break;
            case PlayerManagerForAll.gamestage.Accusation:
                BaseUIManager.Instance.CloseCollectedUI();
                transtitionPanel.transform.Find("Round").GetChild(0).GetComponent<TextMeshProUGUI>().text = "Final Round";
                TimerTitle.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Accusation";
                transtitionPanel.transform.Find("Title").GetChild(0).GetComponent<TextMeshProUGUI>().text = "<b>Accusation</b>";
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
            case PlayerManagerForAll.gamestage.Accusation:
                EndButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "End Accusation";
                break;
            default:
                break;
        }
        TimerTitle.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "00:00";
        EndButton.gameObject.SetActive(false);
        yield return new WaitForSeconds(sec);
        if(nextStage != PlayerManagerForAll.gamestage.Accusation)
            EndButton.gameObject.SetActive(true);
        PublicStageChange(nextStage);
        timeout = false;
    }

}

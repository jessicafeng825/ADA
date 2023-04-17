using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
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
    private GameObject PCaccusationPanel;
    
    [SerializeField]
    private GameObject PCTimerPanel;
    private GameObject PCTimerTitle;
    private GameObject EndButton;
    
    [SerializeField]
    private GameObject playerTimerPanel;
    
    [SerializeField]
    private GameObject playerPanel;
    private GameObject investigationPanel;
    private GameObject discussionPanel;
    private GameObject playerAccusationPanel;
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
    public gamestage publicStageNow;
    private bool timeout;
    private PhotonView pv;
    private string time;
    private string lastUpdateTime;
    private bool accusationTimeUp;

    private int roundCount;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        pv = GetComponent<PhotonView>();
        publicStageNow = gamestage.Investigate;
        gamePhaseTimer = investigateTime;

        investigationPanel = playerPanel.transform.Find("MainMenu").Find("InvestigationPanel").gameObject;
        discussionPanel = playerPanel.transform.Find("MainMenu").Find("DiscussionPanel").gameObject;
        playerAccusationPanel = playerPanel.transform.Find("MainMenu").Find("PlayerAccusationPanel").gameObject;

        characterPanel = playerPanel.transform.Find("CharacterMenu").gameObject;
        cluePanel = playerPanel.transform.Find("ClueMenu").gameObject;
        puzzlePanel = playerPanel.transform.Find("PuzzleMenu").gameObject;

        PCTimerTitle = PCTimerPanel.transform.Find("TimerTitle").gameObject;
        EndButton = PCTimerPanel.transform.Find("EndButton").gameObject;

        if(PhotonNetwork.IsMasterClient)
            SwitchStage(gamestage.Investigate);

        
    }
    
            
        
    

    private void Update()
    { 
        if(!PhotonNetwork.IsMasterClient)
        {
            return;
        }
        if(gamePhaseTimer <= 0 && !timeout)
        {
            if(publicStageNow == gamestage.Investigate)
            {
                SwitchStage(gamestage.Discussion);
            }
            else if(publicStageNow == gamestage.Discussion)
            {
                SwitchStage(gamestage.Investigate);
            }
            else if(publicStageNow == gamestage.Accusation && !accusationTimeUp)
            {
                accusationTimeUp = true;
                AccusationManager.Instance.ForceConclusion();
                Debug.Log("Accusation Time Up");
            }
        }
        else if(gamePhaseTimer > 0 && !timeout)
        {
            time = Mathf.Floor(gamePhaseTimer/60).ToString("00") + ":" + Mathf.Floor(gamePhaseTimer%60).ToString("00");
            PCTimerPanel.transform.Find("TimerTitle").Find("TimeText (TMP)").GetComponent<TextMeshProUGUI>().text = time;
            gamePhaseTimer -= Time.deltaTime;
            if(time != lastUpdateTime)
            {
                //Debug.Log(time);
                pv.RPC(nameof(SyncTimer), RpcTarget.Others, publicStageNow.ToString(), time);
                lastUpdateTime = time;
            }
        }
    }
    private void PublicStageChange(gamestage nextStage)
    {
        switch(nextStage)
        {
            case gamestage.Investigate:
                pv.RPC(nameof(InvestigationManagerSwitch), RpcTarget.All, true);
                pv.RPC(nameof(ResetClueSharedLimitSynchronize), RpcTarget.All);
                currentStageTimer = investigateTime;
                break;
            case gamestage.Discussion:
                pv.RPC(nameof(InvestigationManagerSwitch), RpcTarget.All, false);
                currentStageTimer = discussTime;
                break;
            case gamestage.Accusation:
                pv.RPC(nameof(AccusationSwitch), RpcTarget.All);
                currentStageTimer = accuseTime;
                break;
        }
        pv.RPC(nameof(ChangeAllPlayerStage), RpcTarget.AllBufferedViaServer, nextStage);
        publicStageNow = nextStage;
        
        gamePhaseTimer = currentStageTimer;
    }
    public void LateJoinSwitchStage(gamestage nextStage)
    {
        switch(nextStage)
        {
            case gamestage.Investigate:
                InvestigationManager.Instance.gameObject.SetActive(true);
                playerPanel.transform.Find("MainMenu").gameObject.SetActive(true);
                investigationPanel.gameObject.SetActive(true);
                discussionPanel.gameObject.SetActive(false);
                playerController.Instance.currentClueSharedNum = 0;
                playerController.Instance.ChangeStage(gamestage.Investigate);
                CloseAllMenuonSwitch();
                break;
            case gamestage.Discussion:
                InvestigationManager.Instance.gameObject.SetActive(false);
                playerPanel.transform.Find("MainMenu").gameObject.SetActive(true);
                investigationPanel.gameObject.SetActive(false);
                discussionPanel.gameObject.SetActive(true);
                playerController.Instance.ChangeStage(gamestage.Discussion);
                CloseAllMenuonSwitch();
                break;
            case gamestage.Accusation:
                InvestigationManager.Instance.gameObject.SetActive(false);
                playerPanel.transform.Find("MainMenu").gameObject.SetActive(true);
                investigationPanel.gameObject.SetActive(false);
                discussionPanel.gameObject.SetActive(false);
                playerController.Instance.ChangeStage(gamestage.Accusation);
                playerAccusationPanel.SetActive(true);
                break;
        }
    }
    public void SkipStageButton()
    {
        timeout = true;
        PCTimerTitle.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "00:00";
        switch (publicStageNow)
        {
            case gamestage.Investigate:
                SwitchStage(gamestage.Discussion);
                break;
            case gamestage.Discussion:
                SwitchStage(gamestage.Investigate);
                break;
        }
    }
    public void SwitchStage(gamestage nextStage)
    {
        SoundManager.Instance.PlayBGM(nextStage.ToString());
        //SoundManager.Instance.PlaySoundEffect("SFX_EnterMemory");
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
        playerAccusationPanel.SetActive(true);
        PCaccusationPanel.SetActive(true);
        CloseAllMenuonSwitch();
    }

    [PunRPC]
    public void SyncTimer(string stage, string time)
    {
        playerTimerPanel.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = stage;
        playerTimerPanel.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = time;
    }

    [PunRPC]
    private void ChangeAllPlayerStage(gamestage stage)
    {
        GameObject[] playerList = GameObject.FindGameObjectsWithTag("Player");
        publicStageNow = stage;
        foreach(var player in playerList)
        {
            player.GetComponent<playerController>().stageNow = stage;
        }
    }

    [PunRPC]
    public void SwitchStageVisualRPC(float sec, gamestage nextStage)
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

    
    IEnumerator SwitchStageVisualCoroutine(float sec, gamestage nextStage)
    {
        PCTimerTitle.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "00:00";
        transtitionPanel.gameObject.SetActive(true);
        switch(nextStage)
        {
            case gamestage.Investigate:
                roundCount++;
                transtitionPanel.transform.Find("Round").GetChild(0).GetComponent<TextMeshProUGUI>().text = "Round " + roundCount;
                PCTimerTitle.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Investigation";
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
                
                //Open and close Map and detective board
                PCMapPanel.SetActive(true);
                DetectiveBoardPanel.SetActive(false);

                EndButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "End Investigation";
                break;
            case gamestage.Discussion:
                BaseUIManager.Instance.CloseCollectedUI();
                PCTimerTitle.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Discussion";
                transtitionPanel.transform.Find("Title").GetChild(0).GetComponent<TextMeshProUGUI>().text = "<b>Discussion</b>";

                //Open and close Map and detective board
                PCMapPanel.SetActive(false);
                DetectiveBoardPanel.SetActive(true);

                EndButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "End Discussion";
                break;
            case gamestage.Accusation:
                BaseUIManager.Instance.CloseCollectedUI();
                transtitionPanel.transform.Find("Round").GetChild(0).GetComponent<TextMeshProUGUI>().text = "Final Round";
                PCTimerTitle.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Accusation";
                transtitionPanel.transform.Find("Title").GetChild(0).GetComponent<TextMeshProUGUI>().text = "<b>Accusation</b>";
                
                //Open and close Map and detective board
                PCMapPanel.SetActive(false);
                DetectiveBoardPanel.SetActive(true);
                
                EndButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "End Accusation";
                break;
        }
        yield return new WaitForSeconds(sec);
        transtitionPanel.gameObject.SetActive(false);

    }
    IEnumerator TimerPauseCoroutine(float sec, gamestage nextStage)
    {
        timeout = true;
        PCTimerTitle.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "00:00";
        EndButton.gameObject.SetActive(false);
        yield return new WaitForSeconds(sec);
        if(nextStage != gamestage.Accusation)
            EndButton.gameObject.SetActive(true);
        PublicStageChange(nextStage);
        timeout = false;
    }

    public void RequestRoundCount()
    {
        pv.RPC(nameof(MasterSendRoundCount), RpcTarget.MasterClient);
        Debug.Log("Round Count Updated to: " + roundCount);
    }
    [PunRPC]
    private void MasterSendRoundCount()
    {
        pv.RPC(nameof(RecieveRoundCount), RpcTarget.Others, roundCount);
    }
    [PunRPC]
    private void RecieveRoundCount(int round)
    {
        roundCount = round;
        Debug.Log("Round Count Updated to: " + roundCount);
    }

}

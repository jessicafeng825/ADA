using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class AccusationManager : MonoBehaviour
{
    private PhotonView pv;

    public static AccusationManager Instance;

    private GameObject titleText;
    
    [SerializeField]
    private GameObject playerAccusationPanel;

    
    [SerializeField]
    private GameObject PCAccusationPanel;

    private List<Button> playerButtons = new List<Button>();

    private List<GameObject> PCIcons = new List<GameObject>();

    private GameObject accuseButton;

    private GameObject verifyButton;
    private GameObject clueButton;
    private GameObject puzzleButton;
    static private Button selectedPlayer;
    
    private GameObject playerBackground;
    private GameObject PCBackground;

    private List<string> highestVotedPlayers = new List<string>();

    private string publicSelectedPlayer;


    [SerializeField]
    private Color selectedColor;

    [SerializeField]
    private Color unselectedColor;

    [SerializeField]
    private Color comfirmedColor;

    [SerializeField]
    private Color normalBackgroundColor;

    [SerializeField]
    private Color comfirmedBackgroundColor;

    [SerializeField]
    private Color correctBackgroundColor;

    [SerializeField]
    private Color wrongBackgroundColor;

    private int[] allVotes = new int[6] { 0, 0, 0, 0, 0, 0 };

    public bool solvedTheCase;


    private void Start() 
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        pv = GetComponent<PhotonView>();
        playerBackground = this.playerAccusationPanel.transform.Find("Background").gameObject;
        PCBackground = this.PCAccusationPanel.transform.Find("Background").gameObject;
        accuseButton = this.playerAccusationPanel.transform.Find("AccuseButton").gameObject;
        clueButton = this.playerAccusationPanel.transform.Find("ClueButton").GetChild(0).gameObject;
        puzzleButton = this.playerAccusationPanel.transform.Find("PuzzleButton").GetChild(0).gameObject;
        titleText = this.playerAccusationPanel.transform.Find("Title").transform.GetChild(0).gameObject;
        accuseButton.GetComponent<Button>().onClick.AddListener(AccusePlayer);
        verifyButton = this.PCAccusationPanel.transform.Find("VerifyButton").gameObject;
        verifyButton.GetComponent<Button>().onClick.AddListener(() => VerifyResult(publicSelectedPlayer));
        foreach(Transform button in this.playerAccusationPanel.transform.Find("PlayerButtons"))
        {
            playerButtons.Add(button.GetComponent<Button>());
        }
        foreach(Transform child in this.PCAccusationPanel.transform.Find("PlayerIcons"))
        {
            PCIcons.Add(child.gameObject);
        }
    }
    public void SelectPlayer(Button b)
    {
        if(selectedPlayer != null)
        {
            if(selectedPlayer == b)
            {
                b.GetComponent<Image>().color = unselectedColor;
                selectedPlayer = null;
            }
            else
            {
                b.GetComponent<Image>().color = selectedColor;
                selectedPlayer.GetComponent<Image>().color = unselectedColor;
                selectedPlayer = b;
            }
        }
        else
        {
            b.GetComponent<Image>().color = selectedColor;
            selectedPlayer = b;
        }
        
    }
    
    public void AccusePlayer()
    {
        if(selectedPlayer == null)
        {
            return;
        }
        else
        {
            clueButton.GetComponent<Button>().interactable = false;
            puzzleButton.GetComponent<Button>().interactable = false;
            foreach(Button button in playerButtons)
            {
                button.enabled = false;
                if(button != selectedPlayer)
                    button.transform.Find("Mask").gameObject.SetActive(true);
            }
            playerController.Instance.AccusePlayer(selectedPlayer.name);
            selectedPlayer.GetComponent<Image>().color = comfirmedColor;
            playerBackground.GetComponent<Image>().color = comfirmedBackgroundColor;
            accuseButton.SetActive(false);

        }
        foreach(GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            if(player.GetComponent<playerController>().accusedPlayer != "None")
            {
                continue;
            }
            else
            {
                return;
            }
        }
        pv.RPC(nameof(VoteConclusionMaster), RpcTarget.MasterClient);
        
    }

    [PunRPC]
    public void ResetAccuseRPC(string player01, string player02, string player03, string player04, string player05, string player06)
    {
        clueButton.GetComponent<Button>().interactable = true;
        puzzleButton.GetComponent<Button>().interactable = true;
        List<string> players = new List<string>();
        players.Add(player01);
        players.Add(player02);
        players.Add(player03);
        players.Add(player04);
        players.Add(player05);
        players.Add(player06);
        foreach(Button button in playerButtons)
        {
            if(players.Contains(button.name))
            {
                button.enabled = true;
                button.transform.Find("Mask").gameObject.SetActive(false);
                button.GetComponent<Image>().color = unselectedColor;
            }
            else
            {
                button.gameObject.SetActive(false);
            }
        }
        foreach(GameObject icon in PCIcons)
        {
            if(players.Contains(icon.name))
            {
                icon.SetActive(true);
            }
            else
            {
                icon.SetActive(false);
            }
        }
        selectedPlayer = null;
        titleText.GetComponent<TextMeshProUGUI>().text = "There was a Tie!";
        if(PhotonNetwork.IsMasterClient)
        {
            playerController.Instance.AccusePlayer("Host");
        }
        else
        {
            playerController.Instance.AccusePlayer("None");
        }
        playerBackground.GetComponent<Image>().color = normalBackgroundColor;
        accuseButton.SetActive(true);
    }
    public void VerifyResult(string name)
    {
        pv.RPC(nameof(VerifyResultRPC), RpcTarget.All, name);
    }
    [PunRPC]
    private void VerifyResultRPC(string name)
    {
        if(name == "SecurityGuard")
        {
            solvedTheCase = true;
            StartCoroutine(FadeColor(PCBackground.GetComponent<Image>(), correctBackgroundColor, 1));
            StartCoroutine(FadeColor(playerBackground.GetComponent<Image>(), correctBackgroundColor, 1));
            // PCBackground.GetComponent<Image>().color = correctBackgroundColor;
            // playerBackground.GetComponent<Image>().color = correctBackgroundColor;
            playerAccusationPanel.transform.Find("Title").Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = "The Security Guard is the murderer!";
            PCAccusationPanel.transform.Find("Title").Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = "The Security Guard is the murderer!";
            Debug.Log("Correct");
        }
        else
        {
            solvedTheCase = false;
            StartCoroutine(FadeColor(PCBackground.GetComponent<Image>(), wrongBackgroundColor, 1));
            StartCoroutine(FadeColor(playerBackground.GetComponent<Image>(), wrongBackgroundColor, 1));
            // PCBackground.GetComponent<Image>().color = wrongBackgroundColor;
            // playerBackground.GetComponent<Image>().color = wrongBackgroundColor;
            playerAccusationPanel.transform.Find("Title").Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = name + " was not the murderer!";
            PCAccusationPanel.transform.Find("Title").Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = name + " was not the murderer!";
            Debug.Log("Wrong");
        }
        StartCoroutine(ChangetoOutroScene(6f));
        verifyButton.SetActive(false);
    }
    private IEnumerator FadeColor(Image image, Color newColor, float sec)
    {

        float time = 0;
        Color originColor = image.color;
        while(time < sec)
        {
            image.color = Color.Lerp(originColor, newColor, time/sec);
            time += Time.deltaTime;
            yield return null;
        }
        image.color = newColor;
    }
    IEnumerator ChangetoOutroScene(float sec)
    {
        yield return new WaitForSeconds(sec);
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.LoadLevel(3);

    }


    [PunRPC]
    public void VoteConclusionMaster()
    {
        PCAccusationPanel.SetActive(true);
        foreach(GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            switch(player.GetComponent<playerController>().accusedPlayer)
            {
                case "CyberneticBrawler":
                    allVotes[0]++;
                    break;
                case "NightclubOwner":
                    allVotes[1]++;
                    break;
                case "Lawyer":
                    allVotes[2]++;
                    break;
                case "Biohacker":
                    allVotes[3]++;
                    break;
                case "StreetKid":
                    allVotes[4]++;
                    break;
                case "SecurityGuard":
                    allVotes[5]++;
                    break;
                default:
                    break;
            }
        }
        int max = 0;
        for(int i = 0; i < allVotes.Length; i++)
        {
            if(allVotes[i] > max)
            {
                max = allVotes[i];
                highestVotedPlayers.Clear();
                highestVotedPlayers.Add(playerButtons[i].name);
            }
            else if(allVotes[i] == max)
            {
                highestVotedPlayers.Add(playerButtons[i].name);
            }
        }
        Debug.Log(highestVotedPlayers);
        if(highestVotedPlayers.Count == 1)
        {
            publicSelectedPlayer = highestVotedPlayers[0];
            VoteConclusionVisualPC(allVotes, publicSelectedPlayer);
            pv.RPC(nameof(VoteConclusionVisualPlayer), RpcTarget.Others, allVotes, publicSelectedPlayer);
        }
        else
        {
            string[] players = new string[6];
            for(int i = 0; i < highestVotedPlayers.Count; i++)
            {
                players[i] = highestVotedPlayers[i];
            }
            
            pv.RPC(nameof(ResetAccuseRPC), RpcTarget.All, players[0], players[1], players[2], players[3], players[4], players[5]);
            for(int i = 0; i < allVotes.Length; i++)
            {
                allVotes[i] = 0;
            }
        }
    }
    [PunRPC]
    public void VoteConclusionVisualPlayer(int[] votes, string name)
    {
        StartCoroutine(ConclusionVisualPlayer(votes, name));
    }

    public void VoteConclusionVisualPC(int[] votes, string name)
    {
        
        StartCoroutine(ConclusionVisualPC(votes, name));
    }

    IEnumerator ConclusionVisualPlayer(int[] votes, string name)
    {
        TextMeshProUGUI[] voteText = new TextMeshProUGUI[playerButtons.Count];
        for(int i = 0; i < playerButtons.Count; i++)
        {
            playerButtons[i].transform.Find("VoteNumber").gameObject.SetActive(true);
            voteText[i] = playerButtons[i].transform.Find("VoteNumber").Find("Text (TMP)").GetComponent<TextMeshProUGUI>();

        }
        float timer = 0;
        while(timer < 3f)
        {
            foreach(TextMeshProUGUI text in voteText)
            {
                text.text = Random.Range(0, 6).ToString();
            }
            timer+=Time.deltaTime;
            yield return null;
        }
        for(int i = 0; i < PCIcons.Count; i++)
        {
            voteText[i].text = votes[i].ToString();

        }
        
        foreach(Button playerButton in playerButtons)
        {
            if(playerButton.name == name)
            {
                playerButton.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
                playerButton.GetComponent<RectTransform>().sizeDelta = new Vector2(600, 600);
            }
            else
            {
                playerButton.gameObject.SetActive(false);
            }
        }
        playerAccusationPanel.transform.Find("Title").Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = name + " was voted the murderer!";
    }
    IEnumerator ConclusionVisualPC(int[] votes, string name)
    {
        TextMeshProUGUI[] voteText = new TextMeshProUGUI[PCIcons.Count];
        for(int i = 0; i < PCIcons.Count; i++)
        {
            PCIcons[i].transform.Find("VoteNumber").gameObject.SetActive(true);
            voteText[i] = PCIcons[i].transform.Find("VoteNumber").Find("Text (TMP)").GetComponent<TextMeshProUGUI>();

        }
        float timer = 0;
        while(timer < 3f)
        {
            foreach(TextMeshProUGUI text in voteText)
            {
                text.text = Random.Range(0, 6).ToString();
            }
            timer+=Time.deltaTime;
            yield return null;
        }
        for(int i = 0; i < PCIcons.Count; i++)
        {
            voteText[i].text = votes[i].ToString();

        }
        foreach(GameObject icon in PCIcons)
        {
            if(icon.name == name)
            {
                icon.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
                icon.GetComponent<RectTransform>().sizeDelta = new Vector2(600, 600);
            }
            else
            {
                icon.gameObject.SetActive(false);
            }
        }
        verifyButton.gameObject.SetActive(true);
        PCAccusationPanel.transform.Find("Title").Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = name + "was voted the murderer!";
    }

    public void VoteForAccusationPhase(GameObject button)
    {
        if(button.GetComponent<Image>().color == Color.white)
        {
            button.GetComponent<Image>().color = Color.black;
            button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.white;
        }
        else
        {
            button.GetComponent<Image>().color = Color.white;            
            button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.black;
        }
        playerController.Instance.voteForAccused = !playerController.Instance.voteForAccused;
        playerController.Instance.SyncVoteForAccusationPhase();
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach(GameObject player in players)
        {
            if(player.GetComponent<playerController>().voteForAccused == false && player.GetComponent<playerController>().playerJob != "Host")
                return;
        }
        InvestigationManager.Instance.AskMasterChangeStage(gamestage.Accusation);
    }
    

}

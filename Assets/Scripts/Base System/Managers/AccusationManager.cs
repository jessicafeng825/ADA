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
    
    [SerializeField]
    private GameObject playerAccusationPanel;

    
    [SerializeField]
    private GameObject PCAccusationPanel;

    private List<Button> playerButtons = new List<Button>();

    private List<GameObject> PCIcons = new List<GameObject>();

    private GameObject accuseButton;

    private GameObject verifyButton;
    static private Button selectedPlayer;
    
    private GameObject playerBackground;
    private GameObject PCBackground;

    private string publicSelectedPlayer;


    [SerializeField]
    private Color selectedColor;

    [SerializeField]
    private Color unselectedColor;

    [SerializeField]
    private Color comfirmedColor;

    [SerializeField]
    private Color comfirmedBackgroundColor;

    private int[] allVotes = new int[6] { 0, 0, 0, 0, 0, 0 };


    private void Start() 
    {
        pv = GetComponent<PhotonView>();
        playerBackground = this.playerAccusationPanel.transform.Find("Background").gameObject;
        PCBackground = this.PCAccusationPanel.transform.Find("Background").gameObject;
        accuseButton = this.playerAccusationPanel.transform.Find("AccuseButton").gameObject;
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
            if(player.GetComponent<playerController>().accusedPalyer != "")
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
    public void VerifyResult(string name)
    {
        if(name == "SecurityGuard")
        {
            Debug.Log("Correct");
        }
        else
        {
            Debug.Log("Wrong");
        }
    }


    [PunRPC]
    public void VoteConclusionMaster()
    {
        PCAccusationPanel.SetActive(true);
        foreach(GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            switch(player.GetComponent<playerController>().accusedPalyer)
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
                Debug.Log(i + ": " + allVotes[i]);
                max = allVotes[i];
                publicSelectedPlayer = playerButtons[i].name;
            }
        }
        VoteConclusionVisualPC(allVotes, publicSelectedPlayer);
        
        pv.RPC(nameof(VoteConclusionVisualPlayer), RpcTarget.Others, allVotes, publicSelectedPlayer);
        Debug.Log(publicSelectedPlayer);
    }
    [PunRPC]
    public void VoteConclusionVisualPlayer(int[] votes, string name)
    {
        int i = 0;
        foreach(Button playerButton in playerButtons)
        {
            playerButton.transform.Find("VoteNumber").gameObject.SetActive(true);
            Debug.Log(playerButton.name + " got " + votes[i].ToString() + " votes");
            playerButton.transform.Find("VoteNumber").Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = votes[i].ToString();
            i++;            
        }
        StartCoroutine(ConclusionVisualPlayer(name));
    }

    public void VoteConclusionVisualPC(int[] votes, string name)
    {
        int i = 0;
        foreach(GameObject icon in PCIcons)
        {
            icon.transform.Find("VoteNumber").gameObject.SetActive(true);
            Debug.Log(icon.name + " got " + votes[i].ToString() + " votes");
            icon.transform.Find("VoteNumber").Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = votes[i].ToString();
            i++;            
        }
        StartCoroutine(ConclusionVisualPC(name));
    }

    IEnumerator ConclusionVisualPlayer(string name)
    {
        yield return new WaitForSeconds(3f);
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
    IEnumerator ConclusionVisualPC(string name)
    {
        yield return new WaitForSeconds(3f);
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
}

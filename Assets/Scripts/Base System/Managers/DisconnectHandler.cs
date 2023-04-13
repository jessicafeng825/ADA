using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public struct DisconnectedPlayer
{
    public string character { get; set; }
    public string[] clues { get; set; }
    public string[] clueLocation { get; set; }
    public string[] puzzles { get; set; }
    public string[] puzzleLocation { get; set; }
    public DisconnectedPlayer(string character, string[] clues, string[] clueLocation, string[] puzzles, string[] puzzleLocation)
    {
        this.character = character;
        this.clues = clues;
        this.clueLocation = clueLocation;
        this.puzzles = puzzles;
        this.puzzleLocation = puzzleLocation;
    }
}


public class DisconnectHandler : MonoBehaviourPun
{
    public List<DisconnectedPlayer> disconnectedPlayers = new List<DisconnectedPlayer>();
    
    public DisconnectedPlayer myDisconnectedPlayer;

    public const byte SaveDisconnectedPlayerStatusEvent = 1;
    public const byte SendDisconnectedPlayerStatusEvent = 2;
    public static DisconnectHandler Instance;

    private PhotonView pv;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    void Start()
    {
        pv = GetComponent<PhotonView>();
    }
    private void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
    }

    private void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
    }
    
    void OnApplicationPause(bool pauseStatus)
    {
        Debug.Log("Application Pause Status: " + pauseStatus);
        SaveDisconnectedPlayerStatus();
    }
    public void OnEvent(EventData photonEvent)
    {
        byte eventCode = photonEvent.Code;
        if(eventCode == SaveDisconnectedPlayerStatusEvent)
        {
            object[] data = (object[])photonEvent.CustomData;
            DisconnectedPlayer dp = new DisconnectedPlayer((string)data[0], (string[])data[1], (string[])data[2], (string[])data[3], (string[])data[4]);
            foreach(DisconnectedPlayer d in disconnectedPlayers)
            {
                if(d.character == dp.character)
                {
                    disconnectedPlayers.Remove(d);
                    break;
                }
            }
            disconnectedPlayers.Add(dp);
            Debug.Log(dp.character + "Disconnected Player Status Saved");
            Debug.Log(dp.clues.Length + "Clues Saved");
            Debug.Log(dp.puzzles.Length + "Puzzles Saved");
        }
        else if (eventCode == SendDisconnectedPlayerStatusEvent)
        {
            object[] data = (object[])photonEvent.CustomData;
            myDisconnectedPlayer = new DisconnectedPlayer((string)data[0], (string[])data[1], (string[])data[2], (string[])data[3], (string[])data[4]);
            foreach(string clue in myDisconnectedPlayer.clues)
            {
                Debug.Log("Clue: " + clue);
            }
            foreach(string clueLocation in myDisconnectedPlayer.clueLocation)
            {
                Debug.Log("Clue location: " + clueLocation);
            }
            foreach(string puzzle in myDisconnectedPlayer.puzzles)
            {
                Debug.Log("Puzzle: " + puzzle);
            }
            foreach(string puzzleLocation in myDisconnectedPlayer.puzzleLocation)
            {
                Debug.Log("Puzzle location: " + puzzleLocation);
            }
            for(int i = 0; i < myDisconnectedPlayer.clueLocation.Length; i++)
            {
                Memory location;
                switch(myDisconnectedPlayer.clueLocation[i])
                {
                    case "BishopMemory":
                        location = Memory.BishopMemory;
                        break;
                    case "Mansion":
                        location = Memory.Mansion;
                        break;
                    case "StreetCorner":
                        location = Memory.StreetCorner;
                        break;
                    case "LawyerOffice":
                        location = Memory.LawyerOffice;
                        break;
                    case "Nightclub":
                        location = Memory.Nightclub;
                        break;
                    case "VoidBase":
                        location = Memory.VoidBase;
                        break;
                    case "SecretLaboratory":
                        location = Memory.SecretLaboratory;
                        break;
                    default:
                        location = Memory.None;
                        break;
                }
                InvestigationManager.Instance.AddCluePrefab(myDisconnectedPlayer.clues[i], location);
            }
            for(int i = 0; i < myDisconnectedPlayer.puzzleLocation.Length; i++)
            {
                Memory location;
                switch(myDisconnectedPlayer.puzzleLocation[i])
                {
                    case "BishopMemory":
                        location = Memory.BishopMemory;
                        break;
                    case "Mansion":
                        location = Memory.Mansion;
                        break;
                    case "StreetCorner":
                        location = Memory.StreetCorner;
                        break;
                    case "LawyerOffice":
                        location = Memory.LawyerOffice;
                        break;
                    case "Nightclub":
                        location = Memory.Nightclub;
                        break;
                    case "VoidBase":
                        location = Memory.VoidBase;
                        break;
                    case "SecretLaboratory":
                        location = Memory.SecretLaboratory;
                        break;
                    default:
                        location = Memory.None;
                        break;
                }
                InvestigationManager.Instance.AddPuzzlePrefab(myDisconnectedPlayer.puzzles[i], location);
            }
            Debug.Log(myDisconnectedPlayer.character + "Disconnected Player Status Revised");
            Debug.Log(myDisconnectedPlayer.clues.Length + "Clues Revised");
            Debug.Log(myDisconnectedPlayer.puzzles.Length + "Puzzles Revised");
            myDisconnectedPlayer = new DisconnectedPlayer();
        }
    }
    
    public void SaveDisconnectedPlayerStatus()
    {
        string[] clueNames = BaseUIManager.Instance.inBaseClueBtns.Keys.ToArray();
        string[] clueLocations = new string[clueNames.Length];
        for(int i = 0; i < clueNames.Length; i++)
        {
            clueLocations[i] = BaseUIManager.Instance.inBaseClueBtns[clueNames[i]].GetComponent<ClueBtn>().collectedAt.ToString();
        }
        string[] puzzleNames = BaseUIManager.Instance.inBasePuzzleBtns.Keys.ToArray();
        string[] puzzleLocations = new string[puzzleNames.Length];
        for(int i = 0; i < puzzleNames.Length; i++)
        {
            puzzleLocations[i] = BaseUIManager.Instance.inBasePuzzleBtns[puzzleNames[i]].GetComponent<PuzzleBtn>().collectedAt.ToString();
        }
        clueNames = clueNames.Where(x => !string.IsNullOrEmpty(x)).ToArray();
        clueLocations = clueLocations.Where(x => !string.IsNullOrEmpty(x)).ToArray();
        puzzleNames = puzzleNames.Where(x => !string.IsNullOrEmpty(x)).ToArray();
        puzzleLocations = puzzleLocations.Where(x => !string.IsNullOrEmpty(x)).ToArray();
        object[] data = new object[] { playerController.Instance.playerJob, clueNames, clueLocations, puzzleNames, puzzleLocations};
        PhotonNetwork.RaiseEvent(SaveDisconnectedPlayerStatusEvent, data, new RaiseEventOptions { Receivers = ReceiverGroup.MasterClient }, SendOptions.SendUnreliable);
        
    }

    public void RequestDisconnectedPlayerStatus(string character)
    {
        int actorNumber = PhotonNetwork.LocalPlayer.ActorNumber;
        pv.RPC(nameof(RequestDisconnectedPlayerStatusRPC), RpcTarget.MasterClient, character, actorNumber);
    }
    [PunRPC]
    private void RequestDisconnectedPlayerStatusRPC(string character, int actorNumber)
    {
        foreach(DisconnectedPlayer dp in disconnectedPlayers)
        {
            if(dp.character == character)
            {
                object[] data = new object[] { dp.character, dp.clues, dp.clueLocation, dp.puzzles, dp.puzzleLocation };
                PhotonNetwork.RaiseEvent(SendDisconnectedPlayerStatusEvent, data, new RaiseEventOptions { TargetActors = new int[] {actorNumber} }, SendOptions.SendUnreliable);
                disconnectedPlayers.Remove(dp);
                return;
            }
        }
    }
}

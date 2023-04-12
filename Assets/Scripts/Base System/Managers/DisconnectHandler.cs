using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public struct DisconnectedPlayer
{
    public string character { get; set; }
    public Dictionary<string, GameObject> clues { get; set; }
    public Dictionary<string, GameObject> puzzles { get; set; }
    public DisconnectedPlayer(string character, Dictionary<string, GameObject> clues, Dictionary<string, GameObject> puzzles)
    {
        this.character = character;
        this.clues = clues;
        this.puzzles = puzzles;
    }
}


public class DisconnectHandler : MonoBehaviour
{
    public List<DisconnectedPlayer> disconnectedPlayers = new List<DisconnectedPlayer>();
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

    public void AddDisconnectedPlayer(string character, Dictionary<string, GameObject> clues, Dictionary<string, GameObject> puzzles)
    {
        disconnectedPlayers.Add(new DisconnectedPlayer(character, clues, puzzles));
    }
    public void RemoveDisconnectedPlayer(string character)
    {
        for(int i = 0; i < disconnectedPlayers.Count; i++)
        {
            if(disconnectedPlayers[i].character == character)
            {
                disconnectedPlayers.RemoveAt(i);
            }
        }
    }
    
    public void SaveDisconnectedPlayerStatus()
    {
        pv.RPC("RPCSaveDisconnectedPlayerStatus", RpcTarget.MasterClient);
    }
    [PunRPC]
    private void RPCSaveDisconnectedPlayerStatus()
    {
        if(!PhotonNetwork.IsMasterClient)
            return;
        Debug.Log(playerController.Instance.playerJob + " is saved");
        DisconnectHandler.Instance.AddDisconnectedPlayer(playerController.Instance.playerJob, BaseUIManager.Instance.inBaseClueBtns, BaseUIManager.Instance.inBasePuzzleBtns);
    }
    public void RefreshDisconnectedPlayer(string character)
    {
        foreach(DisconnectedPlayer dp in disconnectedPlayers)
        {
            if(character == dp.character)
            {
                foreach(KeyValuePair<string, GameObject> clue in dp.clues)
                {
                    InvestigationManager.Instance.AddCluePrefab(clue.Key, clue.Value.GetComponent<ClueBtn>().collectedAt);
                }
                foreach(KeyValuePair<string, GameObject> puzzle in dp.puzzles)
                {
                    InvestigationManager.Instance.AddPuzzlePrefab(puzzle.Key, puzzle.Value.GetComponent<PuzzleBtn>().collectedAt);
                }
            }
        }
    }
}

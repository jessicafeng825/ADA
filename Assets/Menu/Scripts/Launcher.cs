using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using TMPro;
using Photon.Realtime;
using System.Linq;
using UnityEngine.SceneManagement;

public class Launcher : MonoBehaviourPunCallbacks {
  public static Launcher Instance;
  private string playerNickname;

  [SerializeField] TMP_InputField playerNameInputField;
  [SerializeField] TMP_Text titleWelcomeText;
  [SerializeField] TMP_InputField roomNameInputField;
  [SerializeField] Transform roomListContent;
  [SerializeField] GameObject roomListItemPrefab;
  [SerializeField] TMP_Text roomNameText;
  [SerializeField] Transform playerListContent;
  [SerializeField] GameObject playerListItemPrefab;
  [SerializeField] GameObject startGameButton;
  [SerializeField] TMP_Text errorText;
  [SerializeField] public Menu[] unableMenuList;

    private void Awake() {
    Instance = this;
    }

  private void Start() {
    SetName();
    Debug.Log("Connecting to master...");
    PhotonNetwork.ConnectUsingSettings();
        for (int i = 0; i < unableMenuList.Length; i++)
        {
            if (unableMenuList[i].open)
            {
                unableMenuList[i].Close();
            }
        }
        //SetName();
    MenuManager.Instance.OpenMenu("title");
    PhotonNetwork.KeepAliveInBackground = 300;
  }

  public override void OnConnectedToMaster() {
    Debug.Log("Connected to master!");
    // PhotonNetwork.JoinLobby();
    // Automatically load scene for all clients when the host loads a scene
    PhotonNetwork.AutomaticallySyncScene = true;
    }

    public void JoinLobby()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        // if (PhotonNetwork.NickName == "") {
        //   PhotonNetwork.NickName = "Player " + Random.Range(0, 1000).ToString(); // Set a default nickname, just as a backup
        //   MenuManager.Instance.OpenMenu("name");
        // } else 
        // {
        //MenuManager.Instance.OpenMenu("title");
        // }
        Debug.Log("Joined lobby");
    }

    public void LeaveLobby()
    {
        PhotonNetwork.LeaveLobby();
    }

    public override void OnLeftLobby()
    {
        Debug.Log("Left Lobby");   
    }

  public void SetName() {
    //string name = playerNameInputField.text;
    string name = "Detective " + Random.Range(0, 1000).ToString();
        if (!string.IsNullOrEmpty(name)) {
      PhotonNetwork.NickName = name;
      playerNickname = name;
      titleWelcomeText.text = $"Welcome, {name}!";
      //playerNameInputField.text = "";
    } else {
      Debug.Log("No player name entered");
      // TODO: Display an error to the user
    }
  }


  public void CreateRoom() {
        string roomName = "Room " + Random.Range(0, 1000).ToString();
        var options = new RoomOptions() { MaxPlayers = 7, PlayerTtl = 10 };
        options.CustomRoomPropertiesForLobby = new string[1] { "gameRunning" };
        options.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable();
        options.CustomRoomProperties.Add("gameRunning", false);
        PhotonNetwork.CreateRoom(roomName, roomOptions:options);
        MenuManager.Instance.OpenMenu("loading");

  }
   
  public override void OnJoinedRoom() {
    // Called whenever you create or join a room
    MenuManager.Instance.OpenMenu("room");
    roomNameText.text = PhotonNetwork.CurrentRoom.Name;
    Debug.Log("Joined room");
    if (PhotonNetwork.IsMasterClient)
    {
        PhotonNetwork.NickName = "Host";
    }

    UpdatePlayersOnUI();

    // Only enable the start button if the player is the host of the room
    startGameButton.SetActive(PhotonNetwork.IsMasterClient);
  }
  
  private void UpdatePlayersOnUI()
    {
        Player[] players = PhotonNetwork.PlayerList;
        foreach (Transform trans in playerListContent)
        {
            Destroy(trans.gameObject);
        }
        for (int i = 0; i < players.Count(); i++)
        {
            Instantiate(playerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(players[i]);
        }
    }

  public override void OnMasterClientSwitched(Player newMasterClient) {
    startGameButton.SetActive(PhotonNetwork.IsMasterClient);
  }

  public void LeaveRoom() {
    PhotonNetwork.LeaveRoom();
    MenuManager.Instance.OpenMenu("loading");
  }

  public void JoinRoom(RoomInfo info) {
    PhotonNetwork.JoinRoom(info.Name);
    MenuManager.Instance.OpenMenu("loading");
  }

  public override void OnLeftRoom() {
    PhotonNetwork.NickName = playerNickname;
    MenuManager.Instance.OpenMenu("title");
  }

  public override void OnRoomListUpdate(List<RoomInfo> roomList) {
        foreach (Transform trans in roomListContent)
        {
            Destroy(trans.gameObject);
        }

        for (int i = 0; i < roomList.Count; i++) {
            //if (roomList[i].RemovedFromList) {
            // Don't instantiate stale rooms
            //  continue;
            // }
            if (roomList[i].PlayerCount == 0)
            {
                continue;
            }
            Instantiate(roomListItemPrefab, roomListContent).GetComponent<RoomListItem>().SetUp(roomList[i]);
    }
  }

  public override void OnCreateRoomFailed(short returnCode, string message) {
    errorText.text = "Room Creation Failed: " + message;
    MenuManager.Instance.OpenMenu("error");
  }

  public override void OnPlayerEnteredRoom(Player newPlayer) {
    Instantiate(playerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(newPlayer);
  }

  public void StartGame() {
    // 1 is used as the build index of the game scene, defined in the build settings
    // Use this instead of scene management so that *everyone* in the lobby goes into this scene
    PhotonNetwork.LoadLevel(1);
  }
  

  public void QuitGame() {
    Application.Quit();
  }
}

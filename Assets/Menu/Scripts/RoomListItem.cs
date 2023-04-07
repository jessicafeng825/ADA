using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using TMPro;

public class RoomListItem : MonoBehaviour {
  [SerializeField] TMP_Text label;

  RoomInfo info;
  public void SetUp(RoomInfo _info) {
    info = _info;
    label.text = _info.Name;
    if(info.CustomProperties["gameRunning"].Equals(true)) {
      label.text += " (Started)";
    }
    else {
      label.text += " (" + info.PlayerCount + "/" + info.MaxPlayers + ")";
    }
  }

  public void OnClick() {
    Launcher.Instance.JoinRoom(info);
  }
}

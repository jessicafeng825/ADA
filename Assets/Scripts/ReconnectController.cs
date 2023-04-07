using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using UnityEngine;

public class ReconnectController : MonoBehaviourPunCallbacks
{
    private void OnApplicationPause(bool isPaused)
    {
        if (isPaused)
        {
            PhotonNetwork.Disconnect();
        }
    }
    private void Update()
    {
        if (PhotonNetwork.NetworkingClient.LoadBalancingPeer.PeerState == PeerStateValue.Disconnected)
        {
            if (!PhotonNetwork.ReconnectAndRejoin())
            {
                Debug.Log("Failed reconnecting and joining!!", this);
            }
            else
            {
                Debug.Log("Successful reconnected and joined!", this);
            }
        }
    }
}

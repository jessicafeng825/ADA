using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportInfo : MonoBehaviour
{
    [SerializeField]
    public GameObject teleportFromMemory, teleportToMemory;

    public void TeleportTriggered()
    {
        teleportFromMemory.SetActive(false);
        teleportToMemory.SetActive(true);
        Debug.Log("teleport succeed");
    }
}

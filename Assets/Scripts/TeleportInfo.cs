using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportInfo : MonoBehaviour
{
    [SerializeField]
    public Memory teleportFromMemory, teleportToMemory;

    public void TeleportTriggered()
    {
        InvestigationManager.Instance.SpawnTelepoetDialog("To " + teleportToMemory.ToString(), "Do you want to move from " + teleportFromMemory.ToString() + " to " + teleportToMemory.ToString() + "?",teleportFromMemory, teleportToMemory);
        Debug.Log("teleport succeed");
    }
}

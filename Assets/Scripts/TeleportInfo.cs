using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportInfo : MonoBehaviour
{
    [SerializeField]
    public Memory teleportFromMemory, teleportToMemory;

    public void TeleportTriggered()
    {
        InvestigationManager.Instance.TeleportToFrom(teleportFromMemory, teleportToMemory);
        Debug.Log("teleport succeed");
    }
}

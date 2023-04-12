using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class JoinAfterChooseCharacter : MonoBehaviour
{
    public void SpawnComfirmCharacterDialog(JOb job)
    {
        NotificationScript tempNoti = BaseUIManager.Instance.SpawnNotificationPanel(job.jobName, "Are you sure?<br>(If you have already read description from a certain character, please choose the character)", 2, -1);
        tempNoti.AddFunctiontoYesButton(() => ChooseCharacter(job), true);
    }

    public void ChooseCharacter(JOb job)
    {
        GameObject[] playerList = GameObject.FindGameObjectsWithTag("Player");

        foreach(var player in playerList)
        {
            if(player.GetComponent<playerController>().playerJob == job.jobName)
            {
                NotificationScript tempNoti = BaseUIManager.Instance.SpawnNotificationPanel("Failed", "This character has already been chosen", 1, 3);
                Debug.Log("This character has already been chosen");
                return;
            }
        }
        playerController.Instance.JoinAfterJobSelect(job);
        DisconnectHandler.Instance.RefreshDisconnectedPlayer(job.jobName);
        gameObject.SetActive(false);

    }
}
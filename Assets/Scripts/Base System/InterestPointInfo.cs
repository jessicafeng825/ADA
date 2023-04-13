using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine;
using TMPro;


public enum ipType
{
    Clue, Puzzle, Item
}
public class InterestPointInfo : MonoBehaviour
{
    
    [SerializeField]
    public Memory memory;

    [SerializeField]
    private List<InterestPoint> collectableList;

    private List<bool> collectableBool = new List<bool>();

    [SerializeField]
    private int cnt_current;

    [System.Serializable]
    public class InterestPoint
    {
        public string id;
        public ipType type;
    }

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(SpawnPopup);
        cnt_current = collectableList.Count;
        for(int i = 0; i < collectableList.Count; i++)
        {
            collectableBool.Add(false);
        }
        
        // Debug.Log("Initialize IP count " + name + ": " + cnt_current);
        GetComponentInChildren<TMP_Text>().text = cnt_current.ToString();
    }
    public void SpawnPopup()
    {
        InvestigationManager.Instance.SwitchInterestPointActive(name, false);
        IPCollectPanel tempIPPanel = BaseUIManager.Instance.SpawnInterestPointPanel(this.gameObject.name, collectableBool);
        tempIPPanel.ipName = this.gameObject.name;
        for(int i = 0; i < collectableList.Count; i++)
        {
            int temp = i;
            if(collectableBool[i] == false)
            {
                switch(collectableList[i].type)
                {
                    case ipType.Clue:
                        tempIPPanel.AddFunctiontoChoiceButton(delegate{NewAddCollectable(temp);}, ResourceManager.Instance.GetClueInfoTitle(collectableList[i].id) ,i);
                        break;
                    case ipType.Puzzle:
                        tempIPPanel.AddFunctiontoChoiceButton(delegate{NewAddCollectable(temp);}, collectableList[i].id ,i);
                        break;
                    case ipType.Item:
                        
                        break;
                }
            }
                continue;
            
        }

        // NotificationScript tempNoti = BaseUIManager.Instance.SpawnNotificationPanel("Use Action Points?", "Use the action point for investigate the interest point?", 2, -1f);
        // tempNoti.AddFunctiontoYesButton(AddCollectable, false);
    }

    private void NewAddCollectable(int i)
    {
        if(playerController.Instance.currentAP <= 0)
        {
            BaseUIManager.Instance.SpawnNotificationPanel("No Action Point!", "You don't have any action point left!", 1, 3f);
            
            InvestigationManager.Instance.SwitchInterestPointActive(name, true);
            return;
        }
        else if (cnt_current > 0)
        {
            if (collectableList[i].type == ipType.Clue)
            {
                BaseUIManager.Instance.SpawnNotificationPanel("Clue Found!", "You have found a clue!", 1, 3f);
                InvestigationManager.Instance.AddCluePrefab(collectableList[i].id, memory);
                playerController.Instance.Change_currentAP(-1);
                BaseUIManager.Instance.CollectedThisRoundUI(collectableList[i].type, ResourceManager.Instance.GetClueInfoTitle(collectableList[i].id));
            }
            else if (collectableList[i].type == ipType.Puzzle)
            {
                BaseUIManager.Instance.SpawnNotificationPanel("Puzzle Found!", "You have found a puzzle!", 1, 3f);
                InvestigationManager.Instance.AddPuzzlePrefab(collectableList[i].id, memory);
                playerController.Instance.Change_currentAP(-1);
                BaseUIManager.Instance.CollectedThisRoundUI(collectableList[i].type, collectableList[i].id);
            }
                
            // If the current clue is the last one, inactivate the interest point
            if (cnt_current == 1)
            {
                InvestigationManager.Instance.SynchronizeInterestPointStatus(name, memory);
            }

            // Tell im to synchronize
            InvestigationManager.Instance.SynchronizeInterestPoint(name, i);
            
            InvestigationManager.Instance.SwitchInterestPointActive(name, true);
        }
    }



    // private void AddCollectable()
    // {
    //     if(playerController.Instance.currentAP <= 0)
    //     {
    //         Debug.Log("No AP");
    //         return;
    //     }
    //     if (cnt_current > 0)
    //     {
    //         if (collectableList[collectableList.Count - cnt_current].type == ipType.Clue)
    //         {
    //             BaseUIManager.Instance.SpawnNotificationPanel("Clue Found!", "You have found a clue!", 1, 3f);
    //             InvestigationManager.Instance.AddCluePrefab(collectableList[collectableList.Count - cnt_current].id, memory);
    //         }
    //         else if (collectableList[collectableList.Count - cnt_current].type == ipType.Puzzle)
    //         {
    //             BaseUIManager.Instance.SpawnNotificationPanel("Puzzle Found!", "You have found a puzzle!", 1, 3f);
    //             InvestigationManager.Instance.AddPuzzlePrefab(collectableList[collectableList.Count - cnt_current].id);
    //         }
                

    //         // If the current clue is the last one, inactivate the interest point
    //         if (cnt_current == 1)
    //         {
    //             InvestigationManager.Instance.SynchronizeInterestPointStatus(name, memory);
    //         }

    //         // Tell im to synchronize
    //         InvestigationManager.Instance.SynchronizeInterestPoint(name);
    //     }
    // }

    public void changeIP_Current(int change)
    {
        cnt_current -= change;
        // Debug.Log(name + ": " + cnt_current);
        GetComponentInChildren<TMP_Text>().text = cnt_current.ToString();
    }
    public void itemCollected(int i)
    {
        if(collectableBool.Count > i)
            collectableBool[i] = true;
        // Debug.Log("Item Collected");
    }
}

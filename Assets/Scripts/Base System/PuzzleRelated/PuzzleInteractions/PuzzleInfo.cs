using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PuzzleInfo : MonoBehaviour
{
    
    [field: SerializeField]
    public Memory collectedAt
    {get; set;}

    [SerializeField]
    protected string puzzleID;
    [SerializeField]
    protected bool isSolved;
    [SerializeField]
    protected List<PuzzleEffect> puzzleEffects = new List<PuzzleEffect>();
    [SerializeField]
    protected List<string> clueProvided = new List<string>();
    [SerializeField]
    protected string unlockedRoom;

    [SerializeField]
    protected Memory unlockedMemory;

    // Transfer Puzzles Part
    [SerializeField]
    private GameObject TransferMenu;

    public enum PuzzleEffect
    {
        provideClue, unlockRoom, unlockMemory
    }

    protected virtual void PuzzleSolveEffect()
    {
        foreach(PuzzleEffect effect in puzzleEffects)
        {
            switch(effect)
            {
                case PuzzleEffect.provideClue:
                    // give clue
                    foreach(string clue in clueProvided)
                    {
                        InvestigationManager.Instance.AddCluePrefab(clue, collectedAt);
                    }

                    BaseUIManager.Instance.SpawnNotificationPanel("New Clue!", "You got " + clueProvided.Count + " new clues!", 1, -1f);
                    break;

                case PuzzleEffect.unlockRoom:
                    // unlock area
                    InvestigationManager.Instance.UnlockDoor(collectedAt, unlockedRoom);
                    BaseUIManager.Instance.SpawnNotificationPanel("New Room Unlocked", "The room " + unlockedRoom + " is unlocked!", 1, -1f);
                    break;

                case PuzzleEffect.unlockMemory:
                    // unlock memory, teleport player
                    InvestigationManager.Instance.UnlockMemoryInOverview(unlockedMemory);
                    // TODO: A small bug to fix: right now the teleport is from 1 -> unlocked memory
                    InvestigationManager.Instance.UnlockTeleport(collectedAt, unlockedMemory);
                    BaseUIManager.Instance.SpawnNotificationPanel("New Area Unlocked", "You are now able to navigate to <b>" + unlockedMemory.ToString() + "</b> in investigation phase!", 1, -1f);
                    break;
            }
        }
        
    }

    public void OpenTransferSelectionMenu()
    {
        TransferMenu.gameObject.SetActive(true);

        GameObject[] playerList = GameObject.FindGameObjectsWithTag("Player");
        int cnt = 1;

        foreach (var player in playerList)
        {
            // If the player job is not current player's job or the host's job
            if (player.GetComponent<playerController>().playerJob != playerController.Instance.playerJob && player.GetComponent<playerController>().playerJob != "None")
            {
                TransferMenu.GetComponentsInChildren<Button>()[cnt].GetComponentInChildren<TMPro.TMP_Text>().text = player.GetComponent<playerController>().playerJob;
                TransferMenu.GetComponentsInChildren<Button>()[cnt].onClick.AddListener(() => TransferThisPuzzle(player.GetComponent<playerController>().playerJob));
                cnt++;
            }
        }
    }

    private void TransferThisPuzzle(string playerJob)
    {
        if (isSolved)
        {
            BaseUIManager.Instance.SpawnNotificationPanel("Puzzle Already Solved", "This puzzle is already solved!", 1, -1f);
        }
        else
        {
            NotificationScript tempNoti = BaseUIManager.Instance.SpawnNotificationPanel("Transfer Puzzles", "Are you sure you want to transfer this puzzle?", 2, -1f);
            tempNoti.AddFunctiontoYesButton(() => ExecutePuzzleTransfer(playerJob), true);
        }
    }

    private void ExecutePuzzleTransfer(string playerJob)
    {
        BaseUIManager.Instance.SpawnNotificationPanel("Transfer Completed", "The puzzle is transferred to " + playerJob, 1, -1f);
        BaseUIManager.Instance.HidePuzzleUI();
        BaseUIManager.Instance.RemovePuzzleBtns(puzzleID);
        Destroy(gameObject);
        InvestigationManager.Instance.TransferPuzzleSynchronize(puzzleID, playerJob, collectedAt);
        // Inform receiver get a puzzle
    }

    protected void HideThisUI()
    {
        gameObject.SetActive(false);
        BaseUIManager.Instance.HidePuzzleUI();
    }

    public string GetPuzzleID()
    {
        return puzzleID;
    }

}

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
                    break;

                case PuzzleEffect.unlockRoom:
                    // unlock area
                    InvestigationManager.Instance.UnlockDoor(collectedAt, unlockedRoom);
                    break;

                case PuzzleEffect.unlockMemory:
                    // unlock memory, teleport player
                    InvestigationManager.Instance.UnlockMemoryInOverview(unlockedMemory);
                    // TODO: A small bug to fix: right now the teleport is from 1 -> unlocked memory
                    InvestigationManager.Instance.UnlockTeleport(collectedAt, unlockedMemory);
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
        InvestigationManager.Instance.TransferPuzzleSynchronize(puzzleID, playerJob, collectedAt);
        Destroy(gameObject);
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

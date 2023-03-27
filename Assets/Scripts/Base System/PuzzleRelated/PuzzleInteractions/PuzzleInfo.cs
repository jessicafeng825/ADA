using System.Collections;
using System.Collections.Generic;
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

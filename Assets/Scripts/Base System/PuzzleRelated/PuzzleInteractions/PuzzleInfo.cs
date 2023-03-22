using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleInfo : MonoBehaviour
{
    [SerializeField]
    protected string puzzleID;
    [SerializeField]
    protected bool isSolved;
    [SerializeField]
    protected PuzzleEffect puzzleEffect;
    [SerializeField]
    protected string clueProvided;
    [SerializeField]
    protected int unlockedArea;
    [SerializeField]
    protected Memory collectedAt, unlockedMemory;

    public enum PuzzleEffect
    {
        provideClue, unlockArea, unlockMemory
    }

    protected virtual void PuzzleSolveEffect()
    {
        switch (puzzleEffect)
        {
            case PuzzleEffect.provideClue:
                // give clue
                InvestigationManager.Instance.AddCluePrefab(clueProvided);
                break;

            case PuzzleEffect.unlockArea:
                // unlock area
                break;

            case PuzzleEffect.unlockMemory:
                // unlock memory, teleport player
                InvestigationManager.Instance.UnlockMemoryInOverview(unlockedMemory);
                // TODO: A small bug to fix: right now the teleport is from 1 -> unlocked memory
                InvestigationManager.Instance.UnlockTeleport(collectedAt, unlockedMemory);
                break;
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

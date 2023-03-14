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
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleInfo : MonoBehaviour
{
    [SerializeField]
    protected string puzzleName;
    [SerializeField]
    protected bool isSolved;
    [SerializeField]
    protected PuzzleEffect puzzleEffect;
    [SerializeField]
    protected string clueProvided;
    [SerializeField] 
    protected int areaUnlocked, memoryUnlocked;

    public enum PuzzleEffect
    {
        provideClue, unlockArea, unlockMemory
    }
}

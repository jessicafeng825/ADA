using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleInfo : MonoBehaviour
{
    [SerializeField]
    protected string puzzleName;
    [SerializeField]
    protected bool isSolved, provideClue, unlockArea;
    [SerializeField]
    protected string clueProvided, areaUnlocked;
}

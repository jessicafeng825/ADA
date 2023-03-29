using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineInfo : MonoBehaviour
{
    private string firstClueID;
    private string secondClueID;

    public string GetFirstClueID()
    {
        return firstClueID;
    }

    public string GetSecondClueID()
    {
        return secondClueID;
    }

    public void SetFirstClueID(string givenClueID)
    {
        firstClueID = givenClueID;
    }

    public void SetSecondClueID(string givenClueID)
    {
        secondClueID = givenClueID;
    }
}

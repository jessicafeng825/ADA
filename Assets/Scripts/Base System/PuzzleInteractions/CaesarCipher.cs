using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class CaesarCipher : MonoBehaviour
{
    [SerializeField]
    private Button submitBtn;
    [SerializeField]
    private TMPro.TMP_InputField enteredAnswer;
    [SerializeField]
    private string answer;

    private bool SubmitAnswer()
    {
        if (answer == enteredAnswer.text)
        {
            return true;
        }
        else return false;
    }
}

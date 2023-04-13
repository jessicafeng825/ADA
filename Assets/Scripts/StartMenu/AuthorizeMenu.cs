using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class AuthorizeMenu : MonoBehaviour
{
    private bool startCheck;
    [SerializeField]
    private TMP_Text loadingText;
    [SerializeField]
    private GameObject loadingEffect;

    public void OnClick()
    {
        if (!startCheck)
        {
            startCheck = true;
            loadingEffect.SetActive(true);
            StartCoroutine(AuthorizeCheck());
        }
    }

    private IEnumerator AuthorizeCheck()
    {
        yield return StartCoroutine(TypingSentence("Connecting to MemTech Server..."));
        yield return StartCoroutine(TypingSentence("Connection Established"));
        yield return StartCoroutine(TypingSentence("Authenticating user credentials..."));
        yield return StartCoroutine(TypingSentence("User Authenticated"));
        yield return StartCoroutine(TypingSentence("Authorized"));
    }

    private  IEnumerator TypingSentence(string sentence)
    {
        foreach (char letter in sentence.ToCharArray())
        {
            loadingText.text += letter;
            yield return new WaitForSeconds(0.05f);
        }

        yield return new WaitForSeconds(0.1f);
        loadingText.text += "<br><br>";
        yield return new WaitForSeconds(1f);
    }

}

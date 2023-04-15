using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class AuthorizeMenu : MonoBehaviour
{
    private bool effectPlayed;
    [SerializeField]
    private TMP_Text loadingText, welcomeText;
    [SerializeField]
    private GameObject dataFlowEffect;
    [SerializeField]
    private Launcher launcher;

    private void OnEnable()
    {
        GetComponent<Animator>().Play("FadeIn");
        StartCoroutine(AuthorizeCheck());
    }

    public void OnClick()
    {
        if (!effectPlayed)
        {
            effectPlayed = true;
            dataFlowEffect.SetActive(true);
            StartCoroutine(AuthorizeCheck());
        }
    }

    private IEnumerator AuthorizeCheck()
    {
        yield return StartCoroutine(TypingSentence("Connecting to MemTech Server...", loadingText));
        yield return StartCoroutine(TypingSentence("Connection Established", loadingText));
        yield return StartCoroutine(TypingSentence("Authenticating user credentials...", loadingText));
        yield return StartCoroutine(TypingSentence("User Authenticated", loadingText));
        yield return StartCoroutine(TypingSentence("Authorized", loadingText));
        yield return new WaitForSeconds(0.5f);
        loadingText.GetComponent<Animator>().Play("FadeOut");
        yield return new WaitForSeconds(1f);
        welcomeText.GetComponent<Animator>().Play("FadeIn");
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(WelcomeMessage());
        yield return new WaitForSeconds(2f);
        GetComponent<Animator>().Play("FadeOut");
        yield return new WaitForSeconds(1f);
        MenuManager.Instance.OpenMenu("find_room");
        // show room availability
    }

    private IEnumerator WelcomeMessage()
    {
        yield return StartCoroutine(TypingSentence("Welcome, ", welcomeText));
        yield return StartCoroutine(TypingSentence(launcher.GetPlayerNickname(), welcomeText));
        yield return new WaitForSeconds(0.5f);
    }

    private  IEnumerator TypingSentence(string sentence, TMP_Text textHolder)
    {
        foreach (char letter in sentence.ToCharArray())
        {
            textHolder.text += letter;
            yield return new WaitForSeconds(0.03f);
        }

        yield return new WaitForSeconds(0.03f);
        textHolder.text += "<br><br>";
        yield return new WaitForSeconds(0.2f);
    }

}

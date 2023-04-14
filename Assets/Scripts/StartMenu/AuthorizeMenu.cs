using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class AuthorizeMenu : MonoBehaviour
{
    private bool startCheck;
    [SerializeField]
    private TMP_Text loadingText, welcomeText;
    [SerializeField]
    private GameObject dataFlowPanel, welcomePanel, loadingEffect;
    [SerializeField]
    private Launcher launcher;

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
        yield return StartCoroutine(TypingSentence("Connecting to MemTech Server...", loadingText));
        yield return StartCoroutine(TypingSentence("Connection Established", loadingText));
        yield return StartCoroutine(TypingSentence("Authenticating user credentials...", loadingText));
        yield return StartCoroutine(TypingSentence("User Authenticated", loadingText));
        yield return StartCoroutine(TypingSentence("Authorized", loadingText));
        yield return new WaitForSeconds(0.5f);
        //LoadingTXTFadeOut();
        dataFlowPanel.GetComponent<Animator>().Play("FadeOut");
        yield return new WaitForSeconds(1f);
        ShowPlayerWelcome();
        yield return new WaitForSeconds(2f);
        GetComponent<Animator>().Play("FadeOut");
        yield return new WaitForSeconds(1f);
        launcher.ConnectToServer();
    }

    private void ShowPlayerWelcome()
    {
        welcomePanel.SetActive(true);
        welcomePanel.GetComponent<Animator>().Play("FadeIn");
        StartCoroutine(WelcomeMessage());
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

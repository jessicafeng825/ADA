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
    private GameObject loadingEffect;
    [SerializeField]
    private Launcher launcher;

    private bool press;

    private bool authorized;

    private void OnEnable()
    {
        GetComponent<Animator>().Play("FadeIn");
    }

    public void OnClick()
    {
        if (!effectPlayed)
        {
            effectPlayed = true;
            StartCoroutine(AuthorizeCheck());
        }
    }
    public void OnButtonPress()
    {
        press = true;
        if(!authorized)
            StartCoroutine(AuthorizeCheck());
    }
        
    public void OnButtonRelease()
    {
        if(!authorized)
        {
            press = false;
            StopAllCoroutines();
            loadingText.text = "";
            loadingEffect.SetActive(false);
        }
        else{
            press = true;
        }
    }
    private IEnumerator AuthorizeCheck()
    {
        loadingEffect.SetActive(true);
        yield return StartCoroutine(TypingSentence("Connecting to MemTech Server...", loadingText));
        yield return StartCoroutine(TypingSentence("Connection Established", loadingText));
        yield return StartCoroutine(TypingSentence("Authenticating user credentials...", loadingText));
        yield return StartCoroutine(TypingSentence("User Authenticated", loadingText));
        yield return StartCoroutine(TypingSentence("Authorized", loadingText));

        authorized = true;
        yield return new WaitForSeconds(0.5f);
        loadingText.GetComponent<Animator>().Play("FadeOut");
        yield return new WaitForSeconds(1f);
        welcomeText.GetComponent<Animator>().Play("FadeIn");
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(WelcomeMessage());
        yield return new WaitForSeconds(1f);
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
            if(!press)
            {
                yield break;
            }
        }

        yield return new WaitForSeconds(0.03f);
        textHolder.text += "<br><br>";
        yield return new WaitForSeconds(0.2f);
    }

    public void SkipPhoneAnimation()
    {
        MenuManager.Instance.OpenMenu("find_room");
    }

}

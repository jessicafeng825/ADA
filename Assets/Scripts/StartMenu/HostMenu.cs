using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class HostMenu : MonoBehaviour
{
    [SerializeField]
    private Launcher launcher;
    [SerializeField]
    private GameObject loadingEffect, settingServerEffect;
    [SerializeField]
    private TMP_Text loadingText;

    private void OnEnable()
    {
        GetComponent<Animator>().Play("FadeIn");
        StartCoroutine(SettingServer());
    }

    private IEnumerator SettingServer()
    {
        
        yield return new WaitForSeconds(1f);
        loadingEffect.GetComponent<Animator>().Play("FadeOut");
        yield return new WaitForSeconds(1f);
        settingServerEffect.SetActive(true);
        settingServerEffect.GetComponent<Animator>().Play("FadeIn");
        yield return new WaitForSeconds(1f);
        yield return TypingSentence("Setting up MemTech server.....", loadingText);
        yield return TypingSentence("MemTech server set up finished, spent: 0.8s", loadingText);
        yield return TypingSentence("Loading memory pieces......", loadingText);
        yield return TypingSentence("Memory pieces loaded 6/6", loadingText);
        yield return TypingSentence("Host Connecting...", loadingText);
        yield return new WaitForSeconds(0.5f);
        GetComponent<Animator>().Play("FadeOut");
        yield return new WaitForSeconds(1f);
        launcher.CreateRoom();
    }

    private IEnumerator TypingSentence(string sentence, TMP_Text textHolder)
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

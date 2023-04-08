using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField]
    private AudioSource BGM_Source, SFX_Source;

    private void Start()
    {
        BGM_Source = GetComponents<AudioSource>()[0];
        SFX_Source = GetComponents<AudioSource>()[1];
    }

    public void PlayBGM(string bgmName)
    {
        if (playerController.Instance.IsMasterClient() && bgmName == "Discussion")
        {
            BGM_Source.GetComponent<AudioSource>().clip = ResourceManager.Instance.GetBGM(bgmName);
            BGM_Source.Play();
        }
    }
}

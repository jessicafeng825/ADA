using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using TMPro;

public class OutroVideoPlay : MonoBehaviour
{
    // Start is called before the first frame update
    public VideoPlayer myVideoPlayer;

    [SerializeField]
    private string outroVideoname;


    
    void Start()
    {
        
        if(AccusationManager.Instance.solvedTheCase)
        {
            outroVideoname = "SuccessNewsReportCaptioned";
            myVideoPlayer.playbackSpeed = 1;
        }
        else
        {
            outroVideoname = "Bruh";
            myVideoPlayer.playbackSpeed = 1;
        }
        string videoUrl = Application.streamingAssetsPath + "/" + outroVideoname + ".mp4";
        myVideoPlayer.url = videoUrl;
        myVideoPlayer.Play();
    }
}

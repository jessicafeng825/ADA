using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoPlay : MonoBehaviour
{
    // Start is called before the first frame update
    public VideoPlayer myVideoPlayer;
    public string videoname;
    void Awake()
    {
        if(MenuManager.Instance.speedUpVid)
        {
            videoname = "Bruh";
            myVideoPlayer.playbackSpeed = 10;
        }
        string videoUrl = Application.streamingAssetsPath + "/" + videoname + ".mp4";
        myVideoPlayer.url = videoUrl;
        myVideoPlayer.Play();
    }
}

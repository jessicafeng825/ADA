using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoPlay : MonoBehaviour
{
    // Start is called before the first frame update
    public VideoPlayer myVideoPlayer;
    public string videoname;
    void Start()
    {
        string videoUrl = Application.streamingAssetsPath + "/" + videoname + ".mp4";
        myVideoPlayer.url = videoUrl;
    }
}

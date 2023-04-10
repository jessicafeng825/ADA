using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoPlay : MonoBehaviour
{
    // Start is called before the first frame update
    public VideoPlayer myVideoPlayer;
    [SerializeField]
    private string worldVideoname;

    [SerializeField]
    private string memVideoname;

    
    void Awake()
    {
        if(MenuManager.Instance.speedUpVid)
        {
            worldVideoname = "Bruh";
            myVideoPlayer.playbackSpeed = 10;
        }
        string videoUrl = Application.streamingAssetsPath + "/" + worldVideoname + ".mp4";
        myVideoPlayer.url = videoUrl;
        myVideoPlayer.Play();
    }

    public void PlayMemVid()
    {
        if(MenuManager.Instance.speedUpVid)
        {
            memVideoname = "Bruh";
            myVideoPlayer.playbackSpeed = 10;
        }
        UIManager.Instance.ifMemVideoStart = true;
        string videoUrl = Application.streamingAssetsPath + "/" + memVideoname + ".mp4";
        myVideoPlayer.url = videoUrl;
        myVideoPlayer.Play();
    }
}

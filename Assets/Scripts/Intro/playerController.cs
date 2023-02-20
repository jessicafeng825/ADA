using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using TMPro;
using Photon.Realtime;
using System.Linq;
using UnityEngine.UI;

public class playerController : MonoBehaviour
{
    public static playerController Instance;

    public PlayerManagerForAll.gamestage stageNow = PlayerManagerForAll.gamestage.Intro;//playerstage now
    [SerializeField] public float playerSpeed = 2.0f;//player icon
    [SerializeField] public string playerJob = "None";//player job

    
    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(stageNow);
        


    }
    public void introtoInvest()
    {
        stageNow = PlayerManagerForAll.gamestage.Investigate;
    }
    public void investtoDicuss()
    {
        stageNow = PlayerManagerForAll.gamestage.Dissussion;
    }
    public void jobSelect(string job)
    {
        playerJob = job;
    }

    public void discusstoInvest()
    {
        stageNow = PlayerManagerForAll.gamestage.Investigate;
    }
    public void discusstoAccuse()
    {
        
    }
}

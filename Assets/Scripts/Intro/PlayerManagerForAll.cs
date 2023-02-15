using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PlayerManagerForAll : MonoBehaviour
{
    public enum gamestage
    {
        Intro,
        Investigate,
        Dissussion
    };
    
    private PhotonView pv;

    private void Awake()
    {
        pv = GetComponent<PhotonView>();

        
        
    }

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
        if (pv.IsMine)
        {
            //create an player instance
            CreatePlayerInstance();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CreatePlayerInstance()
    {
        PhotonNetwork.Instantiate(Path.Combine("PlayerSetting", "PlayerController"), Vector3.zero, Quaternion.identity);
        //PhotonNetwork.Instantiate(Path.Combine("PlayerSetting", "Canvas"), Vector3.zero, Quaternion.identity);

    }
}

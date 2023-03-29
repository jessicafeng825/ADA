using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;

public class JOb : MonoBehaviour
{
    public string playerImage;
    public string jobName;
    public string playername;

    [TextArea(5, 20)]
    public string brief;

    [TextArea(15, 20)]
    public string backgroundstory;

    [TextArea(15, 20)]
    public string skilltext;

    [TextArea(15, 20)]
    public string relationshiptext;

    [TextArea(15, 20)]
    public string alibitext;

    public bool isselected = false;
    private Button button;
    // Start is called before the first frame update
    void Start()
    {
        button = this.GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void select()
    {
        isselected = true;
        if (!PhotonNetwork.IsMasterClient)
        {
            button.interactable = false;
        }
        
    }
    public void unSelect()
    {
        isselected = false;
        if (!PhotonNetwork.IsMasterClient)
        {
            button.interactable = true;
        }
        
    }
}

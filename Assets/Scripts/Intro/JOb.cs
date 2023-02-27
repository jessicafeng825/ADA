using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class JOb : MonoBehaviour
{
    public string playerImage;
    public string jobName;
    public string playername;
    public string backgroundstory;
    public string skilltext;
    public string relationshiptext;
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
        button.interactable = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AccusationScript : MonoBehaviour
{
    [SerializeField]
    private List<Button> playerButtons;
    static private Button selectedPlayer;
    public void SelectPlayer(Button b)
    {
        if(selectedPlayer != null)
            selectedPlayer.GetComponent<Image>().color = Color.white;
        b.GetComponent<Image>().color = Color.grey;
        selectedPlayer = b;
    }
    
    public void AccusePlayer()
    {
        if(selectedPlayer == null)
        {
            return;
        }
        else
        {
            foreach(Button button in playerButtons)
            {
                button.interactable = false;
            }
            selectedPlayer.GetComponent<Image>().color = Color.black;
            selectedPlayer.transform.GetChild(0).GetComponent<TMP_Text>().color = Color.white;
        }
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BiohackerHack : PuzzleInfo
{

    [SerializeField]
    private GameObject hintText;

    [SerializeField]
    private GameObject leftLockCircle;
    [SerializeField]
    private GameObject rightLockCircle;

    [SerializeField]
    private GameObject lockedScreen;

    

    [SerializeField]
    private GameObject hackButton;

    [SerializeField]
    private GameObject unlockButton;
    
    [SerializeField]
    private GameObject failedText;

    [SerializeField]
    private Animator hackAnim;
    private bool hacked;


    [SerializeField]
    private List<string> clueID = new List<string>();
    private Dictionary<string, bool> clueIDCollected = new Dictionary<string, bool>();

    private void Start()
    {
        this.transform.Find("Btn_close").GetComponent<Button>().onClick.AddListener(HideThisUI);
        foreach(string clue in clueID)
        {
            clueIDCollected.Add(clue, false);
        }
    }
    void OnEnable()
    {
        failedText.SetActive(false);
        unlockButton.SetActive(true);
        if(!CheckCharacter("Biohacker"))
        {
            hackButton.SetActive(true);
        }    
        else if(playerController.Instance.playerJob != "Biohacker")
        {
            hackButton.SetActive(false);
        }
        else
        {
            hackButton.SetActive(true);
        }
    }
    
    public void HackStart()
    {
        hackAnim.SetFloat("Speed", 0.1f);
        hacked = true;
    }

    public void OpenLock()
    {
        float result = Mathf.Abs(leftLockCircle.transform.localRotation.eulerAngles.z) % 180f;
        if(!hacked)
        {
            StartCoroutine(DisableUnlcokButtonTimer(2f));
            hackAnim.SetTrigger("FailTrigger");
        }
        else if(result <= 10f || result >= 170f)
        {            
            InvestigationManager.Instance.UpdatePuzzleBtnSolved(puzzleID);
            hackAnim.SetTrigger("OpenTrigger");
            hintText.SetActive(false);
        }
        else
        {
            StartCoroutine(DisableUnlcokButtonTimer(2f));
            hackAnim.SetTrigger("FailTrigger");
        }
        
    }
    

    public void CollectClueButton(string clueID)
    {
        if(!clueIDCollected[clueID])
        {
            BaseUIManager.Instance.SpawnNotificationPanel("New Clue!", "You got 1 new clues!", 1, -1f);
            InvestigationManager.Instance.AddCluePrefab(clueID, collectedAt);
            clueIDCollected[clueID] = true;
        }
        foreach(KeyValuePair<string, bool> clue in clueIDCollected)
        {
            if(!clue.Value)
            {
                return;
            }
        }
        // Hide UI, Mark this puzzle with "solved";
        InvestigationManager.Instance.UpdatePuzzleBtnSolved(puzzleID);
        isSolved = true;
        
    }

    IEnumerator DisableUnlcokButtonTimer(float time)
    {
        unlockButton.SetActive(false);
        failedText.SetActive(true);
        yield return new WaitForSeconds(time);
        failedText.SetActive(false);
        unlockButton.SetActive(true);
    }
    
}

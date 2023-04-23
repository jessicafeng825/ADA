using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DynamicPasscode : PuzzleInfo
{

    [SerializeField]
    private GameObject hintText;

    [SerializeField]
    private GameObject dynamicNumber;

    [SerializeField]
    private Color normalColor;

    [SerializeField]
    private Color wrongColor;


    [SerializeField]
    private GameObject decodeButton;

    private Animator decodeAnim;

    private bool decoding;
    private void Start()
    {
        decodeAnim = transform.GetComponent<Animator>();
        this.transform.Find("Btn_close").GetComponent<Button>().onClick.AddListener(HideThisUI);
    }
    
    public void DecodeStart()
    {
        if(!isSolved && !decoding)
        {
            StartCoroutine(DecodeTimer(1.5f));
        }
            
        
    }
    void OnEnable()
    {
        decodeButton.GetComponent<Image>().color = normalColor;
        hintText.GetComponent<TextMeshProUGUI>().text = "A dynamic passcode on the wall that has a socket for something to plug in...";

    }

    IEnumerator DecodeTimer(float time)
    {
        decoding = true;
        if(!BaseUIManager.Instance.CheckHavePuzzle("Decoder"))
        {
            hintText.GetComponent<TextMeshProUGUI>().text = "Some kind of device may be needed to solve this...";
            decodeButton.GetComponent<Image>().color = wrongColor;
            dynamicNumber.GetComponent<TextMeshProUGUI>().text = "000000000";
            yield return new WaitForSeconds(2f);
            decodeButton.GetComponent<Image>().color = normalColor;
            hintText.GetComponent<TextMeshProUGUI>().text = "A dynamic passcode on the wall that has a socket for something to plug in...";
            decoding = false;
            yield break;
        }
        else if(!BaseUIManager.Instance.CheckDecoderUnlocked("Decoder"))
        {
            hintText.GetComponent<TextMeshProUGUI>().text = "The Decoder is not avtivated yet...";
            decodeButton.GetComponent<Image>().color = wrongColor;
            dynamicNumber.GetComponent<TextMeshProUGUI>().text = "000000000";
            yield return new WaitForSeconds(2f);
            decodeButton.GetComponent<Image>().color = normalColor;
            hintText.GetComponent<TextMeshProUGUI>().text = "A dynamic passcode on the wall that has a socket for something to plug in...";
            decoding = false;
            yield break;
        }
        decodeAnim.SetTrigger("DecodeTrigger");
        yield return new WaitForSeconds(0.5f);
        float decodeTimer = 0f;
        while(decodeTimer < time)
        {
            dynamicNumber.GetComponent<TextMeshProUGUI>().text = Random.Range(0, 999999999).ToString("000000000");
            decodeTimer += Time.deltaTime;
            yield return null;
        }
        hintText.GetComponent<TextMeshProUGUI>().text = "ACTIVATED";
        
        InvestigationManager.Instance.UpdatePuzzleBtnSolved(puzzleID);
        PuzzleSolveEffect();
        isSolved = true;
        
        decoding = false;

        
    }
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class ResourceManager : Singleton<ResourceManager>
{
    private Dictionary<string, GameObject> clueInfoDic = new Dictionary<string, GameObject>();
    private Dictionary<string, Sprite> clueBtnPicsDic = new Dictionary<string, Sprite>();
    private GameObject tempClueBtn;

    public int allClueCount;
    private Dictionary<string, GameObject> puzzleBtnDic = new Dictionary<string, GameObject>();
    private Dictionary<string, GameObject> puzzleInteractionDic = new Dictionary<string, GameObject>();

    private Dictionary<string, GameObject> uiElements = new Dictionary<string, GameObject>();

    private Dictionary<string, AudioClip> SFX_Dic = new Dictionary<string, AudioClip>();
    private Dictionary<string, AudioClip> BGM_Dic = new Dictionary<string, AudioClip>();

    void Start()
    {
        LoadClueBtnTemplate();
        LoadAllClueInfos();
        LoadAllClueBtnPics();

        LoadAllPuzzleBtns();
        LoadAllPuzzleInteractions();

        LoadUIElements();

        LoadAllBGM();
    }

    #region Clue Related Resources

    private void LoadClueBtnTemplate()
    {
        tempClueBtn = Resources.Load<GameObject>("CluesRelated/clueBtnTemplate");
    }

    private void LoadAllClueBtnPics()
    {
        foreach (Sprite clueBtnPic in Resources.LoadAll<Sprite>("CluesRelated/CluePics/"))
        {
            clueBtnPicsDic.Add(clueBtnPic.name, clueBtnPic);
        }
    }

    public Sprite GetCluePic(string clueID)
    {
        return clueBtnPicsDic[clueID];
    }

    public GameObject GetClueBtn(string clueID)
    {
        tempClueBtn.GetComponent<Image>().sprite = clueBtnPicsDic[clueID];
        tempClueBtn.GetComponent<ClueBtn>().SetClueID(clueID);
        return tempClueBtn;
    }

    private void LoadAllClueInfos()
    {
        foreach (GameObject clueInfo in Resources.LoadAll("CluesRelated/ClueInfos/"))
        {
            clueInfoDic.Add(clueInfo.GetComponent<ClueInfo>().GetClueID(), clueInfo);
        }
    }

    public GameObject GetClueInfo(string clueID)
    {
        return clueInfoDic[clueID];
    }
    public string GetClueInfoTitle(string clueID)
    {
        return clueInfoDic[clueID].transform.Find("txt_ClueName").GetComponent<TextMeshProUGUI>().text;
    }
    #endregion

    #region Puzzle Related Resources
    private void LoadAllPuzzleBtns()
    {
        foreach (GameObject puzzlePrefab in Resources.LoadAll("PuzzlesRelated/PuzzleBtns/"))
        {
            puzzleBtnDic.Add(puzzlePrefab.GetComponent<PuzzleBtn>().GetPuzzleID(), puzzlePrefab);
        }
    }

    public GameObject GetPuzzleBtn(string puzzleID)
    {
        return puzzleBtnDic[puzzleID];
    }

    private void LoadAllPuzzleInteractions()
    {
        foreach (GameObject puzzleInteraction in Resources.LoadAll("PuzzlesRelated/PuzzleInteractions/"))
        {
            puzzleInteractionDic.Add(puzzleInteraction.GetComponent<PuzzleInfo>().GetPuzzleID(), puzzleInteraction);
        }
    }

    public GameObject GetPuzzleInteraction(string puzzleName)
    {
        return puzzleInteractionDic[puzzleName];
    }
    #endregion

    #region UI Related Resources
    //For instantiating UI elements
    private void LoadUIElements()
    {
        foreach (GameObject ui in Resources.LoadAll("UI/"))
        {
            uiElements.Add(ui.name, ui);
        }
    }
    public GameObject GetUIElement(string uiName)
    {
        return uiElements[uiName];
    }
    #endregion

    #region Sound Related Resources
    public void LoadAllBGM()
    {
        if (playerController.Instance.IsMasterClient())
        {
            foreach (AudioClip bgm in Resources.LoadAll("BGM/"))
            {
                BGM_Dic.Add(bgm.name, bgm);
            }
        }
    }

    public AudioClip GetBGM(string bgmName)
    {
        Debug.Log(bgmName);
        return BGM_Dic[bgmName];
    }


    public void LoadAllSFX()
    {
        foreach (AudioClip soundEffect in Resources.LoadAll("SFX/"))
        {

        }
    }

    #endregion
}

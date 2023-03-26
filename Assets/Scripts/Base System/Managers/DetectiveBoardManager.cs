using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using UnityEngine;


public class DetectiveBoardManager : Singleton<DetectiveBoardManager>
{
    private PhotonView pv;
    [SerializeField]
    private Canvas mainCanvas;
    [SerializeField]
    private GameObject detectiveBoard, clueBtnsOnBoard, linesBoard;
    [SerializeField]
    private GameObject clueOnBoardTemplate, onBoardClueInfoTemplate, linePrefab;
    private GameObject tempClueOnBoardBtn, tempClueOnBoardInfo, tempClueInfo, tempLine;
    private Dictionary<string, GameObject> OnBoardClueInfosDic = new Dictionary<string, GameObject>();

    // For Connecting Objects
    private string firstClueID, secondClueID;
    private Dictionary<string, GameObject> allCluesOnBoardDic = new Dictionary<string, GameObject>();

    private void Start()
    {
        pv = GetComponent<PhotonView>();
    }

    private void Update()
    {
/*        if (Input.GetMouseButtonDown(0) && PhotonNetwork.IsMasterClient)
        {
            //Debug.Log("click 1");
            // Connect Clues
            if (r_results[1].gameObject.GetComponent<ClueOnBoardDrag>())
            {
                if (!selectedOne)
                {
                    firstObj = r_results[1].gameObject;
                    selectedOne = true;
                }
                else
                {
                    secondObj = r_results[1].gameObject;
                    selectedOne = false;
                    lineRenderer.SetPosition(0, new Vector3(firstObj.transform.position.x, firstObj.transform.position.y, 1));
                    lineRenderer.SetPosition(1, new Vector3(secondObj.transform.position.x, secondObj.transform.position.y, 1));
                    // Create line prefab between 2 objects
                }
            }
        }*/

        /*        foreach (GameObject clueOnBoard in allCluesOnBoard)
                {
                    // if line dic contains id? line position 
                    // if (clueOnBoard.GetComponent<ClueOnBoardDrag>().GetClueID())
                }*/
    }

    public void ShareClue(string clueID)
    {
        pv.RPC(nameof(SynchronizeShareClue), RpcTarget.MasterClient, clueID);
    }

    [PunRPC]
    private void SynchronizeShareClue(string clueID)
    {
        Debug.Log("share");
        tempClueOnBoardBtn = Instantiate(clueOnBoardTemplate);
        tempClueOnBoardBtn.GetComponent<Image>().sprite = ResourceManager.Instance.GetCluePic(clueID);
        tempClueOnBoardBtn.GetComponent<ClueOnBoardDrag>().SetClueID(clueID);
        tempClueOnBoardBtn.GetComponent<ClueOnBoardDrag>().SetCanvas(mainCanvas);
        allCluesOnBoardDic.Add(clueID, tempClueOnBoardBtn);
        tempClueOnBoardBtn.GetComponent<Transform>().SetParent(clueBtnsOnBoard.GetComponent<Transform>(), false);
    }

    public void OpenClueInfoOnBoard(string clueID, Vector3 clueBtnPosition)
    {
        tempClueInfo = ResourceManager.Instance.GetClueInfo(clueID);
        tempClueOnBoardInfo = Instantiate(onBoardClueInfoTemplate);
        // Title
        tempClueOnBoardInfo.GetComponentsInChildren<TMP_Text>()[0].text = tempClueInfo.GetComponentsInChildren<TMP_Text>()[0].text;
        // Description
        tempClueOnBoardInfo.GetComponentsInChildren<TMP_Text>()[1].text = tempClueInfo.GetComponentsInChildren<TMP_Text>()[1].text;
        // Image
        tempClueOnBoardInfo.GetComponentsInChildren<Image>()[1].sprite = tempClueInfo.GetComponentsInChildren<Image>()[1].sprite;
        tempClueOnBoardInfo.GetComponentInChildren<Button>().onClick.AddListener(() => CloseClueInfoOnBoard(clueID));
        // Put on canvas
        tempClueOnBoardInfo.GetComponent<Transform>().SetParent(detectiveBoard.GetComponent<Transform>(), false);
        tempClueOnBoardInfo.transform.position = clueBtnPosition;
        OnBoardClueInfosDic.Add(clueID, tempClueOnBoardInfo);
    }

    public void CloseClueInfoOnBoard(string clueID)
    {
        Destroy(OnBoardClueInfosDic[clueID]);
        OnBoardClueInfosDic.Remove(clueID);
        firstClueID = null;
    }

    public void ClueSelected(string clueID)
    {
        if (firstClueID == null)
        {
            firstClueID = clueID;
            Debug.Log("1st clue selected: " + clueID);
        }
        else if (secondClueID == null)
        {
            Debug.Log("2nd clue selected: " + clueID);
            secondClueID = clueID;
            // connect clues
            ConnectTwoClues(allCluesOnBoardDic[firstClueID].transform.position, allCluesOnBoardDic[secondClueID].transform.position);
        }
    }

    private void ConnectTwoClues(Vector2 firstCluePosition, Vector2 secondCluePosition)
    {
        Debug.Log("draw");
        tempLine = Instantiate(linePrefab);
        tempLine.GetComponent<Transform>().SetParent(linesBoard.GetComponent<Transform>(), true);
        tempLine.GetComponent<LineRenderer>().SetPosition(0, firstCluePosition);
        tempLine.GetComponent<LineRenderer>().SetPosition(1, secondCluePosition);
    }

    public void ActivateDetectiveBoard()
    {
        detectiveBoard.SetActive(true);
    }

    public void DeactivateDetectiveBoard()
    {
        detectiveBoard.SetActive(false);
    }
}

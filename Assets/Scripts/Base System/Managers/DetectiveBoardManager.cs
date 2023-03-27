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
    private GameObject clueOnBoardTemplate, onBoardClueInfoTemplate;
    private GameObject tempClueOnBoardBtn, tempClueOnBoardInfo, tempClueInfo;
    private Dictionary<string, GameObject> allCluesOnBoardDic = new Dictionary<string, GameObject>();
    private Dictionary<string, GameObject> OnBoardClueInfosDic = new Dictionary<string, GameObject>();

    // For Connecting Objects
    private string firstClueID, secondClueID;
    [SerializeField]
    private GameObject linePrefab;
    private GameObject tempLine;
    private List<GameObject> linesOnBoardList = new List<GameObject>();
    private Vector2 firstCluePosition, secondCluePosition, diff;
    private float angle, sign, width, midpointX, midpointY;
    [SerializeField]
    private float lineThickness;

    private void Start()
    {
        pv = GetComponent<PhotonView>();
    }

    private void Update()
    {
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
            //DrawTwoCluesConnection(allCluesOnBoardDic[firstClueID], allCluesOnBoardDic[secondClueID]);
        }
    }

    #region For Connecting lines
    private void DrawTwoCluesConnection(GameObject firstClue, GameObject secondClue)
    {
        Debug.Log("draw");
        firstCluePosition = firstClue.transform.position;
        secondCluePosition = secondClue.transform.position;
        tempLine = Instantiate(linePrefab, secondCluePosition, Quaternion.identity);

        // Calculate the line rotation
        diff = firstCluePosition - secondCluePosition;
        sign = (firstCluePosition.y < secondCluePosition.y) ? -1.0f : 1.0f;
        angle = Vector2.Angle(Vector2.right, diff) * sign;
        tempLine.transform.Rotate(0, 0, angle);

        // Calculate the line length
        width = Vector2.Distance(secondCluePosition, firstCluePosition);
        tempLine.GetComponent<RectTransform>().sizeDelta = new Vector2(width, lineThickness);

        // Calculate mid point position
        midpointX = secondCluePosition.x + (firstCluePosition.x - secondCluePosition.x) / 2;
        midpointY = secondCluePosition.y + (firstCluePosition.y - secondCluePosition.y) / 2;
        tempLine.transform.position = new Vector3(midpointX, midpointY, 0);

        // Update line info
        tempLine.GetComponent<LineInfo>().SetFirstClueID(firstClue.GetComponent<ClueOnBoardDrag>().GetClueID());
        tempLine.GetComponent<LineInfo>().SetSecondClueID(firstClue.GetComponent<ClueOnBoardDrag>().GetClueID());

        tempLine.GetComponent<Transform>().SetParent(linesBoard.GetComponent<Transform>(), true);
        linesOnBoardList.Add(tempLine);
    }

    public void UpdateLinesConnection(string clueID, Vector2 newPosition)
    {
        foreach (GameObject line in linesOnBoardList)
        {
            if (line.GetComponent<LineInfo>().GetFirstClueID() == clueID)
            {
                UpdateLineConnectionFirstPosition(line, newPosition);
            }
            else if (line.GetComponent<LineInfo>().GetSecondClueID() == clueID)
            {

            }
        }
    }

    private void UpdateLineConnectionFirstPosition(GameObject line, Vector2 newFirstPosition)
    {
        Vector2 secondPosition = allCluesOnBoardDic[line.GetComponent<LineInfo>().GetSecondClueID()].transform.position;
        
        // Update Rotation
        diff = newFirstPosition - secondPosition;
        sign = (newFirstPosition.y < secondPosition.y) ? -1.0f : 1.0f;
        angle = Vector2.Angle(Vector2.right, diff) * sign;
        line.transform.Rotate(0, 0, angle);

        // Update Length
        width = Vector2.Distance(secondPosition, newFirstPosition);
        line.GetComponent<RectTransform>().sizeDelta = new Vector2(width, line.GetComponent<RectTransform>().sizeDelta.y);

        // Update mid point position
        midpointX = secondPosition.x + (newFirstPosition.x - secondPosition.x) * 0.5f;
        midpointY = secondPosition.y + (newFirstPosition.y - secondPosition.y) * 0.5f;
        line.transform.position = new Vector3(midpointX, midpointY, 0);
    }
    #endregion

    public void ActivateDetectiveBoard()
    {
        detectiveBoard.SetActive(true);
    }

    public void DeactivateDetectiveBoard()
    {
        detectiveBoard.SetActive(false);
    }
}

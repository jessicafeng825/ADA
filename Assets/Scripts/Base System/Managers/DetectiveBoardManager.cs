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
    private GameObject detectiveBoard, clueBtnsOnBoard;
    [SerializeField]
    private GameObject clueOnBoardTemplate, onBoardClueInfoTemplate;
    private GameObject tempClueOnBoardBtn, tempClueOnBoardInfo, tempClueInfo;
    private Dictionary<string, GameObject> OnBoardClueInfosDic = new Dictionary<string, GameObject>();

    // For Raycast
    private GraphicRaycaster graphicRaycaster;
    private PointerEventData peData;
    [SerializeField]
    private EventSystem eventSystem;
    private List<RaycastResult> r_results;

    // For Connecting Objects
    private LineRenderer lineRenderer;
    private int lineCnt = 0;
    private bool selectedOne;
    private GameObject firstObj;
    private GameObject secondObj;

    private void Start()
    {
        pv = GetComponent<PhotonView>();
        graphicRaycaster = mainCanvas.GetComponent<GraphicRaycaster>();
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && PhotonNetwork.IsMasterClient)
        {
            // Move Clues
            peData = new PointerEventData(eventSystem);
            peData.position = Input.mousePosition;
            r_results = new List<RaycastResult>();
            graphicRaycaster.Raycast(peData, r_results);


            // Connect Clues
/*            if (r_results[1].gameObject.GetComponent<ClueOnBoardDrag>())
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
            }*/
            //Todo: Highlight selected
            //r_results[1].gameObject.GetComponent<Material>() =
        }
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

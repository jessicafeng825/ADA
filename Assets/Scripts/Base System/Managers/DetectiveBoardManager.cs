using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;


public class DetectiveBoardManager : Singleton<DetectiveBoardManager>
{
    private PhotonView pv;
    [SerializeField]
    private Canvas mainCanvas;
    [SerializeField]
    private GameObject detectiveBoard;
    [SerializeField]
    private GameObject clueOnBoardTemplate;
    private GameObject tempClue;

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
        tempClue = Instantiate(clueOnBoardTemplate);
        tempClue.GetComponent<Image>().sprite = ResourceManager.Instance.GetCluePic(clueID);
        tempClue.GetComponent<ClueOnBoardDrag>().SetClueID(clueID);
        tempClue.GetComponent<ClueOnBoardDrag>().SetCanvas(mainCanvas);
        tempClue.GetComponent<Transform>().SetParent(detectiveBoard.GetComponent<Transform>(), false);
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

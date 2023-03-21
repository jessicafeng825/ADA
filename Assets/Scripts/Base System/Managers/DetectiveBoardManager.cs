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
    private EventSystem eventSystem;
    private List<RaycastResult> r_results;

    // For Connecting Objects
    private bool selectedOne;
    private GameObject firstObj;
    private GameObject secondObj;

    private void Start()
    {
        pv = GetComponent<PhotonView>();
        graphicRaycaster = mainCanvas.GetComponent<GraphicRaycaster>();
        eventSystem = GetComponent<EventSystem>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            peData = new PointerEventData(eventSystem);
            peData.position = Input.mousePosition;
            r_results = new List<RaycastResult>();
            graphicRaycaster.Raycast(peData, r_results);

            //Debug.Log(r_results[1].gameObject.GetComponent<ClueOnBoardDrag>().GetClueID());

            foreach (RaycastResult result in r_results)
            {
                Debug.Log(result.gameObject.GetComponentsInChildren<ClueOnBoardDrag>()[0].GetClueID());
            }

            if (!selectedOne)
            {
                firstObj = r_results[0].gameObject;
                selectedOne = true;
            }
            else
            {
                secondObj = r_results[0].gameObject;
                selectedOne = false;
                // Create line prefab between 2 objects

            }

            //Todo: Highlight selected
            //r_results[0].gameObject.GetComponent<Material>() =
            
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

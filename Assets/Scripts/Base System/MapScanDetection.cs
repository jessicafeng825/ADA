using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MapScanDetection : MonoBehaviour,  IPointerClickHandler
{
    #region Parameters: InterestPoint Scan

    [SerializeField]
    private GameObject normalMapScanner;

    [SerializeField]
    private GameObject largeMapScanner;
    private GameObject Scanner;
    
    [SerializeField]
    private float scanDistance;
    
    private bool isScanning;

    #endregion
    // Start is called before the first frame update
    void Start()
    {
        if(playerController.Instance.playerJob == "Security Guard")
        {
            Scanner = largeMapScanner;
        }
        else
        {
            Scanner = normalMapScanner;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    

    public void OnPointerClick(PointerEventData data)
    {
        if(!isScanning)
        {
            
            StartCoroutine(StartScanWaitSecond(data.position, 1.5f));
        }
    }
    IEnumerator StartScanWaitSecond(Vector3 scanPos, float second)
    {
        scanDistance = Scanner.GetComponent<RectTransform>().rect.height / 2f * playerController.Instance.currentRoom.transform.lossyScale.x;
        isScanning = true;        
        GameObject scanTemp = Instantiate(Scanner, scanPos, Quaternion.identity);
        scanTemp.transform.SetParent(playerController.Instance.currentRoom.transform);
        scanTemp.transform.localScale = new Vector3(1, 1, 1);
        foreach(InterestPointInfo IP in playerController.Instance.currentMemory.GetComponent<MemoryInfo>().interestPoints)
        {
            if(IP.locatedRoom == playerController.Instance.currentRoom)
            {
                if(Vector3.Distance(IP.transform.position, scanPos) <= scanDistance && IP.gameObject.activeSelf)
                {
                    IP.GetComponent<InterestPointInfo>().ShowInterestPointOnScan();
                }
            }
        }
        yield return new WaitForSeconds(second);
        isScanning = false;
    }
}

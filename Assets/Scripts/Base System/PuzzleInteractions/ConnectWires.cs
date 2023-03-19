using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ConnectWires : PuzzleInfo
{
    [SerializeField]
    private GameObject[] points;
    [SerializeField]
    private LineRenderer lineRenderer;
    [SerializeField]
    private float mouseTolerance;
    [SerializeField]
    private Button closeBtn;

    private bool startConnect;

    private void Start()
    {
        closeBtn.onClick.AddListener(HideThisUI);
        lineRenderer.positionCount = points.Length;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
/*            Debug.Log("pressed");
            if (OnInitialPoint())
            {
                Debug.Log("instart");
                startConnect = true;
            }*/
            startConnect = true;
            if (startConnect)
            {
                Debug.Log("started");
                lineRenderer.SetPosition(0, points[0].transform.position);
                lineRenderer.SetPosition(1, Input.mousePosition);
            }
        }
        else
        {
/*            startConnect = false;
            Debug.Log("released");*/
        }
    }

    private bool OnInitialPoint()
    {
        return InRegion();
    }

    private bool InRegion()
    {
        Vector2 mouseP = Input.mousePosition;
        Vector2 initialPointP = points[0].transform.position;
        if (mouseP.x < initialPointP.x + mouseTolerance
            && mouseP.x > initialPointP.x - mouseTolerance
            && mouseP.y < initialPointP.y + mouseTolerance
            && mouseP.y > initialPointP.y - mouseTolerance)
        {
            return true;
        }
        return false;
    }
}

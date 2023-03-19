using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectWires : PuzzleInfo
{
    [SerializeField]
    private GameObject[] points;
    [SerializeField]
    private LineRenderer lineRenderer;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && OnInitialPoint())
        {
            Debug.Log("start");
            lineRenderer.SetPosition(0, points[0].transform.position);
            lineRenderer.SetPosition(1, Input.mousePosition);
        }
    }

    private bool OnInitialPoint()
    {
        return Input.mousePosition == points[0].transform.position;
    }
}

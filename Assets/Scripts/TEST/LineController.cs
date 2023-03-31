using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineController : MonoBehaviour
{
    private LineRenderer lr;
    private List<Transform> points = new List<Transform>();
    // Start is called before the first frame update
    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
    }
    void Start()
    {
        

    }
    public void SetupLine(List<Transform> points)
    {
        lr.positionCount = points.Count;
        this.points = points;
    }
    // Update is called once per frame
    void Update()
    {
        for(int i = 0;i < points.Count; i++)
        {
            lr.SetPosition(i, points[i].position);
        }
    }
}

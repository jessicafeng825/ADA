using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class lineTest : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private List<Transform> points;
    public GameObject objadd;
    [SerializeField] private LineController line;
    public Canvas canvasui;
    private Camera cam;
    enum RenderModeStates { camera, overlay, world };
    RenderModeStates m_RenderModeStates;
    void Start()
    {
        line.SetupLine(points);
        canvasui = GetComponent<Canvas>();
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ChangeState();
            
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            Transform trans = objadd.transform;
            if (!points.Find(x => x.transform == objadd.transform))
            {
                points.Add(objadd.transform);
                line.SetupLine(points);
                Debug.Log("ADDD");
            }
            
        }
    }
    void ChangeState()
    {
        Debug.Log("change to ScreenSpaceCamera");
        canvasui.renderMode = RenderMode.ScreenSpaceCamera;
        canvasui.worldCamera = cam;
    }
}

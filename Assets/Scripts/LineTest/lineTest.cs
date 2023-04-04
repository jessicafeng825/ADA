using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class lineTest : Singleton<lineTest>
{
    // Start is called before the first frame update
    [SerializeField] private List<Transform> points;
    [SerializeField] private GameObject p1;
    [SerializeField] private GameObject p2;
    [SerializeField] List<Transform> p;

    public List<List<GameObject>> lindic = new List<List<GameObject>>();
    private List<GameObject> linetemp = new List<GameObject>();
    public GameObject objadd;
    [SerializeField] private LineController line;
    public Canvas canvasui;
    private Camera cam;
    enum RenderModeStates { camera, overlay, world };
    RenderModeStates m_RenderModeStates;

    public GameObject obj1;
    public GameObject obj2;
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
        if (p.Count == 2)
        {
              if(!findifinDic(p1,p2) && p1 && p2)
              {

                LineController linenew = Instantiate(line);
                //linedic.Add(p1, p2);
                if(linetemp != null) linetemp.Clear();
                linetemp.Add(p1);
                linetemp.Add(p2);
                lindic.Add(linetemp);
                List<Transform> pnew = new List<Transform>(p);
                drawLineIntwoPoint(linenew, pnew);
              }

            Debug.Log(lindic.Count);



        }
        /*
        if (Input.GetKeyDown(KeyCode.A))
        {
            Transform trans = objadd.transform;
            if (!points.Find(x => x.transform == objadd.transform))
            {
                points.Add(objadd.transform);
                line.SetupLine(points);
                Debug.Log("ADDD");
            }
         
        }*/
    }
    bool findifinDic(GameObject p1,GameObject p2)
    {
        for(int i = 0; i < lindic.Count; i++)
        {
            if (lindic[i][0] == p1 && lindic[i][1] == p2) return true;
            if (lindic[i][1] == p1 && lindic[i][0] == p2) return true;

        }
        return false;
    }
    void ChangeState()
    {
        Debug.Log("change to ScreenSpaceCamera");
        canvasui.renderMode = RenderMode.ScreenSpaceCamera;
        canvasui.worldCamera = cam;
    }
    public void addOnlyTwoPoints(GameObject trans)
    {
        //points.Add(trans);
        if (p1 != null && p2 != null)
        {//clear up
            p.Clear();
            p1 = null;
            p2 = null;
            p1 = trans;
            p.Add(p1.transform);
        }

        if (p1 == null && p2 == null)
        {
            p1 = trans;
            p.Add(p1.transform);
        }

        if (p1 != trans && p2 == null)
        {
            p2 = trans;
            p.Add(p2.transform);
            
        }

        

    }
    public void selectandConnectTwo(GameObject j1)
    {
        if(obj1 == null && obj2 == null)
        {
            points.Add(j1.transform);
        }

        if (obj1 != j1 && obj2 != null)
        {
            points.Add(j1.transform);
            line.SetupLine(points);
            points.Clear();
        }
    }
    public void drawLineIntwoPoint(LineController linenew, List<Transform> p)
    {
        linenew.SetupLine(p);

    }
}
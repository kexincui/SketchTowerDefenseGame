using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using UnityEngine;
using PDollarGestureRecognizer;
using System;

public class sketchBox : MonoBehaviour
{
    private Vector3 position;
    private Vector3 s;
    private float xmax;
    private float xmin;
    private float ymax;
    private float ymin;

    private bool isCollecting;
    private bool isBuilt;
    private bool isTimeout;
    private float timeout;
    private float maxTimeout;

    private List<List<Vector2>> Strokes;
    private List<Point> candidate_points;
    private int currentIndex = 0;

    public GameObject line;
    private List<GameObject> Lines;
    private LineRenderer lineRenderer;
    private GameObject border;

    private GameObject CurrentTower;
    private SketchBoxesManagement SBM;

    private Vector2 origin;
    private float start = 0.0f;


    // Start is called before the first frame update
    void Start()
    {
        position = transform.position;
        s = transform.localScale;
        xmax = position.x + s.x / 2.0f;
        xmin = position.x - s.x / 2.0f;
        ymax = position.y + s.y / 2.0f;
        ymin = position.y - s.y / 2.0f;
        origin = (Vector2)(transform.position - transform.localScale / 2.0f);

        isCollecting = false;
        isTimeout = false;
        isBuilt = false;

        SBM = GetComponentInParent<SketchBoxesManagement>();

        maxTimeout = SBM.timeout;
        timeout = maxTimeout;

        Strokes = new List<List<Vector2>>
        {
            new List<Vector2>()
        };
        Lines = new List<GameObject>
        {
            Instantiate(line, Vector3.zero, Quaternion.identity, transform)
        };
        lineRenderer = Lines[currentIndex].GetComponent<LineRenderer>();
        candidate_points = new List<Point>();

        border = transform.Find("Border").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (isCollecting)
        {
            try
            {
                Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                if (InBox(pos))
                {
                    Strokes[currentIndex].Add(pos);
                    candidate_points.Add(new Point(pos.x - origin.x, pos.y - origin.y, currentIndex + 1));
                    lineRenderer.positionCount++;
                    lineRenderer.SetPosition(lineRenderer.positionCount - 1, pos);
                }
                else if (Strokes[currentIndex].Count > 0)
                {
                    Strokes.Add(new List<Vector2>());
                    Lines.Add(Instantiate(line, Vector3.zero, Quaternion.identity, transform));
                    currentIndex++;
                    lineRenderer = Lines[currentIndex].GetComponent<LineRenderer>();
                }

                if (Input.GetMouseButton(0))
                {

                }
                else if (Input.GetMouseButtonUp(0))
                {
                    isCollecting = false;
                    isTimeout = true;
                    if (Strokes[currentIndex].Count > 0)
                    {
                        Strokes.Add(new List<Vector2>());
                        Lines.Add(Instantiate(line, Vector3.zero, Quaternion.identity, transform));
                        currentIndex++;
                        lineRenderer = Lines[currentIndex].GetComponent<LineRenderer>();
                    }
                }

            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
            
        }
        //Debug.Log(currentIndex);
        if (isTimeout)
        {
            timeout -= Time.deltaTime;
            if (timeout < 0)
            {
                isBuilt = true;
                isTimeout = false;
                isCollecting = false;

                // writeToCSV();
                //Debug.Log(candidate_points.Count);
                //GameObject line_g = Instantiate(line, Vector3.zero, Quaternion.identity);
                //lineRenderer = line_g.GetComponent<LineRenderer>();
                //for (int i = 0; i < candidate.Points.Length; i ++)
                //{
                //    Vector2 pos_g = new Vector2(candidate.Points[i].X, candidate.Points[i].Y);
                //    lineRenderer.positionCount++;
                //    lineRenderer.SetPosition(lineRenderer.positionCount - 1, pos_g);
                //}
                
                Tuple<GameObject, float> tower = SBM.Gesture_Classify(candidate_points.ToArray(), Time.time - start - maxTimeout);
                
                if (tower == null)
                {
                    isBuilt = false;
                    Strokes = new List<List<Vector2>>
                    {
                        new List<Vector2>()
                    };
                    foreach (var l in Lines)
                    {
                        Destroy(l);
                    }
                    Lines = new List<GameObject>
                    {
                        Instantiate(line, Vector3.zero, Quaternion.identity, transform)
                    };
                    currentIndex = 0;
                    lineRenderer = Lines[currentIndex].GetComponent<LineRenderer>();
                    candidate_points = new List<Point>();
                    timeout = maxTimeout;

                } else
                {
                    border.SetActive(false);
                    foreach (var l in Lines)
                    {
                        l.SetActive(false);
                    }
                    CurrentTower = Instantiate(tower.Item1, transform);
                    CurrentTower.GetComponent<Tower>().Damage *= tower.Item2;
                }
            }
        }
    }

    public bool IsContained(Vector2 pos)
    {
        if (pos.x >= xmin && pos.x <= xmax && pos.y >= ymin && pos.y <= ymax)
        {
            if (!isBuilt)
            {
                if (!isTimeout)
                {
                    start = Time.time;
                }
                isCollecting = true;
                isTimeout = false;
                timeout = maxTimeout;
            }
            return true;
        }
        return false;
    }

    private bool InBox(Vector2 pos)
    {
        if (pos.x >= xmin && pos.x <= xmax && pos.y >= ymin && pos.y <= ymax)
        {
            return true;
        }
        return false;
    }

    private void writeToCSV() 
    {
        string strFilePath = @"C:\\Users\\Kexin Cui\\Desktop\\624Project\\CSCE624\\Assets\\Data\\stroke_1.csv";
        //string strSeperator = ",";
        StringBuilder sbOutput = new StringBuilder();

        for (int i = 0; i < Strokes.Count - 1; i ++) {
            for (int j = 0; j < Strokes[i].Count; j ++) {
                Vector2 ele = Strokes[i][j];
                ele -= origin;
                sbOutput.AppendLine(ele.ToString("f6").TrimEnd(')').TrimStart('('));
            }
            sbOutput.AppendLine("");
        }
 
        // Create and write the csv file
        File.WriteAllText(strFilePath, sbOutput.ToString());
        
        // To append more lines to the csv file
        //File.AppendAllText(strFilePath, sbOutput.ToString());
    }
}

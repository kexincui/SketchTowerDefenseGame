using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PDollarGestureRecognizer;
using System;
using System.IO;

public class SketchBoxesManagement : MonoBehaviour
{

    public bool isInputEnabled = true;
    public float timeout;
    public List<GameObject> objs;

    private GameObject[] Boxes;

    private Gesture[] trainingSet;
    private string[] towersType = { "butterfly", "music", "moon" };

    public float similarity=0;
    public float numGesture = 0;
    public float length=0;
    public float time = 0;


    //public GameObject line;
    //private GameObject lineInstance;
    //private LineRenderer lineRenderer;

    // Start is called before the first frame update
    void Start()
    {
        Boxes = GameObject.FindGameObjectsWithTag("sketchBox");

        trainingSet = new Gesture[towersType.Length];
        // TODO: Add training set.
        for (int i = 0; i < towersType.Length; i++)
        {
            using (var reader = new StreamReader(Application.dataPath + "/Data/CleanData/" + towersType[i] + ".csv"))
            {
                List<Point> points = new List<Point>();
                int strokeId = 0;
                while (!reader.EndOfStream)
                {
                    var textline = reader.ReadLine();
                    var values = textline.Split(',');
                    if (values[0].Length == 0 || values[1].Length == 0)
                    {
                        strokeId++;
                        continue;
                    }
                    points.Add(new Point(float.Parse(values[0]), float.Parse(values[1]), strokeId));

                }
                trainingSet[i] = new Gesture(points.ToArray(), towersType[i]);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && isInputEnabled)
        {
            // Debug.Log(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            foreach (GameObject item in Boxes)
            {
                if (item.GetComponent<sketchBox>().IsContained(pos))
                {
                    break;
                }
            }
        }
    }

    public Tuple<GameObject, float> Gesture_Classify(Point[] candidate_points, float t)
    {
        Debug.Log(t);
        Gesture candidate = new Gesture(candidate_points);
        length += candidate.PathLength(candidate_points);
        time += t;
        numGesture++;
        Tuple<GameObject, float> tower = null;
        Tuple<string, float> result = PointCloudRecognizer.Classify(candidate, trainingSet);
        similarity += result.Item2;
        Debug.Log(result);
        if (result.Item2 < 0.7) return null;

        for (int i = 0; i < objs.Count; i++)
        {
            if (result.Item1 == towersType[i])
            {
                tower = new Tuple<GameObject, float>(objs[i], result.Item2);
                break;
            }
        }
        return tower;
    }
}

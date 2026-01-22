using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class SplineLogic : MonoBehaviour
{
    public SplineContainer splineContainer;
    public List<GameObject> waypoints;

    BezierKnot knot;
    void Awake()
    {
        splineContainer = GetComponent<SplineContainer>();
        waypoints = FindAnyObjectByType<WaypointOrder>().Waypoints;
        for (int i = 0; i <= waypoints.Count - 1; i++)
        {
            knot = new BezierKnot(waypoints[i].transform.position);
            splineContainer.Spline.Add(knot, TangentMode.AutoSmooth);
            //splineContainer.Spline.SetTangentMode(splineContainer.Spline.Count - 1, TangentMode.Continuous);
            //EVALUATEEEEEEE PLEASEEEEEEEEE

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class AITrack : MonoBehaviour
{

    public WaypointOrder waypointOrder;
    public SplineContainer splineContainer;

    public int step = 5;
    public float waypointTriggerRadius = 4f;
    public float yOffset = 0.5f;

    Transform waypointParent;
    ProceduralTrack track;

    void Awake()
    {
        track = GetComponent<ProceduralTrack>();
    }

    void OnEnable()
    {
        track.OnGenerated += Build;
    }

    void OnDisable()
    {
        track.OnGenerated -= Build;
    }

    void Build()
    {
        if (waypointOrder == null || splineContainer == null)
            return;

        var centerline = track.GetCenterlineLocal();
        if (centerline == null || centerline.Count < 2)
            return;

 
        if (waypointParent != null)
            Destroy(waypointParent.gameObject);

        waypointParent = new GameObject("AI_Waypoints").transform;
        waypointParent.SetParent(transform, false);

        waypointOrder.Waypoints.Clear();


        for (int i = 0; i < centerline.Count; i += Mathf.Max(1, step))
        {
            Vector3 pos = transform.TransformPoint(centerline[i]);
            pos.y += yOffset;

            GameObject wp = new GameObject($"WP_{waypointOrder.Waypoints.Count:000}");
            wp.transform.SetParent(waypointParent);
            wp.transform.position = pos;

            var col = wp.AddComponent<SphereCollider>();
            col.isTrigger = true;
            col.radius = waypointTriggerRadius;

            waypointOrder.Waypoints.Add(wp);
        }


        var spline = splineContainer.Spline;
        spline.Clear();

        foreach (var wp in waypointOrder.Waypoints)
            spline.Add(new BezierKnot(wp.transform.position), TangentMode.AutoSmooth);

        spline.Closed = true;


        if (waypointOrder.Waypoints.Count > 0)
            waypointOrder.Waypoints[^1].AddComponent<FinishTrigger>();
    }
}

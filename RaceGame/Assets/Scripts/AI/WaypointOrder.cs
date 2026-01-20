using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class WaypointOrder : MonoBehaviour
{

    public List<GameObject> Waypoints;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddCheckpoint(GameObject checkpoint)
    {
        Waypoints.Add(checkpoint);
    }
}

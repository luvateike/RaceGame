using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class WaypointOrder : MonoBehaviour
{

    public List<GameObject> Waypoints = new();

    public void AddCheckpoint(GameObject checkpoint)
    {
        Waypoints.Add(checkpoint);
    }
}

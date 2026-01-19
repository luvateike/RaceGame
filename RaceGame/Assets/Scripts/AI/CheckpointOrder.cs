using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointOrder : MonoBehaviour
{

    public List<GameObject> Checkpoints;

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
        Checkpoints.Add(checkpoint);
    }
}

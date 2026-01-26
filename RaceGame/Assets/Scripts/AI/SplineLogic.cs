using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class SplineLogic : MonoBehaviour
{
    public SplineContainer splineContainer;

    void Awake()
    {
        if (splineContainer == null)
            splineContainer = GetComponent<SplineContainer>();
    }
}

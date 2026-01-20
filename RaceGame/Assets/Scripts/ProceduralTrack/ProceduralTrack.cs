using System.Collections.Generic;
using UnityEngine;

public class ProceduralTrack : MonoBehaviour
{
    public int points = 64;
    public float radius = 50f;
    public float width = 10f;

    public List<Vector3> Centerline = new();

    void Start()
    {
        Generate();
    }

    void Generate()
    {
        Vector3[] center = new Vector3[points];

      
        for (int i = 0; i < points; i++)
        {
            float a = i * Mathf.PI * 2f / points;
            center[i] = new Vector3(Mathf.Cos(a) * radius, 0f, Mathf.Sin(a) * radius);
        }

        Centerline.Clear();
        Centerline.AddRange(center);

       
        Vector3[] verts = new Vector3[points * 2];
        int[] tris = new int[points * 6];

        for (int i = 0; i < points; i++)
        {
            int ip = (i - 1 + points) % points;
            int inx = (i + 1) % points;

            Vector3 tangent = (center[inx] - center[ip]).normalized;
            Vector3 perp = new Vector3(-tangent.z, 0f, tangent.x);

            verts[i * 2] = center[i] + perp * width * 0.5f;
            verts[i * 2 + 1] = center[i] - perp * width * 0.5f;
        }

        for (int i = 0; i < points; i++)
        {
            int n = (i + 1) % points;
            int v = i * 2;

            tris[i * 6 + 0] = v;
            tris[i * 6 + 1] = n * 2;
            tris[i * 6 + 2] = v + 1;
            tris[i * 6 + 3] = v + 1;
            tris[i * 6 + 4] = n * 2;
            tris[i * 6 + 5] = n * 2 + 1;
        }

        Mesh m = new Mesh();
        m.vertices = verts;
        m.triangles = tris;
        m.RecalculateNormals();

        GetComponent<MeshFilter>().mesh = m;
    }
}

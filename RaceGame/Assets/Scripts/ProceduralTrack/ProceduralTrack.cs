using System.Collections.Generic;
using UnityEngine;
using System;

public class ProceduralTrack : MonoBehaviour
{
    public int points = 200;
    public float radius = 60f;
    public float wiggleAmount = 0.15f;
    public float wiggleScale = 2f;
    public float trackWidth = 10f;

    public float wallHeight = 2f;
    public float wallOutset = 0.2f;

    public Transform car;
    public float spawnHeight = 1f;

    public List<Vector3> Centerline = new();

    public event Action OnGenerated;
    public IReadOnlyList<Vector3> GetCenterlineLocal() => Centerline;

    MeshFilter mf;
    MeshCollider mc;

    void Awake()
    {
        mf = GetComponent<MeshFilter>();

        mc = GetComponent<MeshCollider>();
        if (mc == null) mc = gameObject.AddComponent<MeshCollider>();
    }

    void Start()
    {
        Generate();
        PlaceCarAtStart();
    }

    public void Generate()
    {

        //Generate the centerline points
        Vector3[] center = new Vector3[points];

        for (int i = 0; i < points; i++)
        {
            float t = (float)i / points;
            float ang = t * Mathf.PI * 2f;

            float p = Mathf.PerlinNoise(
                Mathf.Cos(ang) * wiggleScale,
                Mathf.Sin(ang) * wiggleScale
            ) * 2f - 1f;

            float r = radius * (1f + p * wiggleAmount);
            center[i] = new Vector3(Mathf.Cos(ang) * r, 0f, Mathf.Sin(ang) * r);
        }

        //Saves centerline so AI can read it
        Centerline.Clear();
        Centerline.AddRange(center);

        float halfW = trackWidth * 0.5f;
        Vector3[] left = new Vector3[points];
        Vector3[] right = new Vector3[points];

        for (int i = 0; i < points; i++)
        {
            int ip = (i - 1 + points) % points;
            int inx = (i + 1) % points;

            Vector3 tangent = (center[inx] - center[ip]).normalized;
            Vector3 perp = new Vector3(-tangent.z, 0f, tangent.x);

            left[i] = center[i] + perp * halfW;
            right[i] = center[i] - perp * halfW;
        }

        int roadVertCount = points * 2;
        int leftWallStart = roadVertCount;
        int rightWallStart = roadVertCount + points * 2;

        Vector3[] verts = new Vector3[roadVertCount + points * 4];



        //Road Vertex Generation
        for (int i = 0; i < points; i++)
        {
            verts[i * 2] = left[i];
            verts[i * 2 + 1] = right[i];
        }

        Vector3 up = Vector3.up;


        //Wall Vertex Generation
        for (int i = 0; i < points; i++)
        {
            Vector3 outLeft = (left[i] - center[i]).normalized;
            Vector3 outRight = (right[i] - center[i]).normalized;

            Vector3 lb = left[i] + outLeft * wallOutset;
            verts[leftWallStart + i * 2] = lb;
            verts[leftWallStart + i * 2 + 1] = lb + up * wallHeight;

            Vector3 rb = right[i] + outRight * wallOutset;
            verts[rightWallStart + i * 2] = rb;
            verts[rightWallStart + i * 2 + 1] = rb + up * wallHeight;
        }

        int[] tris = new int[points * 18];
        int ti = 0;


        //Road Triangles
        for (int i = 0; i < points; i++)
        {
            int iNext = (i + 1) % points;

            int v0 = i * 2;
            int v1 = i * 2 + 1;
            int v2 = iNext * 2;
            int v3 = iNext * 2 + 1;

            tris[ti++] = v0; tris[ti++] = v2; tris[ti++] = v1;
            tris[ti++] = v1; tris[ti++] = v2; tris[ti++] = v3;
        }


        //Left wall triangles
        for (int i = 0; i < points; i++)
        {
            int iNext = (i + 1) % points;

            int b0 = leftWallStart + i * 2;
            int t0 = leftWallStart + i * 2 + 1;
            int b1 = leftWallStart + iNext * 2;
            int t1 = leftWallStart + iNext * 2 + 1;

            tris[ti++] = b0; tris[ti++] = t0; tris[ti++] = t1;
            tris[ti++] = b0; tris[ti++] = t1; tris[ti++] = b1;
        }


        //Right Wall Triangles
        for (int i = 0; i < points; i++)
        {
            int iNext = (i + 1) % points;

            int b0 = rightWallStart + i * 2;
            int t0 = rightWallStart + i * 2 + 1;
            int b1 = rightWallStart + iNext * 2;
            int t1 = rightWallStart + iNext * 2 + 1;

            tris[ti++] = b0; tris[ti++] = t1; tris[ti++] = t0;
            tris[ti++] = b0; tris[ti++] = b1; tris[ti++] = t1;
        }

        Mesh mesh = new Mesh();
        mesh.vertices = verts;
        mesh.triangles = tris;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        mf.sharedMesh = mesh;

        mc.sharedMesh = null;
        mc.sharedMesh = mesh;

        OnGenerated?.Invoke();
    }

   

    public void PlaceCarAtStart()
    {
        if (car == null || Centerline.Count < 2) return;

        Vector3 start = transform.TransformPoint(Centerline[0]);
        Vector3 next = transform.TransformPoint(Centerline[1]);

        Vector3 forward = next - start;
        forward.y = 0f;

        car.SetPositionAndRotation(
            start + Vector3.up * spawnHeight,
            Quaternion.LookRotation(forward.normalized, Vector3.up)
        );
    } 

}

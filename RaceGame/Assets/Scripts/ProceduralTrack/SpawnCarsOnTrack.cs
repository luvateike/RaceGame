using UnityEngine;

public class SpawnCarsOnTrack : MonoBehaviour
{
 
    public string playerTag = "Player";
    public float playerHeight = 1.0f;    

    ProceduralTrack track;

    void Awake()
    {
        track = GetComponent<ProceduralTrack>();
    }

    void Start()
    {
      
        StartCoroutine(SpawnNextFrame());
    }

    System.Collections.IEnumerator SpawnNextFrame()
    {
        yield return null;

        var player = GameObject.FindWithTag(playerTag);
        if (player != null)
            PlaceOnTrack(player.transform, 0, playerHeight);

    }

    void PlaceOnTrack(Transform t, int centerlineIndex, float height)
    {
        if (track.Centerline == null || track.Centerline.Count < 2) return;

        int count = track.Centerline.Count;
        int i0 = ((centerlineIndex % count) + count) % count;
        int i1 = (i0 + 1) % count;

        Vector3 p0 = transform.TransformPoint(track.Centerline[i0]);
        Vector3 p1 = transform.TransformPoint(track.Centerline[i1]);

        Vector3 forward = (p1 - p0);
        forward.y = 0f;
        if (forward.sqrMagnitude < 0.001f) forward = transform.forward;

        Vector3 pos = p0 + Vector3.up * height;
        Quaternion rot = Quaternion.LookRotation(forward.normalized, Vector3.up);

   
        var rb = t.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.position = pos;
            rb.rotation = rot;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.Sleep();
            rb.WakeUp();
        }
        else
        {
            t.SetPositionAndRotation(pos, rot);
        }
    }
}

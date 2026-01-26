using UnityEngine;
using System.Collections;

public class LoadCar : MonoBehaviour
{
    [SerializeField] private ProceduralTrack track;     
    [SerializeField] private int startIndex = 0;      
    [SerializeField] private float spawnHeight = 1.2f;

    [SerializeField] private CarLibrary library;
    public CarLibrary Library => library;

    void Awake()
    {
        if (track == null) track = FindAnyObjectByType<ProceduralTrack>();
    }

    IEnumerator Start()
    {
        if (library == null)
        {
          
            yield break;
        }

        
        while (track == null || track.Centerline == null || track.Centerline.Count < 2)
            yield return null;

      
        int count = track.Centerline.Count;
        int i0 = ((startIndex % count) + count) % count;
        int i1 = (i0 + 1) % count;

        Vector3 p0 = track.transform.TransformPoint(track.Centerline[i0]);
        Vector3 p1 = track.transform.TransformPoint(track.Centerline[i1]);

        Vector3 forward = (p1 - p0);
        forward.y = 0f;
        if (forward.sqrMagnitude < 0.001f) forward = track.transform.forward;

        Vector3 spawnPos = p0 + Vector3.up * spawnHeight;
        Quaternion spawnRot = Quaternion.LookRotation(forward.normalized, Vector3.up);

        // Pick selected car
        int selectedCar = PlayerPrefs.GetInt("selectedCar", 0);
        selectedCar = Mathf.Clamp(selectedCar, 0, library.Prefabs.Count - 1);

        Instantiate(library.Prefabs[selectedCar], spawnPos, spawnRot);
    }


  /*  private void Reset()
    {
#if UNITY_EDITOR
        // Try to auto-assign a default library asset by name.
        if (library != null) return;

        const string assetName = "CarLibrary_Default";
        var guids = UnityEditor.AssetDatabase.FindAssets($"{assetName} t:CarLibrary");
        if (guids.Length == 0) return;

        var path = UnityEditor.AssetDatabase.GUIDToAssetPath(guids[0]);
        library = UnityEditor.AssetDatabase.LoadAssetAtPath<CarLibrary>(path);
#endif
    } */
}
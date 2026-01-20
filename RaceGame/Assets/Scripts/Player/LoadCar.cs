using UnityEngine;

public class LoadCar : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;
    
    [SerializeField] private CarLibrary library;
    public CarLibrary Library => library;

    void Start()
    {
        if (spawnPoint == null)
            spawnPoint = transform;
        
        int selectedCar = 0;
        if (PlayerPrefs.HasKey("selectedCar"))
            selectedCar = PlayerPrefs.GetInt("selectedCar");
        
        Instantiate(library.Prefabs[selectedCar], spawnPoint.position, spawnPoint.rotation);
    }

    private void Reset()
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
    }
}
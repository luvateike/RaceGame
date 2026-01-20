using UnityEngine;

public class LoadCar : MonoBehaviour
{
    [SerializeField] private GameObject[] carPrefabs;
    [SerializeField] private Transform spawnPoint;

    void Start()
    {
        int selectedCar = PlayerPrefs.GetInt("selectedCar");
        Instantiate(carPrefabs[selectedCar], spawnPoint.position, spawnPoint.rotation);
    }
}
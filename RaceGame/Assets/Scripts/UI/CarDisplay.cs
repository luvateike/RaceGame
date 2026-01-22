using UnityEngine;

public class MainMenuC : MonoBehaviour
{
    [SerializeField] private GameObject[] mainMenuCars;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        RandomizeCars();

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void RandomizeCars()
    {
        
        int randomCarIndex = Random.Range(0, mainMenuCars.Length);
        
        Instantiate(mainMenuCars[randomCarIndex], transform.position, Quaternion.Euler(0f, -131f, 0f));
        
        
    }
}

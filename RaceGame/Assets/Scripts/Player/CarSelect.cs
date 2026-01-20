using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class CarSelect : MonoBehaviour
{
    [SerializeField] private GameObject[] cars;
    [SerializeField] private int selectedCar = 0;
    [SerializeField] private string sceneToLoad;
  
    public void NextCar()
    {
        cars[selectedCar].SetActive(false);
        selectedCar = (selectedCar + 1) % cars.Length;
        cars[selectedCar].SetActive(true);
    }

    public void PreviousCar()
    {
        cars[selectedCar].SetActive(false);
        selectedCar--;
        if(selectedCar < 0)
            selectedCar += cars.Length;
        cars[selectedCar].SetActive(true);
    }

    public void StartGame() 
    {
        PlayerPrefs.SetInt("selectedCar", selectedCar);
        SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Single);
    }
}

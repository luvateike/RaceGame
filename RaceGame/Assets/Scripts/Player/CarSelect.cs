using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CarSelect : MonoBehaviour
{
    [SerializeField] private GameObject[] cars;
    [SerializeField] private int selectedCar = 0;
    [SerializeField] private string sceneToLoad;
    
    [SerializeField] private TMPro.TMP_Text AccelerationValueText;
    [SerializeField] private TMPro.TMP_Text SpeedValueText;
    [SerializeField] private TMPro.TMP_Text TurnValueText;
    [SerializeField] private TMPro.TMP_Text WeightValueText;

    private void Start()
    {
        CarDataUpdate();
    }

    public void NextCar()
    {
        cars[selectedCar].SetActive(false);
        selectedCar = (selectedCar + 1) % cars.Length;
        cars[selectedCar].SetActive(true);
        cars[selectedCar].transform.position = new Vector3(0, .4f, 0);
    }

    public void PreviousCar()
    {
        cars[selectedCar].SetActive(false);
        selectedCar--;
        if(selectedCar < 0)
            selectedCar += cars.Length;
        cars[selectedCar].SetActive(true);
        cars[selectedCar].transform.position = new Vector3(0, .4f, 0);
    }

    public void StartGame() 
    {
        PlayerPrefs.SetInt("selectedCar", selectedCar);
        SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Single);
    }

    public void CarDataUpdate()
    {
        Movement movement = cars[selectedCar].GetComponent<Movement>();
        Rigidbody rb = cars[selectedCar].GetComponent<Rigidbody>();
        
        AccelerationValueText.SetText(movement.maxAcceleration.ToString("F0"));
        SpeedValueText.SetText(movement.maxSpeed.ToString("F0"));
        TurnValueText.SetText((movement.turnSensitivity * 100).ToString("F0"));
        WeightValueText.SetText(rb.mass.ToString("F0"));
    }
}

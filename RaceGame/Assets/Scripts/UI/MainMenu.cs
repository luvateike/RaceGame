using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    //[SerializeField] private Transform CarDisplay;
    public string firstLvlScene;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void StartGame()
    {
        
        SceneManager.LoadScene(firstLvlScene);
        
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit");
    }
}

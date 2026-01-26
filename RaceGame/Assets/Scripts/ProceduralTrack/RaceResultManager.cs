using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RaceResultManager : MonoBehaviour
{
    public static RaceResultManager Instance;

    public TMP_Text resultText;
    public GameObject RestartButton;

    public string CarSelectScene;

    bool finished = false;

    void Awake()
    {
        Instance = this;
        resultText.gameObject.SetActive(false);
        RestartButton.SetActive(false);
    }

    public void PlayerWin()
    {
        if (finished) return;
        finished = true;

        resultText.text = "YOU WIN!";
        ShowEndUI();
    }

    public void PlayerLose()
    {
        if (finished) return;
        finished = true;

        resultText.text = "YOU LOST!";
        ShowEndUI();
    }

    void ShowEndUI()
    {
        resultText.gameObject.SetActive(true);
        RestartButton.SetActive(true);
        StartCoroutine(FreezeNextFrame());
    }

    System.Collections.IEnumerator FreezeNextFrame()
    {
        yield return null;
        Time.timeScale = 0f;
    }

   
    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(CarSelectScene);
    }
}

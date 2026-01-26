using UnityEngine;

public class FinishTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (RaceResultManager.Instance == null) return;

        if (other.CompareTag("Player"))
        {
            RaceResultManager.Instance.PlayerWin();
        }
        else if (other.GetComponent<CarAI>() != null)
        {
            RaceResultManager.Instance.PlayerLose();
        }
    }
}

using UnityEngine;

public class CarAI : MonoBehaviour
{

    public Movement MovementScript;
    public int difficulty;
    public GameObject nextWaypoint;

    WaypointOrder Waypoints;
    float carSpeed = 10;
    float turnSpeed = 1f;

    int waypointNumber = 0;
    int numberOfWaypoints;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        MovementScript = GetComponent<Movement>();

    }

    private void Start()
    {
        Waypoints = FindAnyObjectByType<WaypointOrder>();
        nextWaypoint = Waypoints.Waypoints[0];
        numberOfWaypoints = Waypoints.Waypoints.Count;
    }
    // Update is called once per frame
    void FixedUpdate()
    {

        Vector3 dirToWaypoint = (nextWaypoint.transform.position - transform.position).normalized;
        float dot = Vector3.Dot(transform.forward, dirToWaypoint);
        if (dot > 0) {
            MovementScript.moveInput = carSpeed;
        }
        else
        {
            MovementScript.moveInput = -carSpeed;
        }
          
        float steerAngle = Vector3.SignedAngle(transform.forward, dirToWaypoint, Vector3.up);
        if (steerAngle > 0)
        {
            MovementScript.steerInput = turnSpeed;
        }
        else
        {
            MovementScript.steerInput = -turnSpeed;
        }
        
        MovementScript.isBraking = false;


    }

    public void NextWaypoint()
    {
        if (waypointNumber == numberOfWaypoints-1)
        {
            waypointNumber = 0;
            nextWaypoint = Waypoints.Waypoints[waypointNumber];
        }
        else
        {
            waypointNumber++;
            nextWaypoint = Waypoints.Waypoints[waypointNumber];
        }
        Debug.Log(waypointNumber + "out of " + numberOfWaypoints);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == nextWaypoint)
        {
            NextWaypoint();
            
        }
    }
}

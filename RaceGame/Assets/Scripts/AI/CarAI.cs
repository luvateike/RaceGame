 using UnityEngine;
 using UnityEngine.Splines;
 using Unity.Mathematics; 
public class CarAI : MonoBehaviour
{

    public Movement MovementScript;
    public int difficulty = 0;
    public GameObject nextWaypoint;
    public SplineContainer splineContainer;

    public float forwardSpeed;
    WaypointOrder Waypoints;
    float carSpeed = 10;
    float turnSpeed = 1f;

    //difficulty 1 (using Bezier curves)
    float splineCurrentPoint = 0f;     
    Rigidbody rb;
    float splineLength;
    float closestT;
    float3 nearestPos;
    float lookAheadT;
    float targetT;
    float steerAngle;
    float absAngle;

    int waypointNumber = 0;
    int numberOfWaypoints;
   
    void Awake()
    {
        MovementScript = GetComponent<Movement>();
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        Waypoints = FindAnyObjectByType<WaypointOrder>();
        nextWaypoint = Waypoints.Waypoints[0];
        numberOfWaypoints = Waypoints.Waypoints.Count;

        splineContainer = FindAnyObjectByType<SplineLogic>().splineContainer;
       // Debug.Log("SOLINE CONTAINER" + splineContainer.name);
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (difficulty == 0)
        {
            Vector3 dirToWaypoint = (nextWaypoint.transform.position - transform.position).normalized;
            float dot = Vector3.Dot(transform.forward, dirToWaypoint);
            if (dot > 0)
            {
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
        else if (difficulty == 1)
        {

            var spline = splineContainer.Spline;
            splineLength = spline.GetLength();

            

            SplineUtility.GetNearestPoint(
                spline,
                transform.position,
                out nearestPos,
                out closestT
            );

            // Magic number for ahead distance
             lookAheadT = 30f / splineLength;
           
             targetT = Mathf.Repeat(closestT + lookAheadT, 1f);
            Vector3 targetPos = spline.EvaluatePosition(targetT);

            Vector3 toTarget = (targetPos - transform.position).normalized;
            steerAngle = Vector3.SignedAngle(transform.forward, toTarget, Vector3.up);

            // Normalize to make it behave like player input 1 0 or -1
            MovementScript.steerInput = NormalizeInput(Mathf.Clamp(steerAngle / 30f, -1f, 1f));

            forwardSpeed = Vector3.Dot(rb.linearVelocity, transform.forward);
            absAngle = Mathf.Abs(steerAngle);

            if (absAngle < 8f)
            {
                MovementScript.moveInput = 1f;
                MovementScript.isBraking = false;
            }
            else if (absAngle < 20f)
            {
                MovementScript.moveInput = 1f;
                MovementScript.isBraking = false;
            }
            else if (forwardSpeed < 15f)
            {
                MovementScript.moveInput = 1f;
                MovementScript.isBraking = false;
            }
            else
            {
                MovementScript.moveInput = 0f;
                MovementScript.isBraking = false;
                //MovementScript.steerInput = - NormalizeInput(Mathf.Clamp(steerAngle / 30f, -1f, 1f));
            }

            // Debug
            Debug.DrawLine(transform.position, targetPos, Color.red);
        }

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
        if ( difficulty == 0 && other.gameObject == nextWaypoint )
        {
            NextWaypoint();
            
        }
    }

    private float NormalizeInput(float value)
    {
        if (value > 0.2f) return 1f;
        if (value < -0.2f) return -1f;
        return 0f;
    }
}

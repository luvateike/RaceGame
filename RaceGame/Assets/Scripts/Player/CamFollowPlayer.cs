using UnityEngine;

public class CamFollowPlayer : MonoBehaviour
{
    public string playerTag = "Player";


    public Vector3 offset = new Vector3(0f, 6f, -10f);

    public float positionSmooth = 5f;
    public float rotationSmooth = 5f;

    Transform target;

    void LateUpdate()
    {
        
        if (target == null)
        {
            var playerObj = GameObject.FindWithTag(playerTag);
            if (playerObj != null)
                target = playerObj.transform;
            else
                return;
        }

      
        Vector3 desiredPos = target.TransformPoint(offset);
        transform.position = Vector3.Lerp(
            transform.position,
            desiredPos,
            positionSmooth * Time.deltaTime
        );

    
        Quaternion desiredRot = Quaternion.LookRotation(
            target.position - transform.position,
            Vector3.up
        );

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            desiredRot,
            rotationSmooth * Time.deltaTime
        );
    }
}

using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    Vector3 target_offset;
    Vector3 lastPosition;

    public BallController ballController;

    void Start()
    {
        target_offset = transform.position-target.position;
        lastPosition = new Vector3(3.27f, 1.41f, 1.71f);
    }

    
    void LateUpdate()
    {
        if(!ballController.isFinish)
        transform.position=Vector3.Lerp(transform.position, target.position + target_offset, 1f);
        else
            transform.position = Vector3.Lerp(transform.position, lastPosition, 0.125f);
    }





}

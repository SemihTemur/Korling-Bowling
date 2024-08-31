using UnityEngine;


public class CameraControllerLeft : MonoBehaviour
{
    public Transform target;
    Vector3 target_offset;
    Vector3 lastPosition;

    public BallController2 ballController;

    Vector3 firsPosition;

    private void Awake()
    {
        firsPosition = new Vector3(25.8330002f, 4.23643589f, -1.97571766f);
    }

    void Start()
    {
        target_offset = transform.position - firsPosition;
        lastPosition = new Vector3(3.27f, 1.41f, 1.71f);
    }


    void LateUpdate()
    {
        if (ballController.isGround)
        {
            if (!ballController.isFinish)
                transform.position = Vector3.Lerp(transform.position, target.position + target_offset, 1f);
            else
                transform.position = Vector3.Lerp(transform.position, lastPosition, 0.125f);
        }
    }





}

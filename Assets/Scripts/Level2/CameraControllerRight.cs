using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class CameraControllerRight : MonoBehaviour
{
    public Transform target;
    Vector3 target_offset;
    Vector3 lastPosition;

    public BallController2 ballController;


  

    void Start()
    {
        target_offset = transform.position - (target.position+new Vector3(-0.05f,0f,0.2f));
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

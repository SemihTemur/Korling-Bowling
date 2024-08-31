using UnityEngine;

public class LabutController : MonoBehaviour
{
    private BallController ballController;
    private bool hasFallen;


    void Start()
    {
        ballController = GameObject.Find("ball").GetComponent<BallController>();
        hasFallen = false;
    }

  
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!hasFallen && other.gameObject.CompareTag("plane"))
        {
            Debug.Log("plane");
            ballController.score+=10;
            hasFallen = true;
        }
    }



}

using TMPro;
using UnityEngine;

public class Obstacles : MonoBehaviour
{
    private float obstacleSpeed = 0.1f;
    Vector3 obstaclePosition;
    Vector3 positionLeftRight;

    public TextMeshProUGUI obstaclesPoinText;


    private void Start()
    {
        obstaclePosition = transform.position;
        positionLeftRight = new Vector3(-1, 0, 0);
    }


    void Update()
    {

        if (transform.position.x == 2.8f)
        {
            transform.rotation = Quaternion.Euler(0f, 90f, 0f);
            positionLeftRight = new Vector3(1, 0, 0);
            // Pozisyonu ayarla
            obstaclesPoinText.transform.localPosition = new Vector3(-0.855f, obstaclesPoinText.transform.localPosition.y, obstaclesPoinText.transform.localPosition.z);

            // Rotasyonu ayarla
            obstaclesPoinText.transform.localRotation = Quaternion.Euler(-2f, 180f, 0f);
        }

        else if (transform.position.x == 4f)
        {
            transform.rotation = Quaternion.Euler(0f, -90f, 0f);
            positionLeftRight = new Vector3(-1, 0, 0);

            // Pozisyonu ayarla
            obstaclesPoinText.transform.localPosition = new Vector3(-0.05f, obstaclesPoinText.transform.localPosition.y, obstaclesPoinText.transform.localPosition.z);

            // Rotasyonu ayarla
            obstaclesPoinText.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);

        }

        obstaclePosition += positionLeftRight * obstacleSpeed * Time.deltaTime;

        obstaclePosition.x = Mathf.Clamp(obstaclePosition.x, 2.8f, 4f);

        transform.position = obstaclePosition;

    }


}

using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BallController2 : MonoBehaviour
{
    //ball speedvalues
    float ballSpeed = 6f;
    float maxSpeed = 15.0f;
    float minSpeed = 2f;

    float acceleration = 0.2f;

    public Rigidbody rb;

    private float moveHorizontal;
    private float moveVertical;

    Vector3 movement;

    public int score;

    public Text scoreText;

    public bool isFinish;
    public bool isSpace;

    public bool left;

    public GameObject Camera;
    public GameObject Camera2;

    public bool isGround;


    private void Awake()
    {
        scoreText.text = "Puan : " +PlayerPrefs.GetInt("Puan");
    }

    private void Start()
    {
        score = 0;
        isFinish = false;
        isSpace = false;
        left = true;
        isGround = false;
    }


    private void FixedUpdate()
    {
        if (!isFinish&&!isSpace)
            ApplyBallMovement(movement);
    }


    void Update()
    {
        if (!isFinish)
        {
            movement = GetMovementInput();
        }
        else
        {
            scoreText.text = "Puan : " + score;
        }
    }

    // take the �nputs
    public Vector3 GetMovementInput()
    {
        moveHorizontal = Input.GetAxis("Horizontal");
        moveVertical = Input.GetAxis("Vertical");

        return new Vector3(moveHorizontal, 0.0f, moveVertical);
    }


    public void ApplyBallMovement(Vector3 movement)
    {
        // solda ise top
        if (left == true)
        {

            // �leri ve geri hareketi kontrol et
            if (movement.z > 0f && ballSpeed < maxSpeed)
            {
                ballSpeed += acceleration;
            }
            else if (movement.z < 0f && ballSpeed > minSpeed)
            {
                ballSpeed -= acceleration;
            }

            if (movement.x != 0f)
            {

                rb.AddForce(Vector3.right * movement.x * ballSpeed * Time.fixedDeltaTime*2f, ForceMode.Acceleration);
            }

            // �leri do�ru kuvvet ekle
            rb.AddForce(Vector3.forward * ballSpeed * Time.fixedDeltaTime, ForceMode.Acceleration);
        }

        // sa�da ise top

        else
        {
            // �leri ve geri hareketi kontrol et
            if (movement.z > 0f && ballSpeed < maxSpeed)
            {
                ballSpeed += acceleration;
            }
            else if (movement.z < 0f && ballSpeed > minSpeed)
            {
                ballSpeed -= acceleration;
            }

            if (movement.x != 0f)
            {

                rb.AddForce(Vector3.back * movement.x * ballSpeed * Time.fixedDeltaTime*2f, ForceMode.Force);
            }

            // �leri do�ru kuvvet ekle
            rb.AddForce(Vector3.right * ballSpeed * Time.fixedDeltaTime, ForceMode.Force);
        }

    }


    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("labut"))
        {
            reverseForce(collision);
        }

        else if (collision.gameObject.CompareTag("mushroom"))
        {

            // Canvas alt�nda Text (TMP) bile�enini bul
            TextMeshProUGUI textComponent = collision.gameObject.GetComponentInChildren<TextMeshProUGUI>();

            if (textComponent != null)
            {
                // Text de�erini al
                string textValue = textComponent.text;

                // Regex kullanarak sadece say�sal k�sm� al
                string numericValue = Regex.Match(textValue, @"\d+").Value;

                // Numeric k�sm� integer'a �evir
                int intValue;

                if (int.TryParse(numericValue, out intValue))
                {
                    // Integer de�eri kullanarak sayac� artt�r
                    score += intValue;
                    Debug.Log("g�rd�");
                }
            }
            else
            {
                Debug.Log("�arp�lan objede Text (TMP) bile�eni bulunamad�.");
            }

            Destroy(collision.gameObject);
        }

        else if (collision.gameObject.CompareTag("badObstacles"))
        {
            if (collision.gameObject.transform.localPosition.x < 1)
            {
                rb.AddForce(Vector3.right * 10f, ForceMode.Impulse);
            }

            else
            {
                rb.AddForce(Vector3.left * 10f, ForceMode.Impulse);
            }
        }

        else if (collision.gameObject.CompareTag("ground"))
        {
            isGround = true;
        }



    }

    public void reverseForce(Collision collision)
    {
        Rigidbody collisionRb = collision.gameObject.GetComponent<Rigidbody>();

        // E�er �arp�lan cismin Rigidbody bile�eni varsa

        // �arp��man�n kuvvetini ve y�n�n� al
        Vector3 collisionForce = collision.impulse / Time.fixedDeltaTime;

        // Kuvvetin ters y�n�n� al
        Vector3 reverseForce = -collisionForce;

        // �arp��ma kuvvetini uygula
        collisionRb.AddForce(reverseForce * 0.01f, ForceMode.Impulse);

        rb.velocity *= 0.8f;

    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("line"))
        {
            isFinish = true;
            scoreText.text = "Puan : " + score;
        }

        else if (other.CompareTag("space"))
        {
            if (!isFinish)
            {
                score = 0;
                isSpace= true;
                scoreText.text = "Puan : " + score;
            }
        }

        else if (other.CompareTag("right"))
        {
            left = false;

            Camera.SetActive(false);
            Camera2.SetActive(true);
        }

        else if (other.CompareTag("left"))
        {
            left = true;

            Camera.SetActive(true);
            Camera2.SetActive(false);
        }

        else if (other.CompareTag("acceleration"))
        {
            if (!left)
            {
                rb.AddForce(Vector3.right * 3.2f, ForceMode.Impulse);
            }
            else
            {
                rb.AddForce(Vector3.forward * 3.2f, ForceMode.Impulse);
            }
        }

    }

   


}

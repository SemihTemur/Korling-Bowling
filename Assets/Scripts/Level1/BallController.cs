using System.Collections;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BallController : MonoBehaviour
{
    //ball speedvalues
    float ballSpeed = 7f;
    float maxSpeed = 17f;
    float minSpeed = 5f;

    float acceleration = 0.02f;

    public Rigidbody rb;

    private float moveHorizontal;
    private float moveVertical;

    Vector3 movement;

    public int score;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI missionStatusText;

    public bool isFinish;

    public bool isSpace;

    
    private void Start()
    {
        score = 0;
        isFinish = false;
        isSpace = false;
    }


    private void FixedUpdate()
    {
        if (!isFinish&&!isSpace)
            ApplyBallMovement(movement);
    }


    void Update()
    {
        // e�er oyun bitmediyse yani biti� �izgisine varmad�ysa topu kontrol etmeye devam et
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
        // �leri ve geri hareketi kontrol et
        if (movement.z > 0f && ballSpeed < maxSpeed)
        {
            ballSpeed += acceleration;
        }
        else if (movement.z < 0f && ballSpeed > minSpeed)
        {
            ballSpeed -= acceleration;
        }

        // Sa�a ve sola hareketi kontrol et
        Vector3 velocity = rb.velocity;
        if (movement.x != 0f)
        {
            Debug.Log("selamm");
            rb.AddForce(Vector3.right * movement.x * ballSpeed * Time.fixedDeltaTime, ForceMode.Impulse);
        }

        // �leri do�ru kuvvet ekle
        rb.AddForce(Vector3.forward * ballSpeed * Time.fixedDeltaTime, ForceMode.Impulse);

        Debug.Log(rb.velocity);
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
                rb.AddForce(Vector3.right * 12f, ForceMode.Impulse);
            }

            else
            {
                rb.AddForce(Vector3.left * 12f, ForceMode.Impulse);
            }
        }

    }

    // topun �arpt�g� objey� uzaga savur
    public void reverseForce(Collision collision)
    {
        Rigidbody collisionRb = collision.gameObject.GetComponent<Rigidbody>();

        // E�er �arp�lan cismin Rigidbody bile�eni varsa

        // �arp��man�n kuvvetini ve y�n�n� al
        Vector3 collisionForce = collision.impulse / Time.fixedDeltaTime;

        // Kuvvetin ters y�n�n� al
        Vector3 reverseForce = -collisionForce;

        // �arp��ma kuvvetini uygula
        collisionRb.AddForce(reverseForce * 0.02f, ForceMode.Impulse);

        rb.velocity *= 0.8f;

    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("plane"))
        {
            // b�t�s c�zg�s�ne geld�kten sonra kontrol b�zde olmad�g� �c�n eger bosluklara g�rersede 2.levele git
            if(isFinish)
            SceneManager.LoadScene(2);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        // biti� �izgisine geldiyse 
        if (other.CompareTag("finish-line"))
        {
            isFinish = true;
            scoreText.text = "Puan : " + score;
            missionStatusText.gameObject.SetActive(true);
            missionStatusText.text = "B�l�m Ge�ildi";
            StartCoroutine(MissionStatus(true));

            PlayerPrefs.SetInt("Puan", score);
        }

        // bo�luka girmi� ise biti� �izgisine girmeden once puan� d�rekt 0 yaz
        else if (other.CompareTag("space"))
        {
            if (!isFinish)
            {
                score = 0;
                isSpace= true;
                scoreText.text = "Puan : " + score;
                missionStatusText.gameObject.SetActive(true);
                missionStatusText.text = "B�l�m Ge�ilmedi";
                StartCoroutine(MissionStatus());
            }
        }
    }

  
    public IEnumerator MissionStatus(bool status=false)
    {
        yield return new WaitForSeconds(3f);
        missionStatusText.gameObject.SetActive(false);
        if (!status)
        {
            SceneManager.LoadScene(1);
        }
    }


}

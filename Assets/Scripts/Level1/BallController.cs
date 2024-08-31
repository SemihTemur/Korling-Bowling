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
        // eðer oyun bitmediyse yani bitiþ çizgisine varmadýysa topu kontrol etmeye devam et
        if (!isFinish)
        {
            movement = GetMovementInput();
        }
        else
        {
            scoreText.text = "Puan : " + score;
        }
    }

    // take the ýnputs
    public Vector3 GetMovementInput()
    {
        moveHorizontal = Input.GetAxis("Horizontal");
        moveVertical = Input.GetAxis("Vertical");

        return new Vector3(moveHorizontal, 0.0f, moveVertical);
    }


    public void ApplyBallMovement(Vector3 movement)
    {
        // Ýleri ve geri hareketi kontrol et
        if (movement.z > 0f && ballSpeed < maxSpeed)
        {
            ballSpeed += acceleration;
        }
        else if (movement.z < 0f && ballSpeed > minSpeed)
        {
            ballSpeed -= acceleration;
        }

        // Saða ve sola hareketi kontrol et
        Vector3 velocity = rb.velocity;
        if (movement.x != 0f)
        {
            Debug.Log("selamm");
            rb.AddForce(Vector3.right * movement.x * ballSpeed * Time.fixedDeltaTime, ForceMode.Impulse);
        }

        // Ýleri doðru kuvvet ekle
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

            // Canvas altýnda Text (TMP) bileþenini bul
            TextMeshProUGUI textComponent = collision.gameObject.GetComponentInChildren<TextMeshProUGUI>();

            if (textComponent != null)
            {
                // Text deðerini al
                string textValue = textComponent.text;

                // Regex kullanarak sadece sayýsal kýsmý al
                string numericValue = Regex.Match(textValue, @"\d+").Value;

                // Numeric kýsmý integer'a çevir
                int intValue;

                if (int.TryParse(numericValue, out intValue))
                {
                    // Integer deðeri kullanarak sayacý arttýr
                    score += intValue;
                    Debug.Log("gýrdý");
                }
            }
            else
            {
                Debug.Log("Çarpýlan objede Text (TMP) bileþeni bulunamadý.");
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

    // topun çarptýgý objeyý uzaga savur
    public void reverseForce(Collision collision)
    {
        Rigidbody collisionRb = collision.gameObject.GetComponent<Rigidbody>();

        // Eðer çarpýlan cismin Rigidbody bileþeni varsa

        // Çarpýþmanýn kuvvetini ve yönünü al
        Vector3 collisionForce = collision.impulse / Time.fixedDeltaTime;

        // Kuvvetin ters yönünü al
        Vector3 reverseForce = -collisionForce;

        // Çarpýþma kuvvetini uygula
        collisionRb.AddForce(reverseForce * 0.02f, ForceMode.Impulse);

        rb.velocity *= 0.8f;

    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("plane"))
        {
            // býtýs cýzgýsýne geldýkten sonra kontrol býzde olmadýgý ýcýn eger bosluklara gýrersede 2.levele git
            if(isFinish)
            SceneManager.LoadScene(2);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        // bitiþ çizgisine geldiyse 
        if (other.CompareTag("finish-line"))
        {
            isFinish = true;
            scoreText.text = "Puan : " + score;
            missionStatusText.gameObject.SetActive(true);
            missionStatusText.text = "Bölüm Geçildi";
            StartCoroutine(MissionStatus(true));

            PlayerPrefs.SetInt("Puan", score);
        }

        // boþluka girmiþ ise bitiþ çizgisine girmeden once puaný dýrekt 0 yaz
        else if (other.CompareTag("space"))
        {
            if (!isFinish)
            {
                score = 0;
                isSpace= true;
                scoreText.text = "Puan : " + score;
                missionStatusText.gameObject.SetActive(true);
                missionStatusText.text = "Bölüm Geçilmedi";
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject ball;
    int score = 0;
    int turnCounter = 0;
    GameObject[] pins;
    Rigidbody rb;
    public AudioSource ballAudio;
    public Text scoreUI;

    Vector3[] position;
    public HighScore highScore;
    public GameObject menu;

    [SerializeField]
    float force;

    bool isShooting = false;
    bool isGoingRight = true;

    


    void Start()
    {
        pins = GameObject.FindGameObjectsWithTag("Pin");
        position = new Vector3[pins.Length];
        rb = ball.GetComponent<Rigidbody>();
        rb.maxAngularVelocity = 50;

        for(int i = 0; i < pins.Length; i++)
        {
            position[i] = pins[i].transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            rb.AddForce(Vector3.forward * force);
            ballAudio.Play();
            isShooting = true;
        }

        if(Input.GetKey(KeyCode.Space))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if(!isShooting)
        {
            MoveBall();
        }

        if(Input.GetKeyDown(KeyCode.Space) || ball.transform.position.y < 20)
        {
            CountPinsDown();
            turnCounter++;
            //ResetPins();

            if(turnCounter == 10)
            {
                menu.SetActive(true);
            }
        }
    }


    void MoveBall()
    {
        if(isGoingRight)
        {
            ball.transform.Translate(Vector3.right * Time.deltaTime);
        }
        else
        {
            ball.transform.Translate(Vector3.left * Time.deltaTime);
        }

        if (ball.transform.position.x > 0.5f)
        {
            isGoingRight = false;
        }


        if(ball.transform.position.x < -0.5f)
        {
            isGoingRight = true;
        }
    }


    void CountPinsDown()
    {
        for (int i = 0; i < pins.Length; i++)
        {
            if(pins[i].transform.eulerAngles.z > 5 && pins[i].transform.eulerAngles.z < 355 &&
                pins[i].activeSelf)
            {
                score++;
                pins[i].SetActive(false);
            }
            
            scoreUI.text = score.ToString();

            if(score > highScore.highScore)
            {
                highScore.highScore = score;
            }

            Debug.Log(highScore.highScore);
        }
    }

    /*void ResetPins()
    {
        for (int i = 0; i < pins.Length; i++)
        {
            pins[i].SetActive(true);
            pins[i].transform.position = position[i];
            pins[i].GetComponent<Rigidbody>().velocity = Vector3.zero;
            pins[i].GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            pins[i].transform.rotation = Quaternion.identity;
        }

        ball.transform.position = new Vector3(0, 0.108f, -9f);
        ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
        ball.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        ball.transform.rotation = Quaternion.identity;
    }*/
}

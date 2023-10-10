using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Movement : MonoBehaviour
{
    public float Speed = 5;
    public Rigidbody RB;
    public float jumpforce=10;
    Animator playerAnimator;
    public bool floor;
    public int ExtraJump=1;
    public int Marks;
    public Text Score;
    public Text HighScore;
    public Rigidbody Bullet;
    public Transform SpawnPoint;
    public int facingdirection;
    public static bool GameOver;
    public bool Fallen;
    public GameControl timer;
    public GameObject GameOverUI;
    public GameObject WinScreen;
    private void Awake()
    {
        WinScreen.gameObject.SetActive(false);
        HighScore.text = "Highscore: " + PlayerPrefs.GetInt("Highscore",0).ToString();
        playerAnimator = gameObject.GetComponent<Animator>();
        playerAnimator.SetBool("Spell", false);
        Marks = 0;
        GameOverUI.gameObject.SetActive(false);
       
    }
    private void Start()
    {
        Time.timeScale = 1f;
        Fallen = false;
    }

    // Update is called once per frame
    void Update()
    {   
        float Horizontal = Input.GetAxisRaw("Horizontal");
        Vector3 Direction = new Vector3(Horizontal, 0, 0).normalized; //normalized so we don´t move faster when holding two keys
        transform.position += Direction * Speed * Time.deltaTime;
        if (Horizontal > 0)
        {
            transform.rotation = Quaternion.Euler(0, 90, 0);
            playerAnimator.SetBool("Running", true);
        }
        else if (Horizontal == 0)
        {
            playerAnimator.SetBool("Running", false);
        }
        else if (Horizontal < 0)
        {
            transform.rotation = Quaternion.Euler(0, -90, 0);
            playerAnimator.SetBool("Running", true);
        }

            if (Input.GetButtonDown("Jump")&&floor)
            {
                RB.AddForce(new Vector2(0, jumpforce), ForceMode.Impulse);
            }
        //verifica si está tocando el piso y activa o desactiva el brinco acorde a ello
        if (floor)
        {
            playerAnimator.SetBool("Leaping", false);
            playerAnimator.SetBool("Double", false);
        }
        else if (!floor)
        {
            playerAnimator.SetBool("Leaping", true);
        }
        //activa el hechizo
        if (Input.GetKeyDown(KeyCode.J))
        {
            playerAnimator.SetBool("Spell", true);
            Invoke("BulletSpawner", 1);
        }
        else if(Input.GetKeyUp(KeyCode.J))
        {
            playerAnimator.SetBool("Spell", false);
        }
        //Double jump
        if (Input.GetButtonDown("Jump") && !floor && ExtraJump>0)
        {
            ExtraJump--;
            RB.AddForce(new Vector2(0, jumpforce), ForceMode.Impulse);
            playerAnimator.SetBool("Double", true);
        }
        Score.text = Marks.ToString() + " Marks";
        if (Marks > PlayerPrefs.GetInt("Highscore"))
        {
            PlayerPrefs.SetInt("Highscore",Marks);
            HighScore.text = "Highscore: " + Marks.ToString();
        }
        //Delete Highscore
        if(Input.GetKeyDown(KeyCode.T))
        {
        PlayerPrefs.DeleteKey("Highscore");
        }
        //Saber la dirección a la que mira
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            facingdirection = -1;
            Debug.Log("Left");
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            facingdirection = 1;
            Debug.Log("Right");
        }
        if (Fallen == true || timer.Timer == 0)
        {
            GameOverUI.SetActive(true);
            Time.timeScale = 0f;
            GameOver = true;
        }
        //Win
        if (Marks == 15)
        {
            WinScreen.gameObject.SetActive(true);
        }
        //Aqui termina el update
    }
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Piso")
        {
            floor = true;
            ExtraJump = 1;
            Debug.Log("está tocando el piso");
        }
        else if (collision.gameObject.tag == "Cushion")
        {
            Fallen = true;
        }
    }
    public void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Piso")
        {
            floor = false;
            Debug.Log("Está flotando");
        }

    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Collectible")
        {
            Destroy(other.gameObject);
            Marks++;
        }
    }
    public void BulletSpawner()
    {
        Rigidbody SpawnedBullet;
        SpawnedBullet = Instantiate(Bullet, SpawnPoint.position, Bullet.transform.rotation) as Rigidbody;
        SpawnedBullet.AddForce(new Vector2(30*facingdirection,0),ForceMode.Impulse);
    }
    public void Retry()
    {
        SceneManager.LoadScene("Luces");

    }
    public void toMenu()
    {
        SceneManager.LoadScene("Menu");
    }
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Gracias por jugar");
    }
}

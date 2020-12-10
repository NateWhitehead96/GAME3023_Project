using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControls : MonoBehaviour
{

    public Rigidbody2D rigidbody;
    public float speed;
    private Animator animator;

    public Animator screenTransitions;

    private BattleAttributes myData;
    public GameObject pause;

    private bool justExitedBattle;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        myData = GetComponent<BattleAttributes>();
        justExitedBattle = true;

        if (PlayerPrefs.HasKey("playerLevel"))
        {
            myData.SetAttributes("Player", PlayerPrefs.GetString("playerLevel")
                    , PlayerPrefs.GetInt("playerMaxHP"), PlayerPrefs.GetInt("playerCurrentHP")
                    , PlayerPrefs.GetInt("playerMaxXP"), PlayerPrefs.GetInt("playerCurrentXP"));
        }
        StartCoroutine(IdleScreen());
        // load player position if possible
        LoadPosition();
    }

    IEnumerator IdleScreen()
    {
        yield return new WaitForSeconds(1.5f);
        screenTransitions.SetInteger("ScreenTransition", 1);
    }
    // Update is called once per frame
    void Update()
    {
        Movement();
        if(Input.GetKeyDown("p"))
        {
            pause.GetComponent<PauseScript>().SetShow(true);
            pause.gameObject.SetActive(true);
        }
    }


    private void Movement()
    {
        // movement states are 0 idle, 1 up, 2 right, 3 down, 4 left
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        Vector2 movementVector = new Vector2(x, y);
        movementVector *= speed;
        rigidbody.velocity = movementVector; // Now using rigidbody for movement
        if (pause.GetComponent<PauseScript>().isShowing == false) // if our pause is up then stop player from moving
        {
            speed = 5;
            justExitedBattle = false; // means when you start moving you can now enter a battle
            if (x > 0)
            {
                animator.SetInteger("AnimState", 2);
                animator.SetBool("isWalking", true);
            }
            else if (x < 0)
            {
                animator.SetInteger("AnimState", 4);
                animator.SetBool("isWalking", true);
            }
            else if (y > 0)
            {
                animator.SetInteger("AnimState", 1);
                animator.SetBool("isWalking", true);
            }
            else if (y < 0)
            {
                animator.SetInteger("AnimState", 3);
                animator.SetBool("isWalking", true);
            }
            else
            {
                animator.SetBool("isWalking", false);
            }
        }
        else
            speed = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("BattlePossible"))
        {
            int encounterChance = Random.Range(0, 3);
            Debug.Log(encounterChance);
            if(encounterChance == 0 && justExitedBattle == false)
            {
                // Save location
                SavePosition();
                screenTransitions.SetInteger("ScreenTransition", 2);
                SceneManager.LoadScene("BattleScene");
            }
        }

        if(collision.gameObject.CompareTag("CaveEnter"))
        {
            PlayerPrefs.SetFloat("playerX", transform.position.x);
            PlayerPrefs.SetFloat("playerY", transform.position.y - 1);
            screenTransitions.SetInteger("ScreenTransition", 2);
            SceneManager.LoadScene("CaveScene");
        }

        if(collision.gameObject.CompareTag("CaveExit"))
        {
            screenTransitions.SetInteger("ScreenTransition", 2);
            SceneManager.LoadScene("GameScene");
        }

        if(collision.gameObject.CompareTag("Boss"))
        {
            screenTransitions.SetInteger("ScreenTransition", 2);
            SceneManager.LoadScene("BossBattle");
        }
    }

    private void SavePosition()
    {
        PlayerPrefs.SetFloat("playerX", transform.position.x);
        PlayerPrefs.SetFloat("playerY", transform.position.y);
        PlayerPrefs.Save();
    }

    private void LoadPosition()
    {
        if(PlayerPrefs.HasKey("playerX") && SceneManager.GetActiveScene() == SceneManager.GetSceneByName("GameScene"))
        {
            transform.position = new Vector3( PlayerPrefs.GetFloat("playerX"), PlayerPrefs.GetFloat("playerY"));
        }
    }
}

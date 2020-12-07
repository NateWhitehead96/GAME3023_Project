using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControls : MonoBehaviour
{

    public Rigidbody2D rigidbody;
    public float speed;
    private Animator animator;

    private bool isMoving = false;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }


    private void Movement()
    {
        // movement states are 0 idle, 1 up, 2 right, 3 down, 4 left
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        Vector2 movementVector = new Vector2(x, y);
        movementVector *= speed;
        rigidbody.velocity = movementVector; // Now using rigidbody for movement

        if (x > 0) // or if(Input.GetKeyUp(KeyCode.D) etc
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("BattlePossible"))
        {
            int encounterChance = Random.Range(0, 3);
            Debug.Log(encounterChance);
            if(encounterChance == 0)
            {
                SceneManager.LoadScene("BattleScene");
            }
        }
    }
}

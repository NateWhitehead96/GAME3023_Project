using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{

    public Rigidbody2D rigidbody;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Easy movement using Axis Input
        //float x = Input.GetAxis("Horizontal");
        //float y = Input.GetAxis("Vertical");

        //Vector2 movementVector = new Vector2(x, y);
        //movementVector *= speed;
        //rigidbody.velocity = movementVector; // Now using rigidbody for movement

        Movement();
    }


    private void Movement()
    {
        if(Input.GetKey("w"))// going up
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + speed * Time.deltaTime, transform.position.z);
        }
        else if (Input.GetKey("s"))// going down
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - speed * Time.deltaTime, transform.position.z);
        }
        else if (Input.GetKey("a"))// going left
        {
            transform.position = new Vector3(transform.position.x - speed * Time.deltaTime, transform.position.y, transform.position.z);
        }
        else if (Input.GetKey("d"))// going right
        {
            transform.position = new Vector3(transform.position.x + speed * Time.deltaTime, transform.position.y, transform.position.z);
        }
    }
}

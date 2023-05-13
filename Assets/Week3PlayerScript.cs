
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Week3PlayerScript : MonoBehaviour
{
    Animator anim;
    Rigidbody2D rb;
    public float maxSpeed;
    public float acceleration;
    public float jumpForce;
    public float inAirAcceleration;
    public float inAirMaxSpeed;
    float currentSpeed;
    float upSpeed;
    public Transform animatorTransform;
    public bool amIGrounded;
    public ParticleSystem dust;


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        dust.Stop();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //move starts
        currentSpeed = rb.velocity.x;

        float move = Input.GetAxis("Horizontal");
        if (move < 0)
        {
            animatorTransform.localScale = new Vector3(-1, animatorTransform.localScale.y, animatorTransform.localScale.z); //flips the gameobject to -1 scale. Depending on which direction your gameobject started off, the -1 may need to be 1 instead this allows the animation to play in the opposite direction
            dust.Play();
        }
        else if (move > 0)
        {
            animatorTransform.localScale = new Vector3(1, animatorTransform.localScale.y, animatorTransform.localScale.z);
            dust.Play();
        }

        if (move == 0)
        {
            dust.Stop();
        }

        if (Mathf.Abs(currentSpeed) < maxSpeed && amIGrounded == true)
        {
            rb.AddForce(new Vector2(move * acceleration, 0));
        }
        if (Mathf.Abs(currentSpeed) < inAirMaxSpeed && amIGrounded == false)
        {
            rb.AddForce(new Vector2(move * inAirAcceleration, 0)); 
        }

        anim.SetFloat("speed", (Mathf.Abs(currentSpeed + move)));
        //move ends

        //jump starts
        float moveup = Input.GetAxis("Vertical");
        upSpeed = rb.velocity.y;
        if (Input.GetKey("space") && rb.velocity.y < 0.1 && rb.velocity.y > -0.1 && amIGrounded == true || moveup > 0.1 && rb.velocity.y < 0.1 && rb.velocity.y > -0.1 && amIGrounded == true) //checks if Spacebar OR up button is being pressed, and if the rigidbody is not already moving up/down, and if the player is standing on something
        {
            rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse); 
        }
        anim.SetFloat("verticalSpeed", (upSpeed + moveup));
        //jump ends
    }

    //start of groundcheck
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Player" && collision.isTrigger == false)
        {
            amIGrounded = true;
            anim.SetBool("grounded", true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Player" && collision.isTrigger == false)
        {
            amIGrounded = false;
            anim.SetBool("grounded", false);
        }
    }
}
    //end of groundcheck

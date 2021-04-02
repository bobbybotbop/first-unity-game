using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class newPlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private enum State {idle, running, jumping, falling, crouching, hurt};  //finite state machineeeeeeeeeeeeeeee
    private State state = State.idle;
    private BoxCollider2D boxColl;
    private CircleCollider2D circoll;



    // makes it so you can edit the stuff in unity
    [SerializeField] private LayerMask ground;
    [SerializeField] private float walkSpeed = 14f;
    [SerializeField] private float speed = 14f;
    [SerializeField] private float jumpForce = 30f;
    [SerializeField] private float crouchSpeed = 7f;
    [SerializeField] private double cherries = 0;
    [SerializeField] private Text cherryText;
    [SerializeField] private float hurtForce = 10f;
    [SerializeField] private float sprintSpeed = 30f;
    [SerializeField] private AudioSource cherrySfx;
    [SerializeField] private AudioSource footstep;
    [SerializeField] private AudioSource hurtSfx;
    [SerializeField] private bool isUnder;
    [SerializeField] private bool crouchFix;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxColl = GetComponent<BoxCollider2D>();
        circoll = GetComponent<CircleCollider2D>();
}

    private void Update()
    {
        float hDirection = Input.GetAxis("Horizontal");

        if (state != State.hurt)
        { 
        InputManager(hDirection);
        Crouching();
        // Sprinting();
        }


        CherryRounder();
        VelocityState();
        
        
        anim.SetInteger("state", (int)state);  // sets animation based off state

        if (Input.GetButtonUp("Crouch"))
        {
            crouchFix = true;
        }
        else if (Input.GetButtonDown("Crouch"))
        {
            crouchFix = false;
        }

        else if (isUnder == false)
        {
            crouchFix = false;
        }
    }

    private void CherryRounder()// cherry bug fix
    {
        if (cherries > 0)
        {
            cherries = Math.Round(cherries);
            cherryText.text = cherries.ToString();
        }
    }

    private void InputManager(float hDirection)// where all the moves are
    {
        // moving left
        if (hDirection < 0)
        {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
            transform.localScale = new Vector2(-13, 13);

        }

        // moving right
        else if (hDirection > 0)
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
            transform.localScale = new Vector2(13, 13);
        }

        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }


        // jumping script
        if (Input.GetButtonDown("Jump") && circoll.IsTouchingLayers(ground))
        {
            Jump();
        }

    }

    private void Crouching()
    {
        // crouching script
        if (Input.GetButtonDown("Crouch") || (isUnder == true))
        {
            boxColl.enabled = false;
            speed = crouchSpeed;
            state = State.crouching;
        }
        else if (Input.GetButtonUp("Crouch"))
        {
            boxColl.enabled = true;
            speed = walkSpeed;
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        state = State.jumping;
    }

    private void OnTriggerEnter2D(Collider2D collision) // cherry picker-uper and all collision stuffs
    {
        if(collision.tag == "Collectable")
        {
            cherrySfx.Play();
            Destroy(collision.gameObject);
            cherries += .51;
        }
        if (collision.tag == "ground")
        {
            isUnder = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision) // isunderstuffs
    {
        if(collision.tag == "ground")
        {
            isUnder = false;
            if (crouchFix == false)
            {
                
            }
            else
            {
                boxColl.enabled = true;
                speed = walkSpeed;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)  //how the enemy interacts with fox
    {
        if(other.gameObject.tag == "Enemy")
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            if ((other.gameObject.transform.position.y + 2.5) < transform.position.y)
            {
                enemy.JumpedOn();
                Jump();
            }
            else
            {
                if (other.gameObject.transform.position.x > transform.position.x)
                {
                    hurtSfx.Play();
                    state = State.hurt;
                    // im to the right of the guy
                    rb.velocity = new Vector2(-hurtForce, rb.velocity.y + (hurtForce - 5));
                }
                else if (other.gameObject.transform.position.x < transform.position.x)
                {
                    hurtSfx.Play();
                    state = State.hurt;
                    // im to the left
                    rb.velocity = new Vector2(hurtForce, rb.velocity.y + (hurtForce - 5));

                }
            }


            
        }
    }


    private void VelocityState() // jumping animation
    {
        if(state == State.jumping)
        {
            if (rb.velocity.y < 0.1)
            {
                state = State.falling;
            }
        } else if (state == State.falling)  // falling animation
        {
            if (circoll.IsTouchingLayers(ground))
            {
                state = State.idle;
            }
        }
        // hurt animation         
        else if(state == State.hurt)
        {
            if(Mathf.Abs(rb.velocity.x) < .1f)
            {
                state = State.idle;
            }
        }




        // running animation
        else if(Mathf.Abs(rb.velocity.x) > 2)
        {            
            state = State.running;
        }
        else // idle animation
        {
            state = State.idle;
        }

        //crouching animation
        if (boxColl.enabled == false)
        {
            if(state != State.hurt)
            {
                state = State.crouching;
            }
        }
        

    }
    

    private void Footsteps()
    {
        if (circoll.IsTouchingLayers(ground))
        {
            footstep.Play();
        }
    }

}


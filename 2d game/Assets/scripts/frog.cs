using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class frog : Enemy
{


    [SerializeField] private LayerMask ground;

    [SerializeField] private float leftCap;
    [SerializeField] private float rightCap;

    [SerializeField] private float jumpLength = 4f;
    [SerializeField] private float jumpHeight = 8f;


    private bool facingLeft = true;


    protected override void Start()
    {
        base.Start(); //runs enemy.cs start function
        boxcoll = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (anim.GetBool("Jumping"))
        {
            if(rb.velocity.y < .1)
            {
                anim.SetBool("Falling", true);
                anim.SetBool("Jumping", false);
            }
        }

        if (anim.GetBool("Falling") && rb.IsTouchingLayers(ground))
        {
             anim.SetBool("Falling", false);
        }
        
    }

    private void Move()
    {
        if (facingLeft == true)
        {

            if (transform.position.x > leftCap)
            {
                if (transform.localScale.x != 1)
                {
                    transform.localScale = new Vector3(1, 1);
                }
                if (boxcoll.IsTouchingLayers(ground))
                {
                    rb.velocity = new Vector2(-jumpLength, jumpHeight);
                    anim.SetBool("Jumping", true);
                }
            }
            else
            {
                facingLeft = false;
            }
        }

        if (facingLeft == false)
        {

            if (transform.position.x < rightCap)
            {
                if (transform.localScale.x != -1)
                {
                    transform.localScale = new Vector3(-1, 1);
                }
                if (boxcoll.IsTouchingLayers(ground))
                {
                    rb.velocity = new Vector2(jumpLength, jumpHeight);
                    anim.SetBool("Jumping", true);
                }
            }
            else
            {
                facingLeft = true;
            }

        }
    }

}

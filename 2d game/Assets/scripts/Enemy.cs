using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected Animator anim;
    protected BoxCollider2D boxcoll;
    protected Rigidbody2D rb;
    protected AudioSource death;

    protected virtual void Start()
    {
        anim = GetComponent<Animator>();
        boxcoll = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        death = GetComponent<AudioSource>();
    }

    private void Death()
    {
        Destroy(this.gameObject);
    }

    public void JumpedOn()
    {
        death.Play();
        anim.SetTrigger("Death");
        boxcoll.enabled = false;
        rb.simulated = false;
    }
}

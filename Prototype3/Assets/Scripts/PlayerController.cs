using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private Collider2D coll;

    //Finite State Machine (FSM) 
    private enum State { idle, running, jumping, falling };
    private State state = State.idle;

    //inspector variables
    [SerializeField]private LayerMask ground;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 10f; 
    [SerializeField] private int cherries = 0;
    [SerializeField] private Text cherryText;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>(); 
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
    }

    private void Update()
    {
        Movement(); 

        if(rb.transform.position.y < -33)
        {
            SceneManager.LoadScene("SampleScene");
        }
    }

    private void FixedUpdate()
    {
        

        animationState(); 
        //setting animation based on Enumerator state
        anim.SetInteger("state", (int)state);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Collectible")
        {
            Destroy(collision.gameObject);
            cherries += 1;
            cherryText.text = cherries.ToString();
            SoundManagerScript.PlaySound("pickup");
        }
    }

    private void Movement()
    {
        float hDirection = Input.GetAxis("Horizontal");
        //float vDirection = Input.GetAxis("Vertical");

        //moving left
        if (hDirection < 0)
        {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
            transform.localScale = new Vector2(-1, 1);
            //anim.SetBool("Running", true);
        }
        //moving right
        else if (hDirection > 0)
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
            transform.localScale = new Vector2(1, 1);
            //anim.SetBool("Running", true);
        }

        else
        {
            //anim.SetBool("Running", false);
        }

        //jumping
        if (Input.GetButtonDown("Jump") && coll.IsTouchingLayers())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            state = State.jumping;
        }
    }

    private void animationState()
    { 
        if(state == State.jumping)
        {
            if(rb.velocity.y < 0.1f)
            {
                state = State.falling;
            }
        } 
        else if(state == State.falling)
        {
            if (coll.IsTouchingLayers(ground))
            {
                state = State.idle;
            }
        } 
        else if (state != State.jumping && !coll.IsTouchingLayers(ground))
        {
            state = State.falling;
        }
        else if(Mathf.Abs(rb.velocity.x) > Mathf.Epsilon)
        {
            //Moving 
            state = State.running;
            Debug.Log("Movin");
        }
        else
        {
            state = State.idle;
        }
    }
}

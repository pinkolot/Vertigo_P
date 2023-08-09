using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("General")]
    public float speed;
    public float sprintspeed;
    public float jumpforce;
    private float moveInput;
    private float gravity;
    bool facingRight = true;

    [Header("Interactors")]
    public Rigidbody2D rb;
    public Animator anim;
    public BoxCollider2D bc;

    [Header("Jumping")]
    public bool isGrounded;
    public Transform groundcheck;
    public LayerMask whatisGround;
    public int extrajumps;
    public int maxjumps;

    [Header("Walls")]
    public bool isWall;
    public Transform wallcheck;
    public LayerMask whatisWall;
    public float wallspeed;
    public float wallDistance;
    public int wallSide;
    public float wallCheckDistance = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        extrajumps = maxjumps -1;
        facingRight = false;
    }

    // Update is called once per frame
    void Update()
    {
        //flipping
        if(facingRight == false && moveInput > 0){
            Flip();
        }
        else if(facingRight && moveInput < 0){
            Flip();
        }

        //jumping 
        if(Input.GetButtonDown("Jump") && extrajumps > 1)
        {
            Jump();
        }
        if(isGrounded || isWall)
        {
            extrajumps = maxjumps;
        }

    }

    void FixedUpdate(){
        moveInput = Input.GetAxis("Horizontal");

        //moving
        if(Input.GetAxis("Vertical") == 0){
            rb.velocity = new Vector2(moveInput * speed * 10f * Time.deltaTime, rb.velocity.y);
        }

        //wall Jumping and Sliding
        if(isWall && !isGrounded)
        { 
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallspeed, float.MaxValue));
            if(Input.GetAxis("Vertical") != 0)
            {
                wallJump();
            }
        }
    }

    void Flip(){
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    void Jump(){
        extrajumps -= 1;
        rb.velocity = new Vector2(rb.velocity.x, jumpforce);  
        //rb.AddForce(transform.up * jumpforce * 1000f *Time.deltaTime);
    }

    void wallJump(){
        extrajumps -= 1;
        // rb.velocity = new Vector2(rb.velocity.x, jumpforce);
        Debug.Log("Wall jump executed"); // add this line
        wallSide = Physics2D.Raycast(transform.position, Vector2.right, wallCheckDistance) ? -1 : 1;
        rb.velocity = new Vector2(jumpforce * wallSide, jumpforce);
    }

    void OnCollisionEnter2D(Collision2D collision){
        // check if touching the ground or wall.
        if (collision.gameObject.CompareTag("Ground")){
            isGrounded = true;
        }
        if(collision.gameObject.CompareTag("Wall")){
            isWall = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision){ 
        // When exiting the ground or wall.
        if (collision.gameObject.CompareTag("Ground")){
            isGrounded = false;
        }
        if(collision.gameObject.CompareTag("Wall")){
            isWall = false;
        }
    }

}


//  ----- Values I use -----  // 
// speed - 7
// jumpforce 3.7
// max jumps 2
// physics material called "slippy" with friction 0 and bouncines 0. 


//  ----- Comments ------  //
// The jump is now consistent but still working on wall jump.
// Changed the code a little to jump functions being their own void thingy and calling them.
// Also I suggest for player interactions, instead of splitting into Gina's code and Paula's code, it be one big thing. 
// The commented out code is stuff that could also work but just feels a little different. Use which ever you prefer.


//  ----- Comments-2 -----  // 
// Attach the slippy material to all walls and bumps although it makes the top of the bumps slidy. I'll get a fix for that. 
// Still trying to find a way to make the wallsliding speed adjustable but other that that things are good.
// I commented out the animations section because somehow it's messing with the jumping... idk, you'll fix that right?

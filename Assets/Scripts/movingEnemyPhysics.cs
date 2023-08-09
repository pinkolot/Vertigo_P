using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movingEnemyPhysics : MonoBehaviour
{
    [SerializeField] Transform castPos;
    [SerializeField] float baseCastDist;
    public float speed;
    private Rigidbody2D rb;
    private BoxCollider2D bc;
    Vector3 baseScale;

    string facingDirection;
    const string left = "left";
    const string right = "right";

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();
        facingDirection = right;
        baseScale = transform.localScale;
    }
    private void FixedUpdate()
    {
        float vX = speed;

        if (facingDirection == left)
        {
            vX = -speed;
        }
        rb.velocity = new Vector2(vX, rb.velocity.y);

        if (isHittingWall() || isNearEdge())
        {
            if (facingDirection == left)
            {
                changeFacingDirection(right);
            }
            else if( facingDirection == right)
            {
                changeFacingDirection(left);
            }
        }
    }

    void changeFacingDirection(string newDirection)
    {
        Vector3 newScale = baseScale;

        if (newDirection == left)
        {
            newScale.x = -baseScale.x;
        }
        else
        {
            newScale.x = baseScale.x;
        }

        transform.localScale = newScale;
        facingDirection = newDirection;
    }

    bool isHittingWall()
    {        
        bool val = true;
        float castDist = baseCastDist;

        //define the cast distance for left and right.
        if (facingDirection == left)
        {
            castDist = -baseCastDist;
        }

        //determines the target destination based on the cast distance. 
        Vector3 targetPos = castPos.position;
        targetPos.x += castDist;

        Debug.DrawLine(castPos.position, targetPos, Color.blue);

        if (Physics2D.Linecast(castPos.position, targetPos, 1 << LayerMask.NameToLayer("Ground")) ||
        Physics2D.Linecast(castPos.position, targetPos, 1 << LayerMask.NameToLayer("Wall")))
        {
            val = true;
            
        }
        else
        {
            val = false;
        }

        return val;
    }

    bool isNearEdge()
    {
        bool val = true;
        float castDist = baseCastDist;

        //determines the target destination based on the cast distance. 
        Vector3 targetPos = castPos.position;
        targetPos.y -= castDist;

        Debug.DrawLine(castPos.position, targetPos, Color.red);

        if (Physics2D.Linecast(castPos.position, targetPos, 1 << LayerMask.NameToLayer("Ground")) ||
        Physics2D.Linecast(castPos.position, targetPos, 1 << LayerMask.NameToLayer("Wall")))
        {
            val = false;
        }
        else
        {
            val = true;
        }
        return val;
    }
    bool isNearPlayer(){
         bool val = true;
        float castDist = baseCastDist;

        //determines the target destination based on the cast distance.
        Vector3 targetPos = castPos.position;
        targetPos.y -= castDist;

        Debug.DrawLine(castPos.position, targetPos, Color.red);

        if (Physics2D.Linecast(castPos.position, targetPos, 1 << LayerMask.NameToLayer("Centients")))
        {
            val = false;
        }
        else
        {
            val = true;
        }
        return val;
    }

    
}

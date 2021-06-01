using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    public float jumpForce = 3;
    public float jumpHoldForce = 1;
    public float jumpHoldDuration = 0.15f;
    public float footOffset;
    public float groundOffset;
    public float groundDistance;
    public LayerMask groundLayer;
    float jumpTime;
    public Rigidbody2D rb;
    bool isOnGround;
    bool JumpStarted;
    bool jumpHeld;
    bool isJumping;

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Walk();
        Jump();
        BetterJump();
        AirMovement();
        PhysicsCheck();
    }

    void PhysicsCheck()
    {
        isOnGround = false;

        RaycastHit2D leftFoot = Raycast(new Vector2(-footOffset, -groundOffset), Vector2.down, groundDistance, groundLayer);
        RaycastHit2D rightFoot = Raycast(new Vector2(footOffset, -groundOffset), Vector2.down, groundDistance, groundLayer);
        if (leftFoot || rightFoot)
        {
            isOnGround = true;
        }
    }
    void Walk()
    {
        float h = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(h * speed, rb.velocity.y);

    }
    void Jump()
    {
        if (Input.GetButtonDown("Jump") && isOnGround)
        {
            JumpStarted = true;
        }
        jumpHeld = Input.GetButton("Jump");
    }
    void AirMovement()
    {
        if (JumpStarted)
        {
            isJumping = true;
            JumpStarted = false;
            rb.velocity = Vector2.zero;
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jumpTime = Time.time + jumpHoldDuration;
        }
        if (isJumping)
        {
            if (jumpHeld)
            {
                rb.AddForce(Vector2.up * jumpHoldForce, ForceMode2D.Impulse);
            }
            if (jumpTime <= Time.time)
            {
                isJumping = false;
            }
        }
    }
    void BetterJump()
    {
        if(rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }else if(rb.velocity.y > 0 && !JumpStarted)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }

    RaycastHit2D Raycast(Vector2 offset, Vector2 rayDirection, float length, LayerMask layerMask)
    {
        Vector2 pos = transform.position;
        RaycastHit2D hit = Physics2D.Raycast(pos + offset, rayDirection, length, layerMask);
        Color color = hit ? Color.red : Color.green;
        Debug.DrawRay(pos + offset, rayDirection * length, color);
        return hit;
    }
}

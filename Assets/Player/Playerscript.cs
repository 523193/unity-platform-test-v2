using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class Playerscript : MonoBehaviour
{


    InputAction moveAction;
    InputAction jumpaction;
    InputAction crouchAction;

    Rigidbody2D rb;
    SpriteRenderer spriteRenderer;

    public LayerMask groundLayerMask;

    bool isGrounded;

    float acceleration = 10f;
    float maxSpeed = 4f;

    Animator anim;

    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        jumpaction = InputSystem.actions.FindAction("Jump");
        crouchAction = InputSystem.actions.FindAction("Crouch");

        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        isGrounded = false;
        groundLayerMask = LayerMask.GetMask("Ground");
    }

    void Update()
    {
        bool leftRay = RayCollisionCheck(-0.2f, 0);
        bool middleRay = RayCollisionCheck(0, 0);
        bool rightRay = RayCollisionCheck(0.2f, 0);

        isGrounded = leftRay || middleRay || rightRay;

        Vector2 moveVel = moveAction.ReadValue<Vector2>();

        if (moveVel.x > 0)
        {
            spriteRenderer.flipX = false;
        }

        else if (moveVel.x < 0)
        {
            spriteRenderer.flipX = true;
        }

        if (moveVel.x > 0)
        {
            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x + acceleration * Time.deltaTime,
                rb.linearVelocity.y
            );

            if (rb.linearVelocity.x > maxSpeed)
            {
                rb.linearVelocity = new Vector2(
                    maxSpeed,
                    rb.linearVelocity.y
                );
            }
        }

        if (moveVel.x < 0)
        {
            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x - acceleration * Time.deltaTime,
                rb.linearVelocity.y
            );

            if (rb.linearVelocity.x < -maxSpeed)
            {
                rb.linearVelocity = new Vector2(
                    -maxSpeed,
                    rb.linearVelocity.y
                );
            }
        }

        if (jumpaction.WasPressedThisFrame() && isGrounded)
        {
            Jump();
        }

        if (rb.linearVelocityX != 0)
        {
            anim.SetBool("walk", true);
        }

        else
        {
            anim.SetBool("walk", false);
        }

        if (crouchAction.IsPressed())
        {
            anim.SetBool("crouch", true);
        }

        else
        {
            anim.SetBool("crouch", false);
        }
    }

    public bool RayCollisionCheck(float xoffs, float yoffs)
    {
        float rayLength = 0.5f;
        bool hitSomething = false;

        Vector3 offset = new Vector3(xoffs, yoffs, 0);

        RaycastHit2D hit;

        hit = Physics2D.Raycast(
            transform.position + offset,
            Vector2.down,
            rayLength,
            groundLayerMask
        );

        Color hitColor = Color.red;

        if (hit.collider != null)
        {
            print("Player has collided with Ground layer");

            hitColor = Color.green;
            hitSomething = true;
        }

        Debug.DrawRay(
            transform.position + offset,
            Vector2.down * rayLength,
            hitColor
        );

        return hitSomething;
    }

    void Jump()
    {
        rb.linearVelocity = new Vector2(
            rb.linearVelocity.x,
            7
        );

        isGrounded = false;
    }
}




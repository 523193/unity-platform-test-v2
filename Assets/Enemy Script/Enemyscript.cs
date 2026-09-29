using UnityEngine;
using UnityEngine.InputSystem;

public class Enemyscript : MonoBehaviour
{


public float speed = 2f;
    public float detectionRange = 2f;
    public float wanderTime = 2f;

    LayerMask groundLayerMask;

    SpriteRenderer sprite;
    Animator anim;

    Transform player;

    bool movingRight = true;
    float timer;

    bool leftRay;
    bool middleRay;
    bool rightRay;


    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        // Find the Player
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;
        }

        timer = wanderTime;

        groundLayerMask = LayerMask.GetMask("Ground");
    }


    void Update()
    {
        // Three rays
        leftRay = RayCollisionCheck(-0.5f, 0);
        middleRay = RayCollisionCheck(0, 0);
        rightRay = RayCollisionCheck(0.5f, 0);

        // If Player cannot be found, keep wandering
        if (player == null)
        {
            Wander();
            return;
        }

        // Check distance between Enemy and Player
        float distance = Vector2.Distance(
            transform.position,
            player.position
        );

        // Player is close
        if (distance <= detectionRange)
        {
            FollowPlayer();
        }

        else
        {
            Wander();
        }
    }


    void Wander()
    {
        // Moving right
        if (movingRight)
        {
            // Turn around if there is no ground on the right
            if (rightRay == false && middleRay)
            {
                movingRight = false;
            }
        }

        // Moving left
        else
        {
            // Turn around if there is no ground on the left
            if (leftRay == false && middleRay)
            {
                movingRight = true;
            }
        }

        // Move right
        if (movingRight)
        {
            transform.position +=
                Vector3.right * speed * Time.deltaTime;

            sprite.flipX = false;
        }

        // Move left
        else
        {
            transform.position +=
                Vector3.left * speed * Time.deltaTime;

            sprite.flipX = true;
        }

        anim.SetBool("walk", true);
    }


    void FollowPlayer()
    {
        // Player is to the right
        if (player.position.x > transform.position.x)
        {
            // Check for ground before the edge
            if (rightRay || middleRay)
            {
                transform.position +=
                    Vector3.right * speed * Time.deltaTime;
            }

            sprite.flipX = false;
        }

        // Player is to the left
        else if (player.position.x < transform.position.x)
        {
            // Check for ground before the edge
            if (leftRay || middleRay)
            {
                transform.position +=
                    Vector3.left * speed * Time.deltaTime;
            }

            sprite.flipX = true;
        }

        anim.SetBool("walk", true);
    }


    // RAYCAST
    public bool RayCollisionCheck(float xoffs, float yoffs)
    {
        float rayLength = 1.3f;
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


    public void Splat()
    {
        anim.SetBool("walk", false);

        anim.SetTrigger("splat");
    }
}
  





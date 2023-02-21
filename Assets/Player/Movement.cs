using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    // Movement Vars
    private const float baseMovement = 2f;
    [SerializeField]
    private float movementSpeed = baseMovement;
    private float movementX     = 0f;
    private float movementY     = 0f;
    // Movement Accelaration Stuff
    private float movementAcceleration  = 0.15f;
    private float startTime             = 0f;
    private float s                     = 0f;
    private bool  isMoving              = false;
    private float timeSinceLastKeyPress = 10f;
    // Status
    private bool isFacingRight = true;
    private bool isJumping = false;
    private bool canJump = true;

    // Components
    private Player player = null;
    private Rigidbody2D rb = null;
    private InputManager input = null;


    void Start()
    {
        player = GetComponent<Player>();
        rb = GetComponent<Rigidbody2D>();
        input = GetComponent<InputManager>();
    }


    // Getting Input
    void Update()
    {
        // if (!player.IsAlive)        return;

        /// Movement Input
        movementX = input.MovementX;
        movementY = input.MovementY;

        // Avoiding getting stuck when pressong both direction at the same time
        // Overiding with newest direction
        if (input.IsMovingX && movementX == 0) {
            // movementX = input.History[1].x;
            movementX = input.MoveHistory[0].x * -1;
        }
        if (input.IsMovingY && movementY == 0) {
            // movementY = input.MoveHistory[1].y;
            movementY = input.MoveHistory[0].y * -1;
        }

        // Movement speed
        Stat speedStat = player.Stats.MoveSpeed;
        movementSpeed = baseMovement + (speedStat.Value * 0.05f);


        /// Slower start
        // Not pressing any keys
        if (movementX == 0 && movementY == 0)
        {
            s = 0;
            isMoving = false;
            timeSinceLastKeyPress += Time.deltaTime;
        }
        // Pressed a key & wasn't moving & more than 0.1 since last key press
        if((movementX != 0 || movementY != 0) && !isMoving && timeSinceLastKeyPress > 0.1)
        {
            startTime = Time.time;
            isMoving = true;
        }
        // Otherwise, slower start (half speed)
        if(movementX != 0 || movementY != 0)
        {
            float t = (Time.time - startTime) / movementAcceleration;
            s = Mathf.SmoothStep(movementSpeed / 2f, movementSpeed, t);
            timeSinceLastKeyPress = 0;
        }


        // Jumping/dodging
        // if (input.JumpPress && !isJumping) {
        //     Vector2 direction = new Vector2(movementX, movementY);
        //     Dodge(direction);
        // }
    }

    // Physics
    private void FixedUpdate()
    {
        // Jumping/Dodging/Dashing
        if (player.Status.IsJumping || player.Status.IsDodging) {
            return;
        }

        // Movement
        Vector2 playerVelocity = new Vector2(movementX, movementY);
        rb.velocity = playerVelocity.normalized * s;

        // Flipping
        if(movementX > 0 && !isFacingRight)     Flip();
        if(movementX < 0 && isFacingRight)      Flip();
    }

    /// <summary>
    /// Rotates the player sprite 180 degrees to face a new direction.
    /// </summary>
    private void Flip() {
        Vector3 rotation = transform.localEulerAngles;
        rotation.y += 180f;
        transform.localEulerAngles = rotation;

        isFacingRight = !isFacingRight;
    }

}

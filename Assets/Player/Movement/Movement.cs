using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    // Movement Vars
    private float baseMovement  = 3f;
    private float movementSpeed = 0f;
    private float movementX     = 0f;
    private float movementY     = 0f;
    private float maxSpeed      = 8f;
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
        // Get components
        player = GetComponent<Player>();
        rb = GetComponent<Rigidbody2D>();
        input = GetComponent<InputManager>();

        // Get base movement speed
        baseMovement = player.Stats.MoveSpeed.baseValue;
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
        movementSpeed = baseMovement + (speedStat.Value / speedStat.valueModifier);
        movementSpeed = Mathf.Clamp(movementSpeed, baseMovement, maxSpeed);


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
    }

    // Physics
    private void FixedUpdate()
    {
        // Ignore movement input if Jumping/Dodging/Dashing/Knocked-back or DEAD
        if (player.Status.IsJumping || player.Status.IsDodging || player.Status.IsBackflipping || player.Status.IsKnockedBack || !player.Status.IsAlive) {
            return;
        }

        // Movement
        Vector2 playerVelocity = new Vector2(movementX, movementY);
        rb.velocity = playerVelocity.normalized * s;
    }

}

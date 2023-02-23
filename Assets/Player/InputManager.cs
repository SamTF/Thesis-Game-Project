using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Tracks all Player Inputs in a single centralised scripts, and shares these values to other components using getters. 
/// </summary>

public class InputManager : MonoBehaviour
{
    /// Input Names
    private bool    usingController = false;
    private string inputHorizontal  = "Horizontal";
    private string inputVertical    = "Vertical";
    private string attackHorizontal = "HorizontalAttack";
    private string attackVertical   = "VerticalAttack"; 
    private string inputFire1       = "Fire1";
    private string inputFire2       = "Fire2";
    private string inputJump        = "Jump";

    /// Private Input values accessible from other components using the getter functions
    // Movement
    private float movementX     = 0f;
    private float movementY     = 0f;
    private bool  isMovingX     = false;
    private bool  isMovingY     = false;
    private bool  jumpPress     = false;
    private bool  jumping       = false;
    // Attack
    private bool  attacking     = false;
    private float attackX = 0f;
    private float attackY = 0f;
    
    // Utilities
    /// <summary>Keeps track of the current and previous movement input directions.</summary>
    private Vector2[] moveHistory = new Vector2[2] {Vector2.one, Vector2.zero};
    /// <summary> Seconds since the user switched to a new movement direction.</summary>
    private Vector2 timeSinceDirectionSwitch = Vector2.one;
    /// <summary>How many seconds a user spent holding down the current and previous direction buttons.</summary>
    private float[] timeHoldingDirection = new float[2] {0, 0};


    // Update is called once per frame
    void Update()
    {
        // Debug.Log(timeSinceDirectionSwitch);

        movementX   = Input.GetAxisRaw(inputHorizontal);
        movementY   = Input.GetAxisRaw(inputVertical);
        isMovingX   = Input.GetButton(inputHorizontal);
        isMovingY   = Input.GetButton(inputVertical);

        jumpPress   = Input.GetButtonDown(inputJump);
        jumping     = Input.GetButton(inputJump);

        attackX     = Input.GetAxisRaw(attackHorizontal);
        attackY     = Input.GetAxisRaw(attackVertical);
        // attacking   = Input.GetButtonDown(inputFire1);

        // Utilities
        Vector2 movement = new Vector2(movementX, movementY);

        //// Movement History
        // Add to the timer is moving in the same direction or not moving at all
        if (movement == Vector2.zero || movement.x == moveHistory[0].x) {
            timeSinceDirectionSwitch.x += Time.deltaTime;
        } else if (movement.y == moveHistory[0].y) {
            timeSinceDirectionSwitch.y += Time.deltaTime;
        }

        // Time history
        if (movement == moveHistory[0]) {
            timeHoldingDirection[0] += Time.deltaTime;
        }

        // If the movement changed
        if (movement != moveHistory[0]) {

            // ignore standing still
            if (movement == Vector2.zero) return;

            // storing the old direction (no zero allowed, must be moving in some direction)
            if (moveHistory[0] != Vector2.zero) {
                moveHistory[1] = moveHistory[0];

                // reset timer when changing direction
                timeSinceDirectionSwitch = Vector2.zero;

                // move 
                timeHoldingDirection[1] = timeHoldingDirection[0];
                timeHoldingDirection[0] = 0;
            }

            // New current direction
            moveHistory[0] = movement;
        }

    }

    ////  Getters
    public Vector2 Movement => new Vector2(movementX, movementY);
    public float MovementX => movementX;
    public float MovementY => movementY;
    public bool  IsMovingX => isMovingX;
    public bool  IsMovingY => isMovingY;

    public bool JumpPress => jumpPress;
    public bool Jumping   => jumping;

    public float AttackX => attackX;
    public float AttackY => attackY;
    // public bool Attacking => attacking;

    // Utilities
    public Vector2[] MoveHistory => moveHistory;
    public Vector2 TimeSinceDirectionSwitch => timeSinceDirectionSwitch;
}

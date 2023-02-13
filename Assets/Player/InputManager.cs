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
    private string inputFire1       = "Fire1";
    private string inputFire2       = "Fire2";
    private string inputJump        = "Jump";

    /// Private Input values accessible from other components using the getter functions
    // Movement
    private float movementX     = 0f;
    private float movementY     = 0f;
    private bool  jumpPress     = false;
    private bool  jumping       = false;
    // Attack
    private bool  attacking     = false;
    
    // Update is called once per frame
    void Update()
    {
        movementX   = Input.GetAxisRaw(inputHorizontal);
        movementY   = Input.GetAxisRaw(inputVertical);

        jumpPress   = Input.GetButtonDown(inputJump);
        jumping     = Input.GetButton(inputJump);

        attacking   = Input.GetButtonDown(inputFire1);
    }

    //  Getters
    public float MovementX => movementX;
    public float MovementY => movementY;

    public bool JumpPress => jumpPress;
    public bool Jumping   => jumping;

    public bool Attacking => attacking;
}

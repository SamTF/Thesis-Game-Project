using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status : MonoBehaviour
{
    //// Bools
    // Alive
    private bool isAlive = true;
    // Movement
    private bool isJumping = false;
    private bool canJump = true;
    private bool isDodging = false;
    private bool canDodge = true;
    private bool isBackflipping = false;
    private bool canBackflip = true;
    // Shooting
    private bool canShoot = true;

    // Cooldowns
    private float jumpCooldown = 1f;
    private float backflipCooldown = 1f;
    private float dodgeCooldown = 1f;
    private float shootingCooldown = 0.5f;

    ///// GETTER SETTERS
    
    /// <summary>
    /// Whether the Player is currently Jumping or not.
    /// </summary>
    /// <value>Bool</value>
    public bool IsJumping {
        get { return isJumping; }
        set {
            isJumping = value;

            if (value == true) {
                canJump = false;
            }
            else {
                StartCoroutine(StatusCooldown (result => canJump = result, false, 1f));
            }
        }
    }

    /// <summary>
    /// Whether the Player is currently Dodging or not.
    /// </summary>
    /// <value>Bool</value>
    public bool IsDodging {
        get { return isDodging; }
        set {
            if (value == true) {
                StartCoroutine(StatusCooldown(result => isDodging = result, true, 0.25f));
                StartCoroutine(StatusCooldown (result => canDodge = result, false, 0.5f));
            } else {
                isDodging = false;
            }
        }
    }

    /// <summary>
    /// Whether the Player is currently Backflipping.
    /// </summary>
    /// <value>Setting this to true will begin performing a backflip!</value>
    public bool IsBackflipping {
        get { return isBackflipping; }
        set {
            if (value == true) {
                isBackflipping = true;
                StartCoroutine(StatusCooldown(result => canBackflip = result, false, 1f));
            } else {
                isBackflipping = false;
            }
        }
    }

    /// <summary>
    /// Player can shoot if the shooting cooldown has passed AND they are not jumping or dodging
    /// </summary>
    /// <value>Setting this to False will begin the shooting cooldown</value>
    public bool CanShoot {
        get { return canShoot && !isBackflipping && !isDodging && !isJumping; }
        set {
            if (value == false) {
                StartCoroutine( StatusCooldown(result => canShoot = result, false, shootingCooldown) );
            } else {
                canShoot = true;
            }
        }
    }

    // Basic getters
    public bool CanJump => canJump;
    public bool CanDodge => canDodge;
    public bool CanBackflip => canBackflip;


    ///// HELPER FUNCTIONS

    /// <summary>
    /// Sets any bool status variable to a false, waits X seconds, then sets it back to true.
    /// </summary>
    /// <param name="variableToChange">Reference to the status variable</param>
    /// <param name="newValue">Value to set right now (will be reversed in X seconds)</param>
    /// <param name="cooldown">How long to wait (in seconds) before reversing the value</param> 
    /// FROM: https://forum.unity.com/threads/passing-ref-variable-to-coroutine.379640/
    public static IEnumerator StatusCooldown(System.Action<bool> variableToChange, bool newValue, float cooldown) {
        variableToChange (newValue);
        yield return new WaitForSeconds(cooldown);
        variableToChange (!newValue);
    }
}

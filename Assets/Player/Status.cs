using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status : MonoBehaviour
{
    // Bools
    private bool isAlive = true;
    private bool isJumping = false;
    private bool canJump = true;
    private bool canBackflip = true;
    private bool isDodging = false;
    [SerializeField]
    private bool canDodge = true;

    // Cooldowns
    private float jumpCooldown = 1f;
    private float backflipCooldown = 1f;
    private float dodgeCooldown = 1f;

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
                StartCoroutine(StatusCooldown (result => canDodge = result));
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
            isDodging = value;
            if (value == true) {
                canDodge = false;
            } else {
                StartCoroutine(StatusCooldown (result => canDodge = result));
            }
        }
    }

    public bool CanJump => canJump;
    public bool CanDodge => canDodge;
    public bool CanBackflip => canBackflip;


    ///// HELPER FUNCTIONS

    /// <summary>
    /// Sets any bool status variable to a false, waits X seconds, then sets it back to true.
    /// </summary>
    /// <param name="variableToChange">Reference to the status variable</param>
    /// FROM: https://forum.unity.com/threads/passing-ref-variable-to-coroutine.379640/
    public static IEnumerator StatusCooldown(System.Action<bool> variableToChange) {
        variableToChange (false);
        yield return new WaitForSeconds(1f);
        variableToChange (true);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status : MonoBehaviour
{
    //// Bools
    // Health
    private bool isAlive = true;
    private bool isInvulnerable = false;
    // Movement
    private bool isJumping = false;
    private bool canJump = true;
    private bool isDodging = false;
    private bool canDodge = true;
    private bool isBackflipping = false;
    private bool canBackflip = true;
    private bool isKnockedBack = false;
    // Shooting
    private bool canShoot = true;

    // Cooldowns
    private float jumpCooldown = 1f;
    private float backflipCooldown = 1f;
    private float dodgeCooldown = 1f;
    private float shootingCooldown = 1f;
    private float invulnerableCooldown = 2f;
    private float knockbackCooldown = 0.25f;

    // Constants
    private const float baseShootingCooldown = 1f;
    private const float baseDodgeLength = 0.25f;

    // Components
    private Stats stats = null;

    private void Awake() {
        stats = GetComponent<Stats>();
    }

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
                float dodgeLength = (stats.MoveSpeed.Value * 0.005f) + baseDodgeLength;
                StartCoroutine(StatusCooldown(result => isDodging = result, true, dodgeLength));
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
                float shotsPerSecond = Mathf.Clamp(stats.AttackSpeed.Value / 33f, baseShootingCooldown, 10f);
                shootingCooldown = 1 / shotsPerSecond;
                StartCoroutine( StatusCooldown(result => canShoot = result, false, shootingCooldown) );
            } else {
                canShoot = true;
            }
        }
    }

    /// <summary>
    /// Whether the Player can currently take damage or not.
    /// </summary>
    /// <value></value>
    public bool IsInvulnerable {
        get { return isInvulnerable || isBackflipping || isDodging || isJumping; }
        set {
            if (value == true) {
                isInvulnerable = true;
                StartCoroutine(StatusCooldown(result => isInvulnerable = result, true, invulnerableCooldown));
            } else {
                isInvulnerable = false;
            }
        }
    }


    public bool IsKnockedBack {
        get { return isKnockedBack; }
        set {
            if (value == true) {
                StartCoroutine(StatusCooldown(result => isKnockedBack = result, true, knockbackCooldown));
            } else {
                isKnockedBack = false;
            }
        }
    }

    // Basic getters
    public bool IsAlive => isAlive;
    public bool CanJump => canJump;
    public bool CanDodge => canDodge;
    public bool CanBackflip => canBackflip;
    public float InvulnerableCooldown => invulnerableCooldown;


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


    ///// EVENT LISTENERS
    private void OnEnable() {
        Health.onPlayerDeath += OnPlayerDeath;
    }
    private void OnDisable() {
        Health.onPlayerDeath -= OnPlayerDeath;
    }
    private void OnPlayerDeath() {
        isAlive = false;
    }
}

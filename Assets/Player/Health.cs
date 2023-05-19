using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Everything related to tracking and updating the Player's Health.
/// </summary>
public class Health : MonoBehaviour
{
    [Header("Health System")]
    [SerializeField][Tooltip("Objects in these Physics Layers will cause the Player to take damage")]
    private LayerMask damageLayer;

    // Health values
    private int baseHearts = 1;
    private int maxHearts;
    private int health;

    // Components
    private Player player;
    private Stats stats;

    // Events
    public static event Action onPlayerDamaged;
    public static event Action onPlayerDeath;


    private void Awake() {
        // Get components
        player = GetComponent<Player>();
        stats = GetComponent<Stats>();

        // Get stats & set vars
        baseHearts = stats.Health.baseValue;
        int extraHearts = Mathf.FloorToInt(stats.Health.Value / stats.Health.valueModifier);
        maxHearts = baseHearts + extraHearts;
        health = maxHearts * 2; // each heart is worth 2 HP
        Debug.Log($"Health stat: {stats.Health.Value} -> Extra Hearts: {extraHearts}");
    }


    /// <summary>
    /// Reduces the amount of health the Player has by a certain amount
    /// </summary>
    /// <param name="amount">Amount to reduce health by. Default: 1</param>
    public void TakeDamage(int amount=1) {
        if (player.Status.IsInvulnerable) {
            Debug.Log("Player can't take damage right now. They are invulnerable!");
            return;
        }

        player.Status.IsInvulnerable = true;    // makes player invulernable to damage for a short while
        health -= amount;                       // subtracts health
        StartCoroutine(DamagedAnimation());     // plays nice blinking animation

        // Triggers the event
        onPlayerDamaged?.Invoke();

        // DEATH!
        if (health <= 0) {
            onPlayerDeath?.Invoke();
        }
    }


    /// <summary>
    /// Trigger a nice Blinking animation on the player sprite.
    /// </summary>
    private IEnumerator DamagedAnimation() {
        player.Animator.SetBool("Blink", player.Status.IsInvulnerable);
        yield return new WaitForSeconds(player.Status.InvulnerableCooldown);
        player.Animator.SetBool("Blink", player.Status.IsInvulnerable);
    }


    // Getters
    /// <summary>Maximum amount of hearts the Player can have.</summary>
    public int MaxHearts => maxHearts;
    /// <summary>The Player's current health points.</summary>
    public int HP => health;
    /// <summary>Objects in these Physics Layers will cause the Player to take damage.</summary>
    public LayerMask DamageLayer => damageLayer;
}

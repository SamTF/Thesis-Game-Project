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
    public static event Action onHealthChange;

    // Subscribing to events
    private void OnEnable() {
        Player.onSpriteUpdated += UpdateHearts;
    }
    private void OnDisable() {
        Player.onSpriteUpdated -= UpdateHearts;
    }


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
        if (player.Status.IsInvulnerable)   yield return null;

        player.Animator.SetBool("Blink", true);
        yield return new WaitForSeconds(player.Status.InvulnerableCooldown);
        player.Animator.SetBool("Blink", false);
    }

    /// <summary>
    /// Regain an amount of Health Points.
    /// </summary>
    /// <param name="amount">Amount of HP to heal.</param>
    public void Heal(int amount = 2) {
        health += amount;
        health = Mathf.Clamp(health, 1, maxHearts * 2);

        onHealthChange?.Invoke();
    }

    /// <summary>
    /// Update the amount of hearts and heal to full health on player level up
    /// </summary>
    private void UpdateHearts() {
        int extraHearts = Mathf.FloorToInt(stats.Health.Value / stats.Health.valueModifier);
        maxHearts = baseHearts + extraHearts;
        health = maxHearts * 2; // each heart is worth 2 HP
        Debug.Log($"Health stat: {stats.Health.Value} -> Extra Hearts: {extraHearts}");

        onHealthChange?.Invoke();
    }


    // Getters
    /// <summary>Maximum amount of hearts the Player can have.</summary>
    public int MaxHearts => maxHearts;
    /// <summary>The Player's current health points.</summary>
    public int HP => health;
    /// <summary>Objects in these Physics Layers will cause the Player to take damage.</summary>
    public LayerMask DamageLayer => damageLayer;
    /// <summary>Checks if the Player is at full health</summary>
    public bool FullHealth => health == (maxHearts * 2);
    /// <summary>RNG chance to spawn a Heart (figured I should put this here for now)</summary>
    public bool SpawnHeart => UnityEngine.Random.Range(0, 7) >= 6;
}

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
    [SerializeField][Tooltip("Object in these Physics Layers will cause the Player to take damage")]
    private LayerMask damageLayer;

    // Health values
    private const int baseHearts = 1;
    private int maxHearts;
    private int health;

    // Components
    private Player player;
    private Stats stats;

    // Events
    public static event Action onPlayerDamaged;
    public static event Action onPlayerDeath;


    private void Awake() {
        player = GetComponent<Player>();
        stats = GetComponent<Stats>();

        int extraHearts = Mathf.FloorToInt(stats.Health.Value / 10f);
        Debug.Log($"Health stat: {stats.Health.Value} -> Extra Hearts: {extraHearts}");
        maxHearts = baseHearts + extraHearts;
        health = maxHearts * 2; // each heart is worth 2 HP
    }

    // Checking for collisions
    private void OnTriggerEnter2D(Collider2D other) {
        if(((1<<other.gameObject.layer) & damageLayer) != 0) {
            Debug.Log($"[HEALTH] >>> Ouch! Player was damaged by {other.name}");
            TakeDamage();
        }
    }

    /// <summary>
    /// Reduces the amount of health the Player has by a certain amount
    /// </summary>
    /// <param name="amount">Amount to reduce health by. Default: 1</param>
    private void TakeDamage(int amount=1) {
        health -= amount;
        onPlayerDamaged?.Invoke();

        if (health <= 0) {
            onPlayerDeath?.Invoke();
        }
    }

    

    // Getters
    /// <summary>Maximum amount of hearts the Player can have.</summary>
    public int MaxHearts => maxHearts;
    /// <summary>The Player's current health points.</summary>
    public int HP => health;
}

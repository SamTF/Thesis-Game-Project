using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Everything related to tracking and updating the Player's Health.
/// </summary>
public class Health : MonoBehaviour
{
    private const int baseHearts = 1;
    private int maxHearts;
    private int health;

    // Components
    private Player player;
    private Stats stats;

    private void Awake() {
        player = GetComponent<Player>();
        stats = GetComponent<Stats>();

        int extraHearts = Mathf.FloorToInt(stats.Health.Value / 10f);
        Debug.Log($"Health stat: {stats.Health.Value} -> Extra Hearts: {extraHearts}");
        maxHearts = baseHearts + extraHearts;
        health = maxHearts * 2; // each heart is worth 2 HP
    }

    // Getters
    /// <summary>Maximum amount of hearts the Player can have.</summary>
    public int MaxHearts => maxHearts;
    /// <summary>The Player's current health points.</summary>
    public int HP => health;
}

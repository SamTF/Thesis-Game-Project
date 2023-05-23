using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Everything related to tracking and updating the Player's Stamina.
/// Stamina governs when the Player can dodge or backflip.
/// </summary>
public class Stamina : MonoBehaviour
{
    // Stamina values
    private int baseStaminaOrbs = 1;
    private int maxOrbs;
    private int _stamina;
    private int _maxStamina;
    private float baseRechargeTime = 2;
    private bool isRecharging = false;

    // Components
    private Player player;
    private Stats stats;

    // Events
    /// <summary>Triggered when the Player spends stamina points.</summary>
    public static event Action onStaminaChange;

    // Subscribing to events
    private void OnEnable() {
        Player.onSpriteUpdated += UpdateStamina;
    }
    private void OnDisable() {
        Player.onSpriteUpdated -= UpdateStamina;
    }


    private void Awake() {
        // Get components
        player = GetComponent<Player>();
        stats = GetComponent<Stats>();

        // Get stats & set vars
        baseStaminaOrbs = stats.Stamina.baseValue;
        int extraOrbs = Mathf.FloorToInt(stats.Stamina.Value / stats.Stamina.valueModifier);
        maxOrbs = baseStaminaOrbs + extraOrbs;
        _maxStamina = maxOrbs * 2;
        _stamina = maxOrbs * 2; // each heart is worth 2 HP
        Debug.Log($"[STAMINA] Stamina stat: {stats.Stamina.Value} -> Extra Stamina Orbs: {extraOrbs}");
        Debug.Log($"[STAMINA] Total Stamina >>> {_stamina}");
    }

    /// <summary>
    /// Refreshes Stats after the Player Sprite has been updated.
    /// </summary>
    private void UpdateStamina() {
        baseStaminaOrbs = stats.Stamina.baseValue;
        int extraOrbs = Mathf.FloorToInt(stats.Stamina.Value / stats.Stamina.valueModifier);
        maxOrbs = baseStaminaOrbs + extraOrbs;
        _maxStamina = maxOrbs * 2;
        _stamina = maxOrbs * 2;

        // invoke event
        onStaminaChange?.Invoke();
    }


    /// <summary>
    /// Spend stamina points to perform an action. Initiaties the stamina refill, if it's not already running.
    /// </summary>
    public void UseStamina() {
        if (stamina <= 0) {
            Debug.LogError("[STAMINA] >>> Can't use any stamina because stamina is depleted!");
            return;
        }
            
        // decrement stamina points
        _stamina -= 1;

        // invoke event
        onStaminaChange?.Invoke();

        // Start the recharge coroutine if it's not already running
        if (!isRecharging)
            StartCoroutine(StaminaRecharge());
    }

    /// <summary>
    /// Recharge 1 point of Stamina every X seconds until filled.
    /// </summary>
    private IEnumerator StaminaRecharge() {
        // recharges until it reaches max stamina
        while (stamina < _maxStamina) {
            isRecharging = true;

            float cooldown = Mathf.Clamp(
                baseRechargeTime * (Player.instance.Stats.Stamina.Value / Player.instance.Stats.Stamina.valueModifier * 6),
                baseRechargeTime,
                0.75f
            );
            Debug.Log(cooldown);
            yield return new WaitForSeconds(cooldown);
            
            // add 1 stamina point every X seconds if not at Max and trigger the event
            if (stamina < _maxStamina) {
                _stamina++;
                onStaminaChange?.Invoke();
            }
        }

        // stops recharging
        isRecharging = false;
    }


    // Getters
    /// <summary>Maximum amount of Stamina Orbs the Player can have.</summary>
    public int MaxOrbs => maxOrbs;
    /// <summary>The Player's current stamina points.</summary>
    public int stamina => _stamina;
}

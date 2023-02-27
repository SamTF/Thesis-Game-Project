using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    [Header("Character Stats Sheet")]
    [SerializeField]
    private Stat[] stats =  new Stat[4];

    // Main Attributes
    private Stat attack = null;
    private Stat health = null;
    private Stat attackSpeed = null;
    private Stat moveSpeed = null;

    // Derived attributes - just a brainstorming list
    private float movementSpeed;
    private float dodgeDistance;
    private float dodgeLength;
    private float dodgeCooldown;
    private float jumpCooldown;
    private float backflipCooldown;
    private float shootingCooldown;

    private Dictionary<Color, Stat> colour2Stat = new Dictionary<Color, Stat>();
    private Dictionary<Attribute, Stat> attribute2Stat = new Dictionary<Attribute, Stat>();

    // currently hardcoded to 2-bit colour scheme
    // Match each stat to a colour value (light, midlight, middark, dark)
    // This must occur after the PaletteManager has loaded the palette
    private void Awake() {
        // Initialising the Stats
        attack = new Stat(Attribute.Attack, "attack", ColourValue.Dark, Palette.Colours.dark);
        health = new Stat(Attribute.Health, "health", ColourValue.Light, Palette.Colours.light);
        attackSpeed = new Stat(Attribute.AttackSpeed, "fire rate", ColourValue.MidDark, Palette.Colours.midDark);
        moveSpeed = new Stat(Attribute.MoveSpeed, "speed", ColourValue.MidLight, Palette.Colours.midLight);

        // Adding stats to list
        stats = new Stat[] { attack, health, attackSpeed, moveSpeed };

        // Adding stats to dictionaries
        foreach (Stat s in stats) {
            colour2Stat.Add(s.Colour, s);
            attribute2Stat.Add(s.Attribute, s);
        }
    }

    /// <summary>
    /// Converts the amount of coloured pixels into their respective stats
    /// </summary>
    /// <param name="colours">Array of Colour objects</param>
    public void GetStatsFromImage(Colour[] colours) {
        Debug.Log("[STATS] >>> Inferring stats from image colours...");

        // Reset all attribute values when refreshing stats
        foreach (Stat stat in stats) {
            stat.Value = 0;
        }

        // Setting the appropriate value for each stat based on their colour
        foreach (Colour c in colours) {
            // If the given colour is part of the Palette
            if (colour2Stat.ContainsKey(c.colour)) {
                colour2Stat[c.colour].Value = c.value;
            }
        }

        // Displaying the stats on the UI
        UIManager.instance.DisplayStats(stats);
    }

    /// <summary>
    /// Gets the Stat object of a given Attribute
    /// </summary>
    /// <param name="attr">Attribute Enum</param>
    /// <returns></returns>
    public Stat GetStat(Attribute attr) {
        return attribute2Stat[attr];
    }

    // Getters
    public Stat Attack => attribute2Stat[Attribute.Attack];
    public Stat Health => attribute2Stat[Attribute.Health];
    public Stat MoveSpeed => attribute2Stat[Attribute.MoveSpeed];
    public Stat AttackSpeed => attribute2Stat[Attribute.AttackSpeed];

}

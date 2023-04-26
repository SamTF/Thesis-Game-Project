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
    private Stat stamina = null;
    private Stat energy = null;
    // Weapon Stats
    private Stat shotSpeed = null;
    private Stat shotRange = null;
    private Stat shotAmount = null;
    private Stat shotSize = null;

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
        attack          = new Stat(Attribute.Attack,        "attack",       Palette.Colours[0]);
        health          = new Stat(Attribute.Health,        "health",       Palette.Colours[1]);
        attackSpeed     = new Stat(Attribute.AttackSpeed,   "fire rate",    Palette.Colours[2]);
        moveSpeed       = new Stat(Attribute.MoveSpeed,     "speed",        Palette.Colours[3]);
        stamina         = new Stat(Attribute.Stamina,       "stamina",      Palette.Colours[4]);
        energy          = new Stat(Attribute.Energy,        "energy",       Palette.Colours[5]);
        shotSpeed       = new Stat(Attribute.ShotSpeed,     "shot speed",   Palette.Colours[6]);
        shotRange       = new Stat(Attribute.ShotRange,     "shot range",   Palette.Colours[7]);
        shotAmount      = new Stat(Attribute.ShotAmount,    "shot amount",  Palette.Colours[8]);
        shotSize        = new Stat(Attribute.ShotSize,      "shot size",    Palette.Colours[9]);
        

        // Adding stats to list
        stats = new Stat[] { attack, health, attackSpeed, moveSpeed, stamina, energy, shotSpeed, shotRange, shotAmount, shotSize };

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
    public Dictionary<Color, Stat> Colour2Stat => colour2Stat;
    public Stat Attack => attribute2Stat[Attribute.Attack];
    public Stat Health => attribute2Stat[Attribute.Health];
    public Stat MoveSpeed => attribute2Stat[Attribute.MoveSpeed];
    public Stat AttackSpeed => attribute2Stat[Attribute.AttackSpeed];

}

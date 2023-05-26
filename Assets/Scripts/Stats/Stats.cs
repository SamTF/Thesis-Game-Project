using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    [Header("Character Stats Sheet")]
    [SerializeField]
    private Stat[] stats =  new Stat[6];

    // Main Attributes
    private Stat attack = null;
    private Stat health = null;
    private Stat attackRate = null;
    private Stat moveSpeed = null;
    private Stat stamina = null;
    // private Stat energy = null;
    // Weapon Stats
    private Stat shotSpeed = null;
    // private Stat shotRange = null;
    // private Stat shotAmount = null;
    // private Stat shotSize = null;

    // Icons
    /// <summary>The Icons for each stats as Sprite objects</summary>
    private Sprite[] iconSprites = new Sprite[6];
    /// <summary>The dimensions of the XP Icon sprite in px.</summary>
    private Vector2Int iconSize = new Vector2Int(16, 16);
    /// <summary>Name of the Stat Icons spritesheet file.</summary>
    private string iconName = "StatIcons";

    // Derived attributes - just a brainstorming list
    private float movementSpeed;
    private float dodgeDistance;
    private float dodgeLength;
    private float dodgeCooldown;
    private float jumpCooldown;
    private float backflipCooldown;
    private float shootingCooldown;

    // Dictionaries
    private Dictionary<Color, Stat> colour2Stat = new Dictionary<Color, Stat>();
    private Dictionary<Attribute, Stat> attribute2Stat = new Dictionary<Attribute, Stat>();

    // currently hardcoded to 2-bit colour scheme
    // Match each stat to a colour value (light, midlight, middark, dark)
    // This must occur after the PaletteManager has loaded the palette
    private void Awake() {
        iconSprites = ImageLoader.CreateAllSprites(iconName, "UI", iconSprites.Length, iconSize);

        // Initialising the Stats
        health          = new Stat(Attribute.Health,        "Health",       Palette.Colours[0], iconSprites[0], 1, 10, $"Num of Hearts\nyou can have");
        attack          = new Stat(Attribute.Attack,        "Attack",       Palette.Colours[1], iconSprites[1], 1, 30, $"Damage dealt\nby projectiles");
        moveSpeed       = new Stat(Attribute.MoveSpeed,     "Speed",        Palette.Colours[2], iconSprites[2], 2, 40, $"How fast you\nmove around");
        attackRate      = new Stat(Attribute.AttackRate,    "Fire rate",    Palette.Colours[3], iconSprites[3], 2, 25, $"Shots fired\nper second");
        shotSpeed       = new Stat(Attribute.ShotSpeed,     "Shot Spd",     Palette.Colours[4], iconSprites[4], 4, 16, $"Speed your \nshots\ntravel at");
        stamina         = new Stat(Attribute.Stamina,       "Stamina",      Palette.Colours[5], iconSprites[5], 3, 25, $"How often you\ncan dodge/jump");
        // stamina         = new Stat(Attribute.Stamina,       "Stamina",      Color.black,        iconSprites[5], 1, 10, $"How often you\ncan dodge/jump");
        // shotRange       = new Stat(Attribute.ShotRange,     "shot range",   Palette.Colours[6]);
        // shotAmount      = new Stat(Attribute.ShotAmount,    "shot amount",  Palette.Colours[7]);
        // shotSize        = new Stat(Attribute.ShotSize,      "shot size",    Palette.Colours[8]);
        

        // Adding stats to list
        stats = new Stat[] {health, attack, moveSpeed, attackRate, shotSpeed, stamina };

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
    public Stat[] StatsArray => stats;
    public Stat Health => health;
    public Stat Attack => attack;
    public Stat MoveSpeed => moveSpeed;
    public Stat AttackRate => attackRate;
    public Stat ShotSpeed => shotSpeed;
    public Stat Stamina => stamina;

}

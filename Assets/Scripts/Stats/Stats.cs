using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    [Header("Character Stats Sheet")]
    [SerializeField]
    private Stat[] stats =  new Stat[4];

    private Stat attack = null;
    private Stat health = null;
    private Stat attackSpeed = null;
    private Stat moveSpeed = null;

    private Dictionary<Color, Stat> colour2Stat = new Dictionary<Color, Stat>();

    // currently hardcoded to 2-bit colour scheme
    // Match each stat to a colour value (light, midlight, middark, dark)
    // This must occur after the PaletteManager has loaded the palette
    private void Start() {
        attack = new Stat("attack", ColourValue.Dark, Palette.Colours.dark);
        health = new Stat("health", ColourValue.Light, Palette.Colours.light);
        attackSpeed = new Stat("fire rate", ColourValue.MidDark, Palette.Colours.midDark);
        moveSpeed = new Stat("speed", ColourValue.MidLight, Palette.Colours.midLight);

        stats = new Stat[] {attack, health, attackSpeed, moveSpeed};

        foreach (Stat s in stats) {
            colour2Stat.Add(s.Colour, s);
        }
    }

    /// <summary>
    /// Converts the amount of coloured pixels into their respective stats
    /// </summary>
    /// <param name="colours">Array of Colour objects</param>
    public void GetStatsFromImage(Colour[] colours) {
        Debug.Log("[STATS] >>> Inferring stats from image colours...");

        foreach (Colour c in colours) {
            // If the given colour is part of the Palette
            if (colour2Stat.ContainsKey(c.colour)) {
                colour2Stat[c.colour].Value = c.value;
            }
        }

        foreach (Stat s in stats)
        {
            Debug.Log($"{s.Name} - {s.Value}");
        }

        // Displaying the stats on the UI
        UIManager.instance.DisplayStats(stats);
    }

}

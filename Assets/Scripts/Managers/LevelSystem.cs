using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

/// <summary>
/// This class is in charge of collecting XP and levelling up the Player.
/// </summary>
public class LevelSystem : MonoBehaviour
{
    [Header("LEVEL SYSTEM")]

    [SerializeField]
    private int level = 1;
    [SerializeField]
    private int xp = 0;
    [SerializeField]
    private int xpToLevelUp = 10;

    [SerializeField]
    private Player player;

    private List<Stat> unlockedStats = new List<Stat>();
    private List<Color> unlockedColours = new List<Color>();

    // Events
    public static event Action onXPGained;
    public static event Action onLevelUp;

    /// Singleton thing
    private static LevelSystem _instance = null;
    public static LevelSystem instance {
        get {return _instance;}
    }


    // Subscribing to Events
    private void OnEnable() {
        LevelUpUI.onNewColourChosen += UnlockColour;
    }
    private void OnDisable() {
        LevelUpUI.onNewColourChosen -= UnlockColour;
    }
    
    private void Awake()
    {
        // Singleton Thing
        if (instance == null)   { _instance = this; }
        else                    { Destroy(gameObject); }

        DontDestroyOnLoad(this.gameObject);

        if (player == null)
            player = Player.instance;
    }


    private void Start() {
        UnlockColour(Palette.Colours[level-1]);

        foreach (Color c in Palette.Colours[0..2])
        {
            Debug.Log(ColorUtility.ToHtmlStringRGBA(c));
        }
    }


    /// <summary>
    /// Increase current XP and potentially level up!
    /// </summary>
    /// <param name="gainedXP">Amount of XP to gain. (Default = 1)</param>
    public void GainXP(int gainedXP=1) {
        // Increment current XP with gained XP
        xp += gainedXP;

        // Check if enough to level up AND is not at the max level already
        if (xp >= xpToLevelUp && level < Palette.NumOfColours) {
            level++;                // increment level
            xp -= xpToLevelUp;      // keep remainder XP
            onLevelUp?.Invoke();    // trigger the level up event
        }

        // Trigger the XP Gain Event
        onXPGained?.Invoke();

        // test
        float progress = ((float)xp / (float)xpToLevelUp) * 100f;
        int progressRounded = Mathf.FloorToInt(progress / 20) * 20;
        Debug.Log($"Progress >>> {progress}%");
        Debug.Log($"Rounded >>> {progressRounded}%");
        Debug.Log(progressRounded / 20);
    }

    /// <summary>
    /// A new Colour to the Player's Palette -> list of available colours to use when drawing the sprite.
    /// </summary>
    /// <param name="newColour">The Colour that the Player chose to unlock</param>
    private void UnlockColour(Color newColour) {
        // Check if the colour given exists in the Palette!
        if (!Array.Exists( Palette.Colours, c => c == newColour )) {
            Debug.LogError("[LEVEL SYSTEM] >>> Unlocked colour does not exist in the Colour Palette!");
            return;
        }

        Stat newStat = Player.instance.Stats.Colour2Stat[newColour];

        unlockedColours.Add(newColour);
        unlockedStats.Add(newStat);
    }


    // Public Getters
    public int Level => level;
    public int CurrentXP => xp;
    public int XPToLevelUp => xpToLevelUp;

    public int Progress {
        get {
            float progress = ((float)xp / (float)xpToLevelUp) * 100f;
            int progressRounded = Mathf.FloorToInt(progress / 20);
            return progressRounded;
        }
    }

    /// <summary>
    /// The Colours that the Player has unlocked so far by levelling up.
    /// </summary>
    public Color[] UnlockedColours => unlockedColours.ToArray();
    // public Stat[] UnlockedStats => unlockedStats.ToArray();

    /// <summary>The Stats that the Player has unlocked so far by levelling up.</summary>
    public Stat[] UnlockedStats {
        get {
            return unlockedColours.Select(x => Player.instance.Stats.Colour2Stat[x]).ToArray();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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


    // Events
    public static event Action onXPGained;
    public static event Action onLevelUp;

    /// Singleton thing
    private static LevelSystem _instance = null;
    public static LevelSystem instance
    {
        get {return _instance;}
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


    /// <summary>
    /// Increase current XP and potentially level up!
    /// </summary>
    /// <param name="gainedXP">Amount of XP to gain. (Default = 1)</param>
    public void GainXP(int gainedXP=1) {
        // Increment current XP with gained XP
        xp += gainedXP;

        // Check if enough to level up
        if (xp >= xpToLevelUp) {
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
}

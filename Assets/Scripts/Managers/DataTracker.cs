using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class DataTracker
{
    private static List<Sprite> playerSprites = new List<Sprite>();
    private static string characterHash = null;

    static DataTracker() {
        Debug.Log("Data Tracker has woken up!");

        Player.onSpriteUpdated += OnPlayerSpriteUpdated;
    }

    public static void WakeUp() {
        Debug.Log("Data Tracker has woken up!");
    }

    /// <summary>
    /// Keep track of all Player sprites
    /// </summary>
    private static void OnPlayerSpriteUpdated() {
        Debug.Log("DATA TRACKERR AAA!!! AA!! AAAAAA");

        // add new sprite to list
        playerSprites.Add(Player.instance.Sprite);
    }

    private static void OnPlayerDeath() {
        int attempts = 0;

        // if the key already exists, get its value and increment it
        if (PlayerPrefs.HasKey("attempts")) {
            attempts = PlayerPrefs.GetInt("attempts");
            attempts++;
        }
        // otherwise, create key
        PlayerPrefs.SetInt("attempts", attempts);

        // reset variables
        playerSprites = null;
        characterHash = null;
    }

    /// <summary>
    /// Hashes a Texture2D into a 6-character string.
    /// </summary>
    /// <param name="texture">Texture object to hash</param>
    /// <returns>6-character string</returns>
    public static string HashTexture(Texture2D texture) {
        // return the current character hash if there already is one
        if (characterHash != null && LevelSystem.instance.Level > 0)
            return characterHash;

        // otherwise create a new hash

        // encode texture to byte array
        byte[] texBytes = texture.EncodeToPNG();

        // Hash the bytes to generate a unique filenames - why not?
        Hash128 hash = new Hash128();
        hash.Append(texBytes);

        // convert to string and cap to 6 characters
        characterHash = hash.ToString().Substring(0,6);

        return characterHash;
    }


    /// <summary>Number of times that the Player has played the game.</summary>
    public static int Attempts => PlayerPrefs.HasKey("attempts") ? PlayerPrefs.GetInt("attempts") : 0;
    /// <summary>Array of Character drawn by the Player in this run as Sprites.</summary>
    public static Sprite[] PlayerSprites => playerSprites.ToArray();
    /// <summary>Array of Character drawn by the Player in this run as Textures.</summary>
    public static Texture2D[] PlayerDrawings => playerSprites.Select(x => x.texture).ToArray();

}

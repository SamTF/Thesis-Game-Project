using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Player player = null;

    /// Singleton thing
    private static GameManager _instance = null;
    public static GameManager instance
    {
        get {return _instance;}
    }

    private void Awake()
    {
        // Singleton Thing
        if (instance == null)   { _instance = this; }
        else                    { Destroy(gameObject); }

        DontDestroyOnLoad(this.gameObject);

        ModManager.ListMods();
        Palette.LoadPalette();
    }

    // Getters
    /// <summary>The Player object in the scene.</summary>
    public Player Player => player;
    /// <summary>The current position of the Player in world space. </summary>
    public Vector2 PlayerPosition => player.transform.position;
    /// <summary>The current Sprite image of the Player's body.</summary>
    public Sprite PlayerSprite => player.Sprite;
}

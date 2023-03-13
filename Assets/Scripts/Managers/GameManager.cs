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
    public Player Player => player;
    public Transform PlayerTrans => player.transform;
    public Vector2 PlayerPosition => player.transform.position;
}

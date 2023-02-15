using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
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
        Palette.WakeUp();
    }
}

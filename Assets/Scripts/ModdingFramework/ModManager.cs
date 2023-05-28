using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

/// <summary>
/// Responsible for loading player created content from disk and inserting it into the game.
/// </summary>
public static class ModManager
{
    /// MODDING CONSTANTS
    private const string modFolderName = "CUSTOM";
    private const string fileType = "png";
    private const string drawingsFolder = "MyDrawings";
    
    // Vars
    private static string modDirectory = null;
    private static string[] modList = null;

    // Class Construction
    static ModManager() {
        Debug.Log("[ModManager] >>> INITIALISED !!");
        modDirectory = $"{Application.dataPath}/{modFolderName}";
        Debug.Log($"[ModManager] >>> Mod Directory: {modDirectory}");

        modList = GetMods();
    }

    /// <summary>
    /// Checks the Mod Directory for any custom files and returns them as an array.
    /// If the folder doesn't exist yet, it is created.
    /// </summary>
    public static void ListMods() {
        Debug.Log("[ModManager] >>> Scanning for mods...");

        // Checks if the mod directory exists, and if not, creates it
        if (!Directory.Exists(modDirectory)) {
            Debug.LogError($"[ModManager] >>> Custom Mod directory does not exist (expected {modDirectory})");
            Debug.Log("[ModManager] >>> Creating Mod Directory...");

            Directory.CreateDirectory(modDirectory);
            return;
        }

        // Check if Drawings directory exists
        if(!Directory.Exists($"{modDirectory}/{drawingsFolder}")) {
            Directory.CreateDirectory($"{modDirectory}/{drawingsFolder}");
        }

        Debug.Log("[ModManager] >>> Mod Directory found!");

        // Listing all files in Mod folder
        DirectoryInfo modDirectoryInfo = new DirectoryInfo(modDirectory);
        FileInfo[] ls = modDirectoryInfo.GetFiles("*.*").Where(file => !file.Name.EndsWith(".meta")).ToArray();

        if (ls.Length == 0) {
            Debug.Log("[ModManager] >>> No files found in Mod Directory! Try adding some :)");
            return;
        }

        foreach (FileInfo file in ls) {
            Debug.Log($"[ModManager] >>> {file.Name}");
        }

    }

    public static string[] GetMods() {
        // Checks if the mod directory exists, and if not, creates it
        if (!Directory.Exists(modDirectory)) {
            Directory.CreateDirectory(modDirectory);
            return new string[0];
        }

        // Listing all files in Mod folder
        DirectoryInfo modDirectoryInfo = new DirectoryInfo(modDirectory);
        FileInfo[] ls = modDirectoryInfo.GetFiles("*.*").Where(file => !file.Name.EndsWith(".meta")).ToArray();

        // similar to JS's array.map
        string[] mods = ls.Select(file => file.Name).ToArray();
        return mods;
    }

    ///<summary>Checks whether a file exists in the Mod Directory.</summary>
    ///<param name="filePath">File's full name with extension and path. Ex: "player.png"</param>
    public static bool FileExists(string filePath) {
        return File.Exists($"{modDirectory}/{filePath}");
    }

    ///<summary>Checks whether a file exists in the load mod list.</summary>
    ///<param name="filename">File's full name with extension. Ex: "player.png"</param>
    public static bool ModExists(string filename) {
        return modList.Contains(filename);
    }

    // Getters
    ///<summary>The full path to the Mods folder on disk.</summary>
    public static string ModDirectory => modDirectory;
    ///<summary>List of all files in the Mods folder.</summary>
    public static string[] ModList => modList;
    ///<summary>The file extension used for mod files.</summary>
    public static string FileType => fileType;
    /// <summary>Directory where Players drawings are saved</summary>
    public static string DrawingsDirectory => $"{modDirectory}/{drawingsFolder}";
}

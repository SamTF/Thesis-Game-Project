using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class Palette
{
    // Class Vars
    private const int paletteWidth = 16;
    private const string fileType = ".png";
    private const string fileName = "palette";
    private const int amountOfColours = 6;
    private const string bgColourName = "background";

    private static Color[] coloursArray = null;
    private static Colour[] colourObjs = null;
    private static Color[] dummyColors = { Color.white, Color.black };
    private static HSVColour bgColour;


    // Class Constructor
    static Palette() {
        Debug.Log("Palette has woken up!");

        LoadBackground();
    }

    public static void LoadPalette(bool forceDefault = false) {
        Texture2D paletteTex = null;

        // Forcefully ignore custom palette and use default palette if option has been specified
        if (forceDefault) {
            Debug.Log("Reverting to default palette...");
            paletteTex = Resources.Load(fileName) as Texture2D;
        }

        // Checking is user created custom palette
        else if (ModManager.ModExists($"{fileName}{fileType}")) {
            Debug.Log("Custom palette found! Importing...");
            paletteTex = ImageLoader.LoadTextureFromFile($"{fileName}{fileType}");
            
        // Load default palette if no custom one was found
        } else {
            Debug.Log($"No custom Colour Palette found. Loading default palette [{fileName}]...");
            paletteTex = Resources.Load(fileName) as Texture2D;
        }

        // Getting all the colours in the palette (only the Color value)
        colourObjs = ImageAnalyser.Analyse(paletteTex);
        coloursArray = colourObjs.Select(c => c.colour).ToArray();

        // Checking that the Custom Palette contains enough colours
        if (coloursArray.Length < amountOfColours) {
            Debug.LogError("[PALETTE] >>> Custom Palette does not contain enough colours!");
            LoadPalette(forceDefault:true);
        }

        // Displaying it on the UI for debugging purposes
        // UIManager.instance.DisplayColours(colourObjs);
        foreach (Colour c in colourObjs) {
            Debug.Log($"{c.hexColour} - {c.value}");
        }
    }

    private static void LoadBackground() {
        // Checking is user created a background colour file
        if (ModManager.ModExists($"{bgColourName}{fileType}")) {
            Debug.Log("Custom background found! Importing...");
            Texture2D backgroundTex = ImageLoader.LoadTextureFromFile($"{bgColourName}{fileType}");
            Color color = ImageAnalyser.GetColours(backgroundTex)[0];
            bgColour = new HSVColour(color);
            
        // Use default background colour if no custom one was given
        } else {
            Color rgbCamera = Camera.main.backgroundColor;
            bgColour = new HSVColour(rgbCamera);
        }

        Debug.Log(bgColour);
    }

    /// <summary>An array of all the Colors used in the game palette.</summary>
    public static Color[] Colours => coloursArray;
    /// <summary>An array of all the Colors used in the game palette as Colour Objects.</summary>
    public static Colour[] ColourObjects => colourObjs;
    /// <summary>Amount of colours present in the Palette.</summary>
    public static int NumOfColours => amountOfColours;
    /// <summary>"Dumb" colours that have no stat but can be used for purely cosmetic purposes.</summary>
    public static Color[] DummyColors => dummyColors;
    /// <summary>The main colour, used as the level background.</summary>
    public static HSVColour BackgroundColour => bgColour;
}

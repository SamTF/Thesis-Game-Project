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
    private const int amountOfColours = 10;

    private static int segments = 4;
    private static Color[] coloursArray = null;
    private static Colour[] colourObjs = null;
    private static PaletteColours coloursPalette = null;


    // Class Constructor
    static Palette() {
        Debug.Log("Palette has woken up!");
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
            // LoadPalette(true);
        }

        // Creating Palette colour object
        coloursPalette = new PaletteColours(coloursArray);

        // Displaying it on the UI for debugging purposes
        // UIManager.instance.DisplayColours(colourObjs);
        foreach (Colour c in colourObjs) {
            Debug.Log($"{c.hexColour} - {c.value}");
        }
    }

    public static Color[] Colours => coloursArray;
    public static Colour[] ColourObjects => colourObjs;
    // public static PaletteColours Colours => coloursPalette;
}

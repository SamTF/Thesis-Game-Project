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

    private static int segments = 4;
    private static Color[] coloursArray = null;
    private static PaletteColours coloursPalette = null;


    // Class Constructor
    static Palette() {
        Debug.Log("Palette has woken up!");
    }

    public static void WakeUp() {
        Texture2D paletteTex = null;

        // Checking is user created custom palette
        if (ModManager.ModExists($"{fileName}{fileType}")) {
            Debug.Log("Custom palette found! Importing...");
            paletteTex = ImageLoader.LoadTextureFromFile($"{fileName}{fileType}");
            
            // Validating the user's palette
            // ...
        } else {
            Debug.Log($"No custom Colour Palette found. Loading default palette [{fileName}]...");
            paletteTex = Resources.Load(fileName) as Texture2D;
        }

        // Getting all the colours in the palette (only the Color value)
        Colour[] colourObjs = ImageAnalyser.Analyse(paletteTex);
        coloursArray = colourObjs.Select(c => c.colour).ToArray();

        // Creating Palette colour object
        coloursPalette = new PaletteColours(coloursArray);

        // Displaying it on the UI for debugging purposes
        // UIManager.instance.DisplayColours(colourObjs);
    }

    public static Color[] ColoursArray => coloursArray;
    public static PaletteColours Colours => coloursPalette;
}

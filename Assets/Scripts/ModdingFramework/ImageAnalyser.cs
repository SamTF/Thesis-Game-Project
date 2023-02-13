using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// A Utility that analyses all the pixels in an image, counting the occurences of each colour present.
/// </summary>
public static class ImageAnalyser
{
    // Loops thru every pixel in the image and gets its colour value
    public static void Analyse(Texture2D image) {

        // 1. Loops thru every pixel in the image and gets its colour value
        List<Color> colours = new List<Color>();

        for (int x = 0; x < image.width; x++) {
            for (int y = 0; y < image.height; y++)
            {
                Color pixelColour = image.GetPixel(x, y);
                colours.Add(pixelColour);
                Debug.Log($"({x}, {y}) - {pixelColour}");
            }
        }

        // 2. Filters only the unique colours
        List<Color> uniqueColours = new List<Color>();
        Dictionary<Color, int> colourCount = new Dictionary<Color, int>();

        foreach (Color c in colours)
        {
            // if the current colour is not yet in the list, add it
            if (!uniqueColours.Contains(c)) {
                uniqueColours.Add(c);       // add the colour to the list
                colourCount.Add(c, 1);      // create dict key
            // increment occurences
            } else {
                colourCount[c]++;
            }
        }

        // Creating a list of my custom Colour Objects that hold both the Color value and its count value
        List<Colour> colourObjs = new List<Colour>();
        foreach (Color c in uniqueColours)
        {
            Colour colour = new Colour {
                colour = c,
                value = colourCount[c]
                // value = colours.FindAll(x => x == c).Count
            };

            colourObjs.Add(colour);
        }

        // Displaying the colours on the UI
        UIManager.instance.DisplayColours(colourObjs.ToArray());
    }
}

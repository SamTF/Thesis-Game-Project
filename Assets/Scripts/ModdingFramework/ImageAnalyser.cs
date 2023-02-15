using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// A Utility that analyses all the pixels in an image, counting the occurences of each colour present.
/// </summary>
public static class ImageAnalyser
{
    /// <summary>
    /// Loops thru every pixel in the image and gets its colour value.
    /// </summary>
    /// <param name="image">Texture2D object of the image you want to analyse.</param>
    /// <returns>Array of Colour objects: Colour: {Color, Occurences}</returns>
    public static Colour[] Analyse(Texture2D image) {

        // 1. Loops thru every pixel in the image and gets its colour value
        List<Color> colours = new List<Color>();

        for (int x = 0; x < image.width; x++) {
            for (int y = 0; y < image.height; y++) {
                Color pixelColour = image.GetPixel(x, y);
                colours.Add(pixelColour);
            }
        }

        // 2. Filters only the unique colours
        List<Color> uniqueColours = new List<Color>();
        Dictionary<Color, int> colourCount = new Dictionary<Color, int>();

        foreach (Color c in colours) {
            // ignore if it's transparent
            if (c.a < 1) {
                continue;
            }
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
        foreach (Color c in uniqueColours) {
            Colour colour = new Colour {
                colour = c,
                value = colourCount[c]
                // value = colours.FindAll(x => x == c).Count
            };

            colourObjs.Add(colour);
        }

        // Return the colour objects
        return colourObjs.ToArray();
    }

    /// <summary>
    /// Gets all the uniqte colours found in the given image. (does not count them)
    /// </summary>
    /// <param name="image">Image whose colours you want to extract</param>
    /// <returns>Color Array of all unique colours found</returns>
    public static Color[] GetColours(Texture2D image) {
        // 1. Loops thru every pixel in the image and gets its colour value
        List<Color> colours = new List<Color>();

        for (int x = 0; x < image.width; x++) {
            for (int y = 0; y < image.height; y++) {
                Color pixelColour = image.GetPixel(x, y);
                colours.Add(pixelColour);
            }
        }

        // 2. Filters only the unique non-transparent colours
        List<Color> uniqueColours = new List<Color>();
        foreach (Color c in colours) {
            // ignore if it's transparent
            if (c.a < 1) {
                continue;
            }
            // if the current colour is not yet in the list, add it
            if (!uniqueColours.Contains(c)) {
                uniqueColours.Add(c);       // add the colour to the list
            }
        }

        return uniqueColours.ToArray();
    }
}

using UnityEngine;
using System.IO;

public static class ImageLoader
{
    // CONSTANTS
    private const float pixelsPerUnit = 16f;
    private static Vector2 pivot =  new Vector2(0.5f, 0.5f);
    private static Vector2 pivotB = new Vector2(0.5f, 0f);
    private static Vector2 pivotBL = new Vector2(0f, 0f);

    /// <summary>
    /// Loads a custom image from disk and creates a Sprite Object from it.
    /// Optionally you can specify an exact size that the custom image must have.
    /// </summary>
    public static Sprite LoadSprite(string filename) {
        Texture2D spriteTexture = LoadTextureFromFile(filename);    // Loads the requested image file as a Texture2D Object
        if (spriteTexture == null)  return null;                    // Checks if it was successful

        // Creating the Sprite
        Sprite newSprite = Sprite.Create(
            spriteTexture,
            new Rect(0, 0, spriteTexture.width, spriteTexture.height),
            pivotB,
            pixelsPerUnit
        );

        return newSprite;
    }


    /// <summary>
    /// Loads a PNG or JPG file from disk into a Texture2D Object.
    /// Returns null if loading fails
    /// </summary>
    public static Texture2D LoadTextureFromFile(string filename) {
        string filePath = $"{ModManager.ModDirectory}/{filename}";

        // Checking if the file exists
        if (!File.Exists(filePath)) {
            Debug.LogError($"No such image: {filePath}");
            return null;
        }

        // Creating the texture
        byte[] fileData = File.ReadAllBytes(filePath);  // reading the Image's data into an array of bytes
        Texture2D texture = new Texture2D(1,1);         // creating a Texture2D object to house it
        texture.filterMode = FilterMode.Point;          // Setting the correct filter mode (no filtering)
        

        // checks if the data is readable
        if (!texture.LoadImage(fileData)) {     
            return null;                                // return null is image is unreadable
        }

        // Return the texture if everything was successful
        return texture; 
    }
}

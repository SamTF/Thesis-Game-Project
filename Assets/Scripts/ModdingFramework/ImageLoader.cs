using UnityEngine;
using System.IO;

/// <summary>
/// This class features helper methods to load images from disk and create Sprite objects from them.
/// </summary>
public static class ImageLoader
{
    // CONSTANTS
    private const float pixelsPerUnit = 16f;
    

    /// <summary>
    /// Loads a custom image from disk and creates a Sprite Object from it.
    /// Optionally you can specify an exact size that the custom image must have.
    /// </summary>
    /// <param name="filename">Name of the image file to load from the mod folder.</param>
    /// <param name="imageSize">Size that the image must have</param>
    /// <returns>Returns a Sprite object to add to the sprite renderer.</returns>
    public static Sprite LoadSprite(string filename, Vector2Int? imageSize = null) {
        Texture2D spriteTexture = LoadTextureFromFile(filename);    // Loads the requested image file as a Texture2D Object
        if (spriteTexture == null)  return null;                    // Checks if it was successful

        // Checking size requirements - returns null if sizes don't match
        if (imageSize.HasValue) {
            Vector2Int textureSize = new Vector2Int(spriteTexture.width, spriteTexture.height); // Storing the texture dimensions into a Vector2Int
            if (textureSize != imageSize) {                                                     // Comparing its dimensions with the required dimensions
                Debug.LogError($"[ImageLoader] Image {filename} ({textureSize}) does not have the required dimensions of {imageSize}");
                return null;
            }
        }

        // Creating the Sprite
        Sprite newSprite = CreateSprite(spriteTexture, Pivot.BottomCenter);

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

    /// <summary>
    /// Creates a Sprite Object from a texture with a custom Rect/slice.
    /// </summary>
    /// <param name="tex">The spritesheet image.</param>
    /// <param name="pivot">Sprite pivot point.</param>
    /// <param name="start">Where to start the rect. (Only set this if creating a sprite from a spritesheet)</param>
    /// <param name="imageSize">Dimensions of the Sprite. (Only set this if creating a sprite from a spritesheet)</param>
    /// <returns>A Sprite object.</returns>
    public static Sprite CreateSprite(Texture2D tex, Pivot pivot, int start=0, Vector2Int? imageSize=null) {
        // Getting the dimensions for the Sprite Rect. Using Texture dimensions if none were given.
        Vector2Int rectSize = imageSize == null ? new Vector2Int(tex.width, tex.height) : imageSize.Value;

        // Creating the Sprite object.
        Sprite newSprite = Sprite.Create(
            tex,
            new Rect(start, 0, rectSize.x, rectSize.y),
            pivot.value,
            pixelsPerUnit
        );

        return newSprite;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A GameObject with this component can have its Sprite changed by the player in the mod directory.
/// </summary>
public class ModdableSprite : MonoBehaviour
{
    [SerializeField][Tooltip("Does this gameObject's sprite need to have a specific size?")]
    private bool enforceSize = true;
    [SerializeField][Tooltip("What should the image file for this sprite be called? Default: The GameObject's name")]
    private string modFileName = null;
    [SerializeField][Tooltip("Count the pixels of each colour in this image?")]
    private bool analyseImage = false;
    [SerializeField][Tooltip("Whether the SpriteRenderer component is a child of this GameObject.")]
    private bool spriteIsChild = false;
    [SerializeField][Tooltip("Allow reloading the sprite texture while the game is running (press R)")]
    private bool allowHotReload = false;

    private string modFileType = "png";

    // [SerializeField]
    private SpriteRenderer spriteRenderer = null;
    private Stats myStats = null;


    private void Awake() {
        // Checking whether to get sprite render from this component or from a child object
        if (spriteIsChild) {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        } else {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        // Getting the stats component if it exists
        myStats = GetComponent<Stats>();
        
        // Setting the name of the file required to mod the image
        string name = modFileName != "" ? modFileName : this.name;
        modFileName = $"{name}.{modFileType}";

        // Checking if the player created a custom asset for this GameObject
        if (ModManager.ModExists(modFileName)) {
            // Debug.Log($"[{this.name}] >>> Player created custom sprite for [{modFileName}]");
            ReplaceSprite();

            // Analyses the pixel colours in the image
            if (analyseImage)   AnalyseImage();
            
        // Also analyse stats for Default image
        } else {
            if (!analyseImage)  return;
            Debug.Log("[ModdableSprite] >>> Analysing Stats for default image...");
            Texture2D defaultTex = spriteRenderer.sprite.texture;
            Colour[] colours = ImageAnalyser.Analyse(defaultTex);

            if (myStats) {
                myStats.GetStatsFromImage(colours);
            }
        }
    }

    /// <summary>
    /// Replaces the current sprite of this GameObject with a new one created by the player.
    /// Optionally analyse the colours in the new texture and re-calculate stats
    /// <param name="newTexture">Texture to update the Sprite with.
    /// If none is given, attempts to load one from mod folder.</param>
    /// </summary>
    public void ReplaceSprite(Texture2D newTexture = null) {
        // getting the size of the sprite within the texture atlas: https://answers.unity.com/questions/1489211/getting-sprite-heightwidth-in-local-space.html
        Vector2Int spriteSize = new Vector2Int(
            (int)spriteRenderer.sprite.rect.width,
            (int)spriteRenderer.sprite.rect.height
        );

        Sprite newSprite; // initialise the new sprite var

        // If a Sprite has been given in the function call, use that one
        if (newTexture)  {
            // check that the texture size matches the desired sprite size
            Vector2Int textureSize = new Vector2Int(newTexture.width, newTexture.height);
            if (textureSize != spriteSize && enforceSize)  return;

            // create the sprite object from the texture
            newSprite = ImageLoader.CreateSprite(newTexture, Pivot.BottomCenter);
            
            // analyse colours and inferr stats from new texture
            Colour[] colours = ImageAnalyser.Analyse(newTexture);
            myStats?.GetStatsFromImage(colours);
        }

        // Otherwise, load texture from disk
        else {
            // loading custom asset into a Sprite
            newSprite = ImageLoader.LoadSprite(modFileName, spriteSize);

            // checking if a sprite was successfully found
            if (newSprite == null)  return;
        }


        // replacing current sprite with custom asset
        spriteRenderer.sprite = newSprite;
    }

    // Swapping the sprite while the game is already running
    private void Update() {
        if (
            Input.GetKeyDown(KeyCode.R)
            && allowHotReload
            && ModManager.FileExists(modFileName)
        ) {
            ReplaceSprite();

            // Analyses the pixel colours in the image
            if (analyseImage) {
                AnalyseImage();
            }
        }
    }

    /// <summary>
    /// Analyses the pixel of each colour in the current sprite. Sends these values to the Stats component.
    /// </summary>
    private void AnalyseImage() {
        print($"[ModdableSprite] >>> Analysing pixels in {modFileName}");

        // Getting colours from texture image
        Texture2D customTex = ImageLoader.LoadTextureFromFile(modFileName);
        Colour[] colours = ImageAnalyser.Analyse(customTex);

        // Sends the colours to the Stats component (if there is one)
        myStats?.GetStatsFromImage(colours);
    }
}

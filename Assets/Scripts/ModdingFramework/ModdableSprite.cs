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

    private string modFileType = "png";

    private SpriteRenderer spriteRenderer = null;


    private void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        // Setting the name of the file required to mod the image
        string name = modFileName != "" ? modFileName : this.name;
        modFileName = $"{name}.{modFileType}";

        // Checking if the player created a custom asset for this GameObject
        if (ModManager.ModExists(modFileName)) {
            Debug.Log($"[{this.name}] >>> Player created custom sprite for [{modFileName}]");
            ReplaceSprite();

            if (analyseImage) {
                Texture2D customTex = ImageLoader.LoadTextureFromFile(modFileName);
                ImageAnalyser.Analyse(customTex);
            }
        }
    }

    /// <summary>
    /// Replaces the current sprite of this GameObject with a new one created by the Player.
    /// </summary>
    private void ReplaceSprite() {
        // Vector2Int spriteSize = new Vector2Int((int)sr.sprite.rect.width, (int)sr.sprite.rect.height);  // getting the size of the sprite within the texture atlas: https://answers.unity.com/questions/1489211/getting-sprite-heightwidth-in-local-space.html
        Sprite modSprite = ImageLoader.LoadSprite(modFileName); // Loading custom asset into a Sprite

        if (modSprite == null)  return;                         // checking if a sprite was successfully found

        spriteRenderer.sprite = modSprite;                      // replacing current sprite with custom asset
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A VFX Object that plays an animation from a spritesheet.
/// </summary>
public class AnimatedFX : MonoBehaviour
{
    [Header("ANIMATED FX")]
    [SerializeField][Tooltip("The texture to use for the spritesheet")]
    private Texture2D texture = null;
    [SerializeField][Tooltip("Name of the sprite sheet file in the Resources/CUSTOM folder")]
    private string textureName = null;
    [SerializeField][Tooltip("Amount of sprites in the spritesheet")][Range(1,20)]
    private int numOfSprites = 0;
    [SerializeField][Tooltip("Speed at which to play the frames. Higher = faster")][Range(0.5f, 2f)]
    private float animationSpeed = 1f;
    [SerializeField][Tooltip("Should the animation be looped forever?")]
    private bool loopAnimation = false;
    [SerializeField][Tooltip("Should the object be destroyed after the animation is completed?")]
    private bool destroyAfterAnimation = false;
    [SerializeField][Tooltip("Applies a random rotation on the Z axis")]
    private bool randomRotation = false;
    [SerializeField][Tooltip("If set, will choose a random texture from this list to create the sprites")]
    private Texture2D[] randomTextures = null;

    // Spritesheet
    private Sprite[] spritesheet = null;

    // Components
    private SpriteRenderer spriteRenderer = null;


    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start() {
        // error handling
        if (numOfSprites == 0) {
            Debug.LogError("[ANIMATED FX] >>> Number of sprites in spritesheet cannot be zero!");
            return;
        }
        // creates sprites
        CreateSprites();

        // random rotation on Z axis
        if (randomRotation)
            transform.Rotate(0f, 0f, Random.Range(0f, 360f));
        
        // Play animation!
        StartCoroutine(PlayAnimation());
    }

    private void CreateSprites() {
        // If multiple random textures have been given
        if (randomTextures.Length > 1) {
            int i = Random.Range(0, randomTextures.Length - 1);
            spritesheet = ImageLoader.CreateAllSprites(textureName, "FX/Animated", numOfSprites, texture:randomTextures[i]);
        }
        // When a single texture has been given
        else {
            spritesheet = ImageLoader.CreateAllSprites(textureName, "FX/Animated", numOfSprites, texture:texture);
        }

        spriteRenderer.sprite =  spritesheet[0];
    }


    private IEnumerator PlayAnimation() {
        foreach (Sprite sprite in spritesheet) {
            spriteRenderer.sprite = sprite;
            yield return new WaitForSecondsRealtime(0.100f / animationSpeed);
        }

        if (loopAnimation)
            StartCoroutine(PlayAnimation());
        
        if (destroyAfterAnimation)
            Destroy(gameObject);
    }
    

}

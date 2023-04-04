using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This object is spawned when an enemy gets killed so that their sprite remains on the battlefield with all their behaviour disabled more easily.
/// </summary>
public class Corpse : MonoBehaviour
{
    [SerializeField][Tooltip("Colour overlay to apply to the corpse sprite.")]
    private Color deathColour;
    [SerializeField][Tooltip("How much to distort the corpse sprite. Positive values increase X and decrease Y, and vice versa")][Range(-0.5f, 0.5f)]
    private float scaleDistortion = 1.2f;

    private SpriteRenderer spriteRenderer = null;


    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// Creates a Corpse sprite on the battlefield.
    /// </summary>
    /// <param name="sprite">Sprite image to give the corpse.</param>
    /// <param name="rotation">Rotation to apply to the corpse.</param>
    public void Init(Sprite sprite) {
        transform.localScale = new Vector3(1 + scaleDistortion, 1 - scaleDistortion, 1);
        spriteRenderer.sprite = sprite;
        spriteRenderer.color = deathColour;
        // transform.Rotate(rotation);
    }
}

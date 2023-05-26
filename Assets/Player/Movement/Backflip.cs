using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Backflip : MonoBehaviour
{
    [Header("BACKFLIP!")]
    [SerializeField][Tooltip("The Physics Layer that this GameObject will be moved to during a Backflip.")]
    private LayerMask physicsLayer;
    [SerializeField][Tooltip("The Sprite Sorting Layer that this GameObject will be moved to during a Backflip.")]
    private string spriteLayer = "Foreground";

    // Constants
    /// <summary>Speed at which the character moves vertically as the backflip is performed.</summary>
    private const float verticalVelocity = 0.5f;
    /// <summary>Downwards force applied to vertical movement.</summary>
    private const float gravity = -(verticalVelocity * 1/16f);
    /// <summary>How far you travel during the backflip.</summary>
    private const float backflipDistance = 9f;

    // Vars
    private float newVerticalVelocity = verticalVelocity;
    private Vector2 originPos;
    private Vector3 originRotation;
    private LayerMask physicsLayerOG;
    private string spriteLayerOG;
    

    // Components
    private Player player = null;
    private Rigidbody2D rb = null;
    private InputManager input = null;
    private Collider2D collider = null;

    private void Awake() {
        // getting components
        player = GetComponent<Player>();
        rb = GetComponent<Rigidbody2D>();
        input = GetComponent<InputManager>();
        collider = GetComponent<Collider2D>();
    }

    private void Start() {
        // setting variables
        physicsLayerOG = player.gameObject.layer;
        spriteLayerOG = player.SpriteRenderer.sortingLayerName;
    }

    private void FixedUpdate() {
        if (player.Status.IsBackflipping) {            
            // Fake vertical movement
            newVerticalVelocity += gravity;
            player.Body.localPosition += new Vector3(0, newVerticalVelocity, 0);

            // Reset when the player touches the ground
            if (player.Body.localPosition.y <= 0) {
                player.Status.IsBackflipping = false;               // setting backflipping status to false to stop the movement

                player.Body.localPosition = Vector3.zero;           // resetting sprite transform to original position
                player.transform.eulerAngles = originRotation;      // ... and rotation
                player.Shadow.localScale = Vector2.one;             // resetting shadow sprite scale

                newVerticalVelocity = verticalVelocity;             // resetting vertical velocity

                UpdateLayers(true);                                 // resets physics and sorting layers
            }

        }
    }

    /// <summary>
    /// Performs a cool blackflip! WOW!!
    /// </summary>
    /// <param name="direction">Vector2 direction to flip in. Only X-Axis actually matters</param>
    public void PerformBackflip(Vector2 direction) {
        // Can only backflip in 4 directions (not diagonaly) - for now anyway
        if (direction.magnitude != 1)   return;

        // getting the correct axis for the current direction - less speed on vertical backflip (X Axis)
        Axis axis = direction.x != 0 ? Axis.Z : Axis.X;
        float speedMod = axis == Axis.Z ? 1f : 0.5f;

        // storing original transform position
        originPos = player.Body.position;
        originRotation = player.transform.eulerAngles;

        // Using Speed stat to increase backflip distance
        float backflipForce = backflipDistance * (player.Stats.MoveSpeed.Value / (player.Stats.MoveSpeed.valueModifier * 3f));
        backflipForce = Mathf.Clamp(backflipForce, backflipDistance, 20f);

        // adding horizontal force and setting flipping status
        rb.AddForce(direction * (backflipForce) * speedMod, ForceMode2D.Impulse);
        player.Status.IsBackflipping = true;

        // Sets new Physics and Sorting layers if requested
        UpdateLayers();
        
    }

    /// <summary>
    /// Sets this GameObject to a new Physics Layer and its SpriteRenderer to a new Sorting Layer (if possible)
    /// </summary>
    /// <param name="reset">Sets the Layers back to their original values.</param>
    private void UpdateLayers(bool reset = false) {
        // reset layers to original values
        if (reset) {
            player.gameObject.layer = physicsLayerOG;
            player.SpriteRenderer.sortingLayerName = spriteLayerOG;

            return;
        }

        // set new physics layer (if a value was given)
        int? newLayer = LayerMaskExtensions.FirstLayer(physicsLayer);
        if (newLayer.HasValue)
            gameObject.layer = newLayer.Value;
        
        // set new sprite sorting layer (if a value was given)
        if (SortingLayer.NameToID(spriteLayer) != 0)
            player.SpriteRenderer.sortingLayerName = spriteLayer;
    }
}

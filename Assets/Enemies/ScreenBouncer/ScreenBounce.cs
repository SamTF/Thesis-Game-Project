using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenBounce : EnemyMovementBase
{
    [Header("Screen Bounce")]
    [SerializeField][Tooltip("Speed of the bouncing object")][Range(1f, 6f)]
    private float screenBounceSpeed = 6f;
    [SerializeField][Tooltip("Modifier to apply to speed value after every bounce")][Range(1f, 1.2f)]
    private float bounceModifier = 1.05f;
    [SerializeField][Tooltip("Maximum speed that this object can move at.")][Range(6f, 15f)]
    private float maxSpeed = 10f;

    // Direction variables
    Vector2 direction = Vector2.one;

    // Constants
    private float spriteSize = 16;
    private Vector2 cameraSize;

    // Components
    private SpriteRenderer spriteRenderer = null;


    private void Start() {
        // Initialising direction
        direction = GenerateInitialDirection(diagonalOnly: true, anyDirection: false);
        // rb.velocity = direction * screenBounceSpeed;

        // Checking the children for sprite renderers if the main object doesn't have one
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null) spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        // Getting constants
        spriteSize = spriteRenderer.sprite.texture.height * transform.localScale.y;
        cameraSize = new Vector2(Camera.main.pixelWidth, Camera.main.pixelHeight);
    }

    private void Update() {
        // ScreenBouncing();
        ScreenSpaceBounce();
    }


    /// <summary>
    /// Generate a Vector2 Direction for the SawDisc's movement.
    /// </summary>
    /// <param name="anyDirection">Any direction is possible or fixed to 8-direction</param>
    /// <returns>A normalised direction vector.</returns>
    private Vector2 GenerateInitialDirection(bool anyDirection = true, bool diagonalOnly = false) {
        if (anyDirection) {
            return new Vector2 (
                Random.Range(-10, 11),
                Random.Range(-10, 11)
            ).normalized;
        } else if (diagonalOnly) {
            Vector2 r =  new Vector2(Random.Range(0f, 1f), Random.Range(0f, 1f));

            return new Vector2 (
                r.x >= 0.5f ? 1 : -1,
                r.y >= 0.5f ? 1 : -1
            );
        } else {
            return new Vector2 (
                Random.Range(-1, 2),
                Random.Range(-1, 2)
            ).normalized;
        }


    }

    /// <summary>
    /// Bounces this object as it hits the screen edges.
    /// </summary>
    private void ScreenBouncing() {
        // Getting the object's position relative to the screen
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(transform.position);

        // Reflect on X axis
        if (screenPosition.x > Screen.width - spriteSize || screenPosition.x < 0f) {
            // Clamp X Position
            screenPosition.x = Mathf.Clamp(screenPosition.x, spriteSize, Screen.width - spriteSize);
            Vector3 newWorldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
            transform.position = new Vector2(newWorldPosition.x, newWorldPosition.y);

            // Flip X Velocity & Clamp to max velocity
            Vector2 newVelocity = rb.velocity;
            newVelocity.x *= -1;
            rb.velocity = Vector2.ClampMagnitude(newVelocity * bounceModifier, maxSpeed);
        }
        // Reflect on Y Axis
        else if (screenPosition.y > Screen.height - spriteSize || screenPosition.y < 0f) {
            // Clamp Y Position
            screenPosition.y = Mathf.Clamp(screenPosition.y, spriteSize, Screen.height -spriteSize);
            Vector3 newWorldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
            transform.position = new Vector2(newWorldPosition.x, newWorldPosition.y);

            // Flip Y Velocity & Clamp to max velocity
            Vector2 newVelocity = rb.velocity;
            newVelocity.y *= -1;
            rb.velocity = Vector2.ClampMagnitude(newVelocity * bounceModifier, maxSpeed);
        }
    }


    private void ScreenSpaceBounce() {
        // Getting the object's position relative to the screen
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(transform.position);


        //// BOUNCING
        // Reflect on X Axis
        if (screenPosition.x > Screen.width - spriteSize || screenPosition.x < spriteSize) {
            // Clamp X Position
            transform.position = ClampPosition(screenPosition, Axis.X);

            // Flip X direction
            direction.x *= -1;
        }
        // Reflect on Y Axis
        else if (screenPosition.y > Screen.height - spriteSize || screenPosition.y < spriteSize) {
            // Clamp Y Position
            transform.position = ClampPosition(screenPosition, Axis.Y);

            // Flip Y Direction
            direction.y *= -1;
        }

        //// MOVING
        // New Screen Position
        screenPosition = Camera.main.WorldToScreenPoint(transform.position);
        Vector2 newPosition = screenPosition + (direction * screenBounceSpeed);
        screenPosition = Camera.main.ScreenToWorldPoint(newPosition);

        // Moving the object
        transform.position = new Vector3( screenPosition.x, screenPosition.y, 0 );
    }


    /// <summary>
    /// Clamps a point so that it remains inside the screen, offset by the sprite size.
    /// </summary>
    /// <param name="screenPosition">The current poisition relative to screen space.</param>
    /// <returns>Clamped position inside the screen boundaries.</returns>
    private Vector2 ClampPosition(Vector2 screenPosition, Axis axis) {
        // Clamp Positions
        if (axis == Axis.X)
            screenPosition.x = Mathf.Clamp(screenPosition.x, spriteSize, Screen.width - spriteSize);
        if (axis == Axis.Y)
            screenPosition.y = Mathf.Clamp(screenPosition.y, spriteSize, Screen.height - spriteSize);

        // Convert screen to world position and return the vector
        Vector3 newWorldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
        return new Vector2(newWorldPosition.x, newWorldPosition.y);
    }
}

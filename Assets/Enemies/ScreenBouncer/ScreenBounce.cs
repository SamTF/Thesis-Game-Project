using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenBounce : EnemyMovementBase
{
    [Header("Screen Bounce")]
    [SerializeField][Tooltip("Speed of the bouncing object")][Range(1f, 10f)]
    private float screenBounceSpeed = 6f;
    [SerializeField][Tooltip("Modifier to apply to speed value after every bounce")][Range(1f, 1.2f)]
    private float bounceModifier = 1.05f;
    [SerializeField][Tooltip("Maximum speed that this object can move at.")][Range(6f, 15f)]
    private float maxSpeed = 10f;

    // Variables
    private Vector2 direction = Vector2.one;
    private int numOfBounces = 0;

    // Constants
    private float spriteSize = 16;
    private Vector2 cameraSize;
    private int bouncesToActivate = 0;

    // Components
    private Bouncer enemy = null;
    private SpriteRenderer spriteRenderer = null;

    // Enums
    private enum DirectionType {
        AnyDirection,
        EightDirections,
        DiagonalOnly
    }


    private void Start() {
        // Initialising direction
        direction = GenerateInitialDirection(DirectionType.DiagonalOnly);
        // rb.velocity = direction * screenBounceSpeed;

        // Getting components
        enemy = GetComponent<Bouncer>();
        spriteRenderer = enemy.SpriteRenderer;

        // Getting constants
        spriteSize = spriteRenderer.sprite.texture.height * transform.localScale.y;
        cameraSize = new Vector2(Camera.main.pixelWidth, Camera.main.pixelHeight);
        bouncesToActivate = enemy.BouncesToActivate;
    }

    private void Update() {
        ScreenSpaceBounce();
    }


    /// <summary>
    /// Generate a Vector2 Direction for the SawDisc's movement.
    /// </summary>
    /// <param name="directionType">Type of directions that this generates.</param>
    /// <returns>A normalised direction vector.</returns>
    private Vector2 GenerateInitialDirection(DirectionType directionType) {
        switch (directionType)
        {
            case DirectionType.AnyDirection:
                return new Vector2 (
                    Random.Range(-10, 11),
                    Random.Range(-10, 11)
                ).normalized;
            
            case DirectionType.DiagonalOnly:
                Vector2 r =  new Vector2(Random.Range(0f, 1f), Random.Range(0f, 1f));

                return new Vector2 (
                    r.x >= 0.5f ? 1 : -1,
                    r.y >= 0.5f ? 1 : -1
                );
            
            case DirectionType.EightDirections:
                return new Vector2 (
                    Random.Range(-1, 2),
                    Random.Range(-1, 2)
                ).normalized;
            
            default:
                return new Vector2 (
                    Random.Range(-1, 2),
                    Random.Range(-1, 2)
                ).normalized;
        }
    }

    /// <summary>
    /// Bounce this object at the camera edges and move it relative to screen space.
    /// </summary>
    private void ScreenSpaceBounce() {
        // Getting the object's position relative to the screen
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(transform.position);

        //// BOUNCING
        // Reflect on X Axis
        if (screenPosition.x > Screen.width - spriteSize || screenPosition.x < spriteSize) {
            transform.position = Bounce(Axis.X, screenPosition);    // Clamp X Position & Flip X direction
        }
        // Reflect on Y Axis
        else if (screenPosition.y > Screen.height - spriteSize || screenPosition.y < spriteSize) {
            transform.position = Bounce(Axis.Y, screenPosition);    // Clamp Y Position & Flip Y Direction
        }

        //// MOVING
        // New Screen Position
        screenPosition = Camera.main.WorldToScreenPoint(transform.position);
        Vector2 newPosition = screenPosition + (direction * (screenBounceSpeed * Time.deltaTime * 100f));
        screenPosition = Camera.main.ScreenToWorldPoint(newPosition);

        // Moving the object
        transform.position = new Vector3( screenPosition.x, screenPosition.y, 0 );
    }


    /// <summary>
    /// Reflects the object's direction around the given axis and clamps its position to screen space.
    /// </summary>
    /// <param name="axis">Which axis to apply the reflection/bounce to.</param>
    /// <param name="screenPosition">The current poisition relative to screen space.</param>
    /// <returns>Clamped position inside the screen boundaries.</returns>
    private Vector2 Bounce(Axis axis, Vector2 screenPosition) {
        ActivateEnemy(bouncesToActivate);

        // Clamp position and reflect direction 
        if (axis == Axis.X) {
            screenPosition.x = Mathf.Clamp(screenPosition.x, spriteSize, Screen.width - spriteSize);
            direction.x *= -1;
        }
        else if (axis == Axis.Y) {
            direction.y *= -1;
            screenPosition.y = Mathf.Clamp(screenPosition.y, spriteSize, Screen.height - spriteSize);
        }

        // Convert screen to world position and return the vector
        Vector3 newWorldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
        return new Vector2(newWorldPosition.x, newWorldPosition.y);
    }


    /// <summary>
    /// Activates the Bouncer Enemy after a certain number of bounces, to give players time to react.
    /// </summary>
    /// <param name="bouncesToActivate">Numbers of bounces required to activate Enemy.</param>
    private void ActivateEnemy(int bouncesToActivate = 3) {
        // Enable the Collider after a certain amount of bounces
        numOfBounces++;
        if (numOfBounces >= bouncesToActivate && !enemy.IsActivated) {
            enemy.IsActivated = true;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Travels across the Screen Space from one random point to another, while always crossing the center.
/// </summary>
public class GhostMovement : EnemyMovementBase
{
    [SerializeField][Tooltip("How long it takes for the Enemy to complete the movement (in seconds)")][Range(1f, 6f)]
    private float duration = 3f;
    [SerializeField][Tooltip("Apply a wavey movement pattern on the Y-axis of the body sprite.")]
    private bool sineWaveMovement = false;
    [SerializeField][Tooltip("Frequency of the wavey up-and-down movement cycles.")][Range(1f, 10f)]
    private float sineWaveFreq = 5f;

    // Constants
    private Vector2 cameraSize;
    private float spriteSize = 16;

    // Components
    private Ghost enemy = null;


    private void Start() {
        // Getting components
        enemy = GetComponent<Ghost>();

        // Getting constants
        cameraSize = new Vector2(Camera.main.pixelWidth, Camera.main.pixelHeight);
        spriteSize = enemy.SpriteRenderer.sprite.texture.height * transform.localScale.y;

        // Begin movement - Tuple!
        (Vector2 start, Vector2 end) = GeneratePoints();
        StartCoroutine( FlyAcrossScreen( start, end ) );
    }


    /// <summary>
    /// Generate random start and target positions between 0 and 1 (with a path that always crosses thru the center of the screen)
    /// </summary>
    private (Vector2, Vector2) GeneratePoints() {
        Vector2 start = new Vector2(Random.Range(0f, 1f), Random.Range(0f, 1f));
        Vector2 target = new Vector2(1 - start.x, 1 - start.y);

        // Setting a random axis to go from 0 to 1
        // (do that it always starts from a screen edge)
        int axis = Random.Range(0, 2);
        // Start at X Axis
        if (axis == 0) {
            start.x = 0f;
            target.x = 1.1f;
        } 
        // Start at Y Axis
        else {
            start.y = 0;
            target.y = 1.1f;
        }

        // Return both position vector as a tuple. Cool!
        return (start, target);
    }


    /// <summary>
    /// Moves across screen space smoothly over time.
    /// </summary>
    /// <param name="start">Start position to begin movement.</param>
    /// <param name="target">Target position to end movement.</param>
    private IEnumerator FlyAcrossScreen(Vector2 start, Vector2 target) {
        float time = 0;

        // Move the enemy for the duration given
        while (time < duration)
        {
            // Set moving status on Enemy
            enemy.IsMoving = true;

            // Getting the appropriate start and end positions relative to the camera
            Vector2 startPosition = Camera.main.ScreenToWorldPoint(Vector2.Scale( cameraSize, start ));
            Vector2 targetPosition = Camera.main.ScreenToWorldPoint(Vector2.Scale( cameraSize, target ));

            // Smoothly lerping from start to end
            transform.position = Vector2.Lerp(startPosition, targetPosition, time / duration);

            // Sine wave offset
            if (sineWaveMovement)
                body.position = new Vector2(transform.position.x, transform.position.y + ( Mathf.Sin (Time.time * sineWaveFreq) * 0.5f ) );

            time += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }

        // Set moving status on Enemy
        enemy.IsMoving = false;

        // Wait X seconds then repeat the movement
        yield return new WaitForSeconds(enemy.RespawnTime);

        // Begin movement anew - Tuple!
        (start, target ) = GeneratePoints();
        StartCoroutine( FlyAcrossScreen( start, target ) );
    }
}

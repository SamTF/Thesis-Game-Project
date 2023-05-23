using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterMovement : EnemyMovementBase
{
    [Header("Shooter Movement")]
    [SerializeField][Range(1f, 6f)][Tooltip("Distance from its target at which this Enemy will start to run away.")]
    private float safeDistance = 4f;
    [SerializeField][Range(0f, 5f)][Tooltip("How quickly the Enemy will react to a change in the target's position. Also works as a speed value, kind of.")]
    private float followDelay = 2.00f;
    [SerializeField][Tooltip("How long until this Enemy rotates to a new position (in seconds)")][Range(0f, 4f)]
    private float rotationCooldown = 2f;
    [SerializeField][Tooltip("Whether this enemy repositions itself every few seconds or not.")]
    private bool rotatesPosition = true;

    private Vector2 velocity = Vector3.one;
    private bool isRunningAway = false;
    private bool isMoving = false;


    private void Start() {
        Vector2 target = GenerateNewTargetPosition();
        StartCoroutine(RunAndGun(target));
    }


    private void Update() {
        // get target position and distance
        Vector2 target = GameManager.instance.PlayerPosition;
        float distance2target = (target - (Vector2)transform.position).magnitude;

        // reset rigidbody velocity
        rb.velocity = Vector2.zero;

        // Run away if the Player gets too close (and the enemy is not already running away)
        if (distance2target < (safeDistance + Mathf.Sin(Time.time * 0.5f))
            && !isRunningAway)
        {
            StopAllCoroutines();
            StartCoroutine(RunAway(target));
        }
    }


    /// <summary>
    /// Continously runs away from a target.
    /// </summary>
    /// <param name="target">The position to run away from.</param>
    private void RunAwayContinuous(Vector2 target) {
        Vector3 runAwayDirection = Vector3.ClampMagnitude( ( (transform.position - (Vector3)target) * 2f ), 5f );
        Vector3 runAwayTarget = transform.position + runAwayDirection;

        Vector2 movement = Vector2.SmoothDamp(
            transform.position,
            runAwayTarget,
            ref velocity,
            followDelay/3f
        );

        transform.position = movement;
    }

    /// <summary>
    /// Runs away in a start & stop method.
    /// </summary>
    /// <param name="target">The position to run away from.</param>
    private IEnumerator RunAway(Vector2 target) {
        isRunningAway = true;

        // Getting the direction/position to run away too
        Vector3 runAwayDirection = Vector3.ClampMagnitude( ( (transform.position - (Vector3)target) * 2f ), 5f );
        Vector3 runAwayTarget = transform.position + runAwayDirection;
        runAwayTarget = Vector3Int.RoundToInt(runAwayTarget);

        // Move until it (roughly reaches its destination)
        while (Vector3Int.RoundToInt(transform.position) != runAwayTarget) {
            Vector2 movement = Vector2.SmoothDamp(
                transform.position,
                runAwayTarget,
                ref velocity,
                followDelay/3f
            );

            transform.position = movement;

            yield return new WaitForFixedUpdate();
        }

        // No longer running away - resume normal movement behaviour
        isRunningAway = false;

        yield return new WaitForSeconds(rotationCooldown);
        StartCoroutine(RunAndGun(GenerateNewTargetPosition()));
    }


    /// <summary>
    /// Changes position every few seconds.
    /// </summary>
    /// <param name="target">The new position to move to.</param>
    private IEnumerator RunAndGun(Vector2 target) {
        // Breaks the coroutine if repositioning is disabled
        if (!rotatesPosition) {
            yield break;
        }

        // Round the target positions to nearest integers
        target = Vector2Int.RoundToInt(target);

        isMoving = true;

        // Move this object until it reaches its destination
        while (Vector2Int.RoundToInt(transform.position) != target)
        // while ((target - (Vector2)transform.position).magnitude >= 0.7f)
        {
            transform.position = Vector2.SmoothDamp(transform.position, target, ref velocity, followDelay);
            rb.velocity = Vector2.zero;

            yield return new WaitForFixedUpdate();
        }

        isMoving = false;

        // Wait X seconds before rotating to a new position
        yield return new WaitForSeconds(rotationCooldown);

        Vector2 newTarget = GenerateNewTargetPosition();
        StartCoroutine( RunAndGun ( newTarget ));
    }

    /// <summary>
    /// Generate a new point for the Shooter to move to based on the distance to the player position.
    /// </summary>
    /// <returns>A Vector2 position.</returns>
    private Vector2 GenerateNewTargetPosition() {
        float distance2player = ((Vector2)transform.position - GameManager.instance.PlayerPosition).magnitude;
        Vector2 newTarget;

        // If too far away from the Player, gets closer
        if (distance2player > 8f) {
            Debug.Log("Closing the gap with the Player");
            newTarget = GameManager.instance.PlayerPosition + (Random.insideUnitCircle.normalized * (safeDistance + 1f) );

        // Otherwise just rotates a lil bit
        } else {
            Vector2 moveDirection = Random.insideUnitCircle.normalized * 5f;
            Debug.Log($"Rotating {moveDirection}");
            newTarget = (Vector2)transform.position + moveDirection;
        }

        return newTarget;
    }


    // Public Getters
    public bool IsMoving => isMoving;
    public bool IsRunningAway => isRunningAway;
}

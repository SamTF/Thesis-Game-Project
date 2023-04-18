using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothFollow : EnemyMovementBase
{
    [Header("Smooth Follow")]
    [SerializeField][Range(0f, 5f)][Tooltip("How quickly the Enemy will react to a change in the Player's position. Also works as a speed value, kind of.")]
    private float followDelay = 2.00f;

    private Vector2 velocity = Vector3.one;

    // Natural randomisation
    private float delayOffset = 0f;
    private Vector2 targetOffset = Vector2.zero;


    private void Start() {
        // Lil bit of delay randomisation so enemies don't all become one hivemind
        float r = followDelay * 0.25f;
        delayOffset = Random.Range(-r, r);

        // Slight target randomisation
        float o = 2f;
        targetOffset = new Vector2(
            Random.Range(-o, o),
            Random.Range(-o, o)
        );
    }

    private void FixedUpdate() {
        Vector2 target = GameManager.instance.PlayerPosition;
        float distance2target = (target - (Vector2)transform.position).magnitude;

        Move(target, distance2target);
    }


    /// <summary>
    /// Moving smoothly towards the Player with a natural delay using SmoothDamp and Transform.position.
    /// Speeds up as the enemy gets closer to its target and vice versa.
    /// Also added increasing randomisation when it's far from its target, and no randomisation when it's close.
    /// </summary>
    /// <param name="target">Vector2 position of the Object to follow.</param>
    /// <param name="distance2target">How far away that object is from our current position.</param>
    private void Move(Vector2 target, float distance2target) {
        float delay = followDelay;

        // Far
        // Slower speed (aka more Delay) + Delay randomisation + extra target randomisation offset
        if (distance2target >= 8f) {
            delay += (delayOffset * 2f);
            delay *= 1.5f;
            target += targetOffset * 2f;
        }
        // Medium-far
        // Delay randomisation + target offset
        else if (distance2target > 5f) {
            delay += delayOffset;
            target += targetOffset;
        }
        //  Medium-close
        // Delay randomisation + 50% delay (faster) + smaller target offset
        else if (distance2target > 3f) {
            delay += delayOffset * 0.5f;
            delay = delay / 2f;
            target += targetOffset * 0.5f;
        }
        // Close range
        // 25 delay (very very fast) and NO randomisation
        else {
            delay = followDelay / 4f;
        }

        // Smooth movement
        transform.position = Vector2.SmoothDamp(transform.position, target, ref velocity, delay);
        rb.velocity = Vector2.zero;
    }
}

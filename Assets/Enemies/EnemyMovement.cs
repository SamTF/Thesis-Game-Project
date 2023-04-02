using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// This script contains all the movement methods that enemies can use for now.
/// </summary>
public class EnemyMovement : MonoBehaviour
{
    [Header("Basic Enemy Movement Scripts")]
    [SerializeField]
    private MovementType movementType = MovementType.Follow;

    [Header("Smooth Follow")]
    [SerializeField][Range(0f, 5f)]
    private float followDelay = 0.75f;
    private Vector3 velocity = Vector3.one;
    private float delayOffset = 0f;
    private Vector2 targetOffset = Vector2.zero;

    [Header("Classic Follow & Follow Delayed")]
    [SerializeField][Tooltip("How fast the enemy follows its target")]
    [Range(50f, 200f)]
    private float followSpeed = 50f;

    [Header("Orbit")]
    [SerializeField][Tooltip("Degrees to rotate around target per second")]
    [Range(0.1f, 1f)]
    private float rotationSpeed = 0.5f;
    [SerializeField]
    private float radius = 3f;
    private float angle;
    

    // Components
    [Header("Components")][Tooltip("Not mandatory to add these manually")]
    [SerializeField]
    private Transform body = null;
    [SerializeField]
    private Transform shadow = null;
    private Rigidbody2D rb = null;
    

    private enum MovementType
    {
        SmoothDamp,
        Follow,
        FollowDelayed,
        Orbit,
        ScreenRelative
    }


    void Awake() {
        rb = GetComponent<Rigidbody2D>();
        body = body ? body : transform.Find("Body");
        shadow = shadow ? shadow : transform.Find("Shadow");

        if (movementType == MovementType.Orbit && shadow != null) {
            shadow.localPosition = shadow.localPosition + new Vector3(0, -0.5f, 0);
        }
    }

    private void Start() {
        if (movementType == MovementType.FollowDelayed) {
            StartCoroutine(FollowDelayed());
        } else if (movementType == MovementType.SmoothDamp) {
            // Lil bit of delay randomisation so enemies don't all become one hivemind
            float r = followDelay * 0.25f;
            delayOffset = Random.Range(-r, r);

            // Slight target randomisation
            float o = 2f;
            targetOffset = new Vector2(Random.Range(-o, o), (Random.Range(-o, o)));
        }
        else if (movementType == MovementType.ScreenRelative) {
            StartCoroutine(ScreenRelative(new Vector2(0, 0.5f), new Vector2(1, 0.5f), 10f));
        }
    }

    private void FixedUpdate() {
        Vector2 target = GameManager.instance.PlayerPosition;
        float distance2target = (target - (Vector2)transform.position).magnitude;
        // Debug.Log(distance2target);

        switch (movementType)
        {
            case MovementType.SmoothDamp:
                SmoothFollow(target);
                break;

            case MovementType.Follow:
                StartCoroutine(Follow());
                break;

            case MovementType.Orbit:
                OrbitAround(target);
                break;
            
            case MovementType.FollowDelayed:
                break;

            case MovementType.ScreenRelative:
                break;

            default:
                SmoothFollow(target);
                break;
        }
    }

    /// <summary>
    /// Moving directly towards the player using the Transform
    /// </summary>
    /// <param name="target"></param>
    private void SmoothFollow(Vector2 target) {
        // Speed up as the enemy gets closer to the player and vice versa
        float distance2target = (target - (Vector2)transform.position).magnitude;
        float delay = followDelay;

        // Far
        if (distance2target >= 8f) {
            delay += (delayOffset * 2f);
            delay *= 1.5f;
            target += targetOffset * 2f;
        }
        // Medium far
        else if (distance2target > 4f ) {
            delay = followDelay + delayOffset;
        }
        //  Medium-close
        else if (distance2target > 3f) {
            delay += delayOffset;
            delay = delay / 2f;
            target += targetOffset;
        }
        // Close range
        else {
            delay = followDelay / 4f;
        }

        // Smooth movement
        transform.position = Vector3.SmoothDamp(transform.position, target, ref velocity, delay);
        rb.velocity = Vector2.zero;
    }

    /// <summary>
    /// Classic follow script using RigidBody to add forces in the direction of the player
    /// </summary>
    private IEnumerator Follow() {
        Vector3 target = GameManager.instance.PlayerPosition;
        Vector2 direction = target - transform.position;
        direction = direction.normalized * followSpeed;

        rb.AddForce(direction);

        yield return new WaitForSeconds(0.2f);

        rb.velocity = rb.velocity/2;
    }

    /// <summary>
    /// Follows the target fast but with delay and momentum
    /// </summary>
    private IEnumerator FollowDelayed() {
        while (true)
        {
            Vector3 target = GameManager.instance.PlayerPosition;
            Vector2 direction = target - transform.position;
            direction = direction.normalized * followSpeed;

            rb.AddForce(direction);

            yield return new WaitForSeconds(0.5f);

            rb.velocity = rb.velocity * 0.8f;
        }
    }

    /// <summary>
    /// Orbits clockwise around a target while slowly moving towards it.
    /// </summary>
    /// <param name="target">Target to robit around and follow.</param>
    private void OrbitAround(Vector2 target) {
        angle += rotationSpeed * 1/16f;

        var offset = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle)) * radius;
        transform.position = target + offset;

        radius -= 0.02f * 1/16f;
    }

    private void OrbitAround2(Vector2 target) {
        transform.position = Vector2.MoveTowards(transform.position, target, 0.05f/16f);    // Move very slowly towards the player
        transform.RotateAround(target, Vector3.forward, rotationSpeed * 1/16);              // Rotate around the player

        // Offset Body and Shadow sprites so they remain in the same rotation
        // body.localPosition = transform.position.normalized;
        // shadow.localPosition = transform.position.normalized;
        body.localEulerAngles = new Vector3(0, 0, -transform.eulerAngles.z);
        shadow.localEulerAngles = new Vector3(0, 0, -transform.eulerAngles.z);
    }



    /// <summary>
    /// Move the enemy from Start to Target relative to the Camera/Screen Space (not to World Space)
    /// </summary>
    /// <param name="start">Start position offset relative to screen size (between 0-1).</param>
    /// <param name="target">Target position offset relative to screen size (between 0-1).</param>
    /// <param name="duration">Length in seconds that the movement will take.</param>
    /// <returns></returns>
    private IEnumerator ScreenRelative(Vector2 start, Vector2 target, float duration) {
        float time = 0;
        Vector2 cameraSize = new Vector2(Camera.main.pixelWidth, Camera.main.pixelHeight);

        while (time < duration)
        {
            // Getting the appropriate start and end positions relative to the camera
            Vector2 startPosition = Camera.main.ScreenToWorldPoint(Vector2.Scale( cameraSize, start ));
            Vector2 targetPosition = Camera.main.ScreenToWorldPoint(Vector2.Scale( cameraSize, target ));

            // Smoothly lerping from start to end
            transform.position = Vector2.Lerp(startPosition, targetPosition, time / duration);

            // Sine wave offset
            transform.position = new Vector2(transform.position.x, transform.position.y + (Mathf.Sin(Time.time * 5f)));

            time += Time.deltaTime;
            yield return null;
        }

        this.enabled = false;
    }


    ///// Stop following if the Player is dead
    private void OnEnable() {
        Health.onPlayerDeath += OnPlayerDeath;
    }
    private void OnDisable() {
        Health.onPlayerDeath -= OnPlayerDeath;
        StopAllCoroutines();
    }
    private void OnPlayerDeath() {
        rb.bodyType = RigidbodyType2D.Kinematic;
        this.enabled = false;
    }

}
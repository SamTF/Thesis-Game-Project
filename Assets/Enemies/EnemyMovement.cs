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
    private Vector2 velocity = Vector3.one;
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

    [Header("Screen Relative")]
    [SerializeField][Tooltip("How many seconds it takes to complete the movement")][Range(1f, 10f)]
    private float screenRelativeDuration = 10f;
    [SerializeField][Tooltip("How long until this Enemy will reappear after leaving the screen")]
    private float reappearanceCooldown = 2f;
    [SerializeField][Tooltip("Whether to also move the object in a Sine wave pattern on the Y axis")]
    private bool sineWaveMovement = true;
    [SerializeField][Tooltip("Frequency of the Sine wave movement")][Range(1f, 10f)]
    private float sineWaveFreq = 5f;

    [Header("Screen Bounce")]
    [SerializeField][Tooltip("Speed of the bouncing object")]
    private float screenBounceSpeed = 6f;
    [SerializeField][Tooltip("Modifier to apply to speed value after every bounce")][Range(1f, 1.2f)]
    private float bounceModifier = 1.05f;
    

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
        ScreenRelative,
        ScreenBounce
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
        // Follow Delayed
        if (movementType == MovementType.FollowDelayed) {
            StartCoroutine(FollowDelayed());
        
        // Smooth Follow
        } else if (movementType == MovementType.SmoothDamp) {
            // Lil bit of delay randomisation so enemies don't all become one hivemind
            float r = followDelay * 0.25f;
            delayOffset = Random.Range(-r, r);

            // Slight target randomisation
            float o = 2f;
            targetOffset = new Vector2(Random.Range(-o, o), (Random.Range(-o, o)));
        }

        // Screen Relative
        else if (movementType == MovementType.ScreenRelative) {
            StartCoroutine(ScreenRelative(new Vector2(0, 0.5f), new Vector2(1, 0.5f), screenRelativeDuration));

        // Screen Bounce
        } else if (movementType == MovementType.ScreenBounce) {
            Vector2 initialDirection = new Vector2(
                Random.Range(-1f, 1f),
                Random.Range(-1f, 1f)
            );
            rb.velocity = initialDirection.normalized * screenBounceSpeed;
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

            case MovementType.ScreenBounce:
                ScreenBounce();
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
        transform.position = Vector2.SmoothDamp(transform.position, target, ref velocity, delay);
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
        // Resetting time and Camera Screen size
        float time = 0;
        Vector2 cameraSize = new Vector2(Camera.main.pixelWidth, Camera.main.pixelHeight);

        // Generating random start and target positions (with a path that crosses thru the center of the screen)
        start = new Vector2(Random.Range(0f, 1f), Random.Range(0f, 1f));
        target = new Vector2(1 - start.x, 1 - start.y);

        // Setting a random axis to go from 0 to 1
        int axis = Random.Range(0, 2);
        if (axis == 0) {
            start.x = 0f;
            target.x = 1.1f;
        } else {
            start.y = 0;
            target.y = 1.1f;
        }

        // Move the enemy for the duration given
        while (time < duration)
        {
            // Getting the appropriate start and end positions relative to the camera
            Vector2 startPosition = Camera.main.ScreenToWorldPoint(Vector2.Scale( cameraSize, start ));
            Vector2 targetPosition = Camera.main.ScreenToWorldPoint(Vector2.Scale( cameraSize, target ));

            // Smoothly lerping from start to end
            transform.position = Vector2.Lerp(startPosition, targetPosition, time / duration);

            // Sine wave offset
            if (sineWaveMovement)
                transform.position = new Vector2(transform.position.x, transform.position.y + (Mathf.Sin(Time.time * sineWaveFreq)));

            time += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }

        // Hide the enemy & disable all colliders after it completes the movement
        GetComponentInChildren<SpriteRenderer>().enabled = false;
        Collider2D[] colliders = GetComponentsInChildren<Collider2D>();
        foreach (Collider2D c in colliders) {
            c.enabled = false;
        }

        // Wait x seconds before doing new movement
        yield return new WaitForSeconds(reappearanceCooldown);

        // Show enemy and enable all colliders
        GetComponentInChildren<SpriteRenderer>().enabled = true;
        foreach (Collider2D c in colliders) {
            c.enabled = true;
        }

        // Repeat the coroutine
        StartCoroutine(ScreenRelative(Vector2.zero, Vector2.one, duration));
    }

    /// <summary>
    /// Bounces this object as it hits the screen edges.
    /// </summary>
    private void ScreenBounce() {
        // Getting the object's position relative to the screen
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(transform.position);

        // Reflect on X axis
        if (screenPosition.x > Screen.width || screenPosition.x < 0f) {
            // Clamp X Position
            screenPosition.x = Mathf.Clamp(screenPosition.x, 16f, Screen.width - 16f);
            Vector3 newWorldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
            transform.position = new Vector2(newWorldPosition.x, newWorldPosition.y);

            // Flip X Velocity & Clamp to max velocity
            Vector2 newVelocity = rb.velocity;
            newVelocity.x *= -1;
            rb.velocity = Vector2.ClampMagnitude(newVelocity * bounceModifier, 10f);
        }
        // Reflect on Y Axis
        else if (screenPosition.y > Screen.height || screenPosition.y < 0f) {
            // Clamp Y Position
            screenPosition.y = Mathf.Clamp(screenPosition.y, 16f, Screen.height -16f);
            Vector3 newWorldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
            transform.position = new Vector2(newWorldPosition.x, newWorldPosition.y);

            // Flip Y Velocity & Clamp to max velocity
            Vector2 newVelocity = rb.velocity;
            newVelocity.y *= -1;
            rb.velocity = Vector2.ClampMagnitude(newVelocity * bounceModifier, 10f);
        }
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
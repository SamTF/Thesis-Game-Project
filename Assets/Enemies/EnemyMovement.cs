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

    [Header("Classic Follow & Follow Delayed")]
    [SerializeField][Tooltip("How fast the enemy follows its target")]
    [Range(30f, 75f)]
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
        Orbit
    }


    void Awake() {
        rb = GetComponent<Rigidbody2D>();
        body = body ? body : transform.Find("Body");
        shadow = shadow ? shadow : transform.Find("Shadow");

        if (movementType == MovementType.Orbit && shadow != null) {
            shadow.localPosition = shadow.localPosition + new Vector3(0, -0.5f, 0);
        } else if (movementType == MovementType.SmoothDamp) {
            rb.bodyType = RigidbodyType2D.Kinematic;
        }
    }

    private void Start() {
        if (movementType == MovementType.FollowDelayed) {
            StartCoroutine(FollowDelayed());
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
        float distance2target = (target - (Vector2)transform.position).magnitude;
        // float delay = distance2target > 2.5f ? followDelay : followDelay / 4f;
        float delay = followDelay;
        if (distance2target <= 2f)   delay = followDelay / 4f;
        else if (distance2target <= 3f)   delay = followDelay / 2f;
        
        // Debug.Log(delay);

        transform.position = Vector3.SmoothDamp(transform.position, target, ref velocity, delay);
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


    ///// Stop following if the Player is dead
    private void OnEnable() {
        Health.onPlayerDeath += OnPlayerDeath;
    }
    private void OnDisable() {
        Health.onPlayerDeath -= OnPlayerDeath;
    }
    private void OnPlayerDeath() {
        rb.bodyType = RigidbodyType2D.Kinematic;
        this.enabled = false;
    }

}
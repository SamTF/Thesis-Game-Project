using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Item that is dropped when Enemies are killed. The Player can collect these to gain XP and level up.
/// </summary>
public class XP : MonoBehaviour
{
    [Header("ITEM")]
    [SerializeField][Tooltip("How quickly the Item will react to a change in the Player's position. Also works as a speed value, kind of.")][Range(0f, 1f)]
    private float followDelay = 0.25f;

    [SerializeField][Tooltip("Auto-pick up range for this item")][Range(0f, 2f)]
    private float detectionRadius = 1f;
    [SerializeField][Tooltip("Transform of object with the detection collider")]
    private Transform detectionTransform = null;

    [SerializeField][Tooltip("Physics layer of the Player Gameobject")]
    private LayerMask playerLayer;

    [SerializeField]
    private Transform body = null;

    private Rigidbody2D rigidBody = null;


    private void Awake() {
        // Getting Components
        rigidBody = GetComponent<Rigidbody2D>();

        if (detectionTransform == null)     detectionTransform = transform.Find("Detection");
        if (body == null)                   body = transform.Find("Body");

        // Setting detection collider radius
        detectionTransform.GetComponent<CircleCollider2D>().radius = detectionRadius;
    }


    public void Spawn(Vector2 bounceDirection) {
        Debug.Log("Spawning!!");
        StartCoroutine(BounceAway(bounceDirection));
    }

    /// <summary>
    /// Make this Item bounce away in a given direction and jump in an arc.
    /// </summary>
    /// <param name="direction">Direction to move away in.</param>
    private IEnumerator BounceAway(Vector2 direction) {
        // Using this instead of Time.time so that it ALWAYS starts at ZERO
        float time = 0f;

        rigidBody.AddForce(direction.normalized * 150f);    // Add force
        body.GetComponent<Collider2D>().enabled = false;    // Disable body collider

        // Perform the arc jump movement
        while (body.localPosition.y >= 0f)
        {
            float verticalVelocity = Mathf.Clamp(Mathf.Sin(time * 4f) * 0.75f, -0.1f, 1f);
            Debug.Log(verticalVelocity);
            body.localPosition = new Vector3(0, verticalVelocity, 0);
            // body.localPosition += new Vector3(0, Mathf.Sin(Time.time * 5f) * 0.05f, 0);

            time += Time.deltaTime;

            yield return null;
        }

        // Reset everything
        body.localPosition = Vector3.zero;
        rigidBody.velocity = Vector2.zero;
        body.GetComponent<Collider2D>().enabled = true;

        // test
        // StartCoroutine(BounceAway( ((Vector2)transform.position - GameManager.instance.PlayerPosition).normalized ));
    }


    // Check for trigger collisions with the Player
    private void OnTriggerEnter2D(Collider2D other) {
        if ( ((1<<other.gameObject.layer) & playerLayer) != 0 ) 
        {
            StartCoroutine(GoToPlayer());
        }
    }


    /// <summary>
    /// Quickly move towards the Player in a smooth fashion until it is collected.
    /// </summary>
    private IEnumerator GoToPlayer() {
        Vector2 velocity = Vector2.one;

        // Follow the player's position
        while ( (GameManager.instance.PlayerPosition - (Vector2)transform.position).magnitude > 0.05f ) {
            Vector2 target = GameManager.instance.PlayerPosition;
            float delayModifier = 1f;

            // No delay at all when very close to the Player
            if ((GameManager.instance.PlayerPosition - (Vector2)transform.position).magnitude < 0.5f)
                delayModifier = 0f;

            transform.position = Vector2.SmoothDamp(transform.position, target, ref velocity, followDelay * delayModifier);

            yield return new WaitForFixedUpdate();
        }

        // Despawn after being collected by the Player
        Despawn();
    }


    /// <summary>
    /// Triggered when the Item is collected by the Player. Does some cool effects before destroying the object.
    /// </summary>
    private void Despawn() {
        Destroy(gameObject);
    }
}

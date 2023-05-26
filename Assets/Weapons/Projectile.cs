using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Generic projectile with constant movement direction and speed
/// </summary>
public class Projectile : MonoBehaviour, IDamage
{
    // Stats
    private float mySpeed;
    private float myRange;
    private Vector2 myDirection;
    private LayerMask targetLayer;
    private int myDamage = 1;

    // Constants
    private Vector3 originPos;
    private float gravity = 0.25f;
    
    // Components
    [SerializeField]
    private Transform body = null;
    private Rigidbody2D rb = null;
    private Animator animator = null;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Start() {
        rb.velocity = (myDirection * mySpeed);
        originPos = transform.position;
    }

    private void FixedUpdate() {
        // Range Drop off
        float distanceTravelled = Mathf.Abs((transform.position - originPos).magnitude);

        if (distanceTravelled >= myRange) {
            Vector2 dropoff = body.localPosition;
            dropoff.y -= gravity * 1/16;
            body.localPosition = dropoff;

            // Checking if it hit the ground
            if (body.localPosition.y <= 0f) Despawn();
        }
    }

    /// <summary>
    /// Basic setup function for the projectile to determine it's direction and speed.
    /// </summary>
    /// <param name="direction">Vector2 direction the projectile will travel in.</param>
    /// <param name="speed">Float speed at which it will travel.</param>
    /// <param name="range">How far the projectile will travel before drop-off is applied.</param>
    /// <param name="layer">Which physics layer this projectile affects.</param>
    /// <param name="damage">How much damage it applies to its target.</param>
    public void Shoot(Vector2 direction, float speed, float range, LayerMask layer, int damage = 1) {
        myDirection = direction;
        mySpeed = speed;
        myRange = range;
        targetLayer = layer;
        myDamage = damage;

        rb.velocity = (myDirection * mySpeed);
        DamageScaleUp();
    }

    /// <summary>
    /// Scales up the Projectile according to its damage
    /// </summary>
    private void DamageScaleUp() {
        if (myDamage <= 1)
            return;
            
        Vector3 scaleUp = Vector3.one * (myDamage * 0.25f);

        body.localScale += scaleUp;
    }

    // Checking for collisions
    private void OnTriggerEnter2D(Collider2D other) {
        if(((1<<other.gameObject.layer) & targetLayer) != 0) {
            Debug.Log($"[PROJECTILE] >>> Gotcha! Projectile hit its target => {other.name}");
            OnHit(other.transform);
        }
    }

    /// <summary>
    /// Call this function when the projectile hit its target
    /// </summary>
    /// <param name="target"></param>
    private void OnHit(Transform target) {
        Despawn();
    }

    /// <summary>
    /// Function wrapper for destroying the projectile with extra effects.
    /// </summary>
    private void Despawn() {
        GameObject poofCloud = Resources.Load<GameObject>("FX/Poof");
        Vector3 randomOffset = new Vector3 (Random.Range(-0.25f, 0.25f), Random.Range(-0.25f, 0.25f), 0);
        Instantiate(poofCloud, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }

    /// Destroy it when it leaves the screen
    private void OnBecameInvisible() {
        Despawn();
    }

    // Getters
    /// <summary>How much Damage this projectile inflicts on its target</summary>
    public int Damage => myDamage;

}

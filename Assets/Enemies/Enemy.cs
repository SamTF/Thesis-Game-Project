using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contains all the functionality and methods that ALL enemies must have. All enemies inherit from this.
/// </summary>

[RequireComponent(typeof(HitStop))]

public abstract class Enemy : MonoBehaviour
{
    [Header("ENEMY")]
    /// <summary>Objects in these Physics Layers will cause this Enemy to take damage on hit.</summary>
    [SerializeField][Tooltip("Objects in these Physics Layers will cause this object to take damage")]
    private LayerMask damageLayer;

    /// <summary>How many hits this enemy can take before it dies.</summary>
    [SerializeField][Tooltip("How many hits this enemy can take before it dies")]
    protected int health = 6;

    // Status
    private bool isKnockedBack = false;
    private bool canTakeDamage = true;
    private float damageCooldown = 0.05f;

    // Children
    protected Transform body = null;
    protected Transform shadow = null;

    // Components
    protected SpriteRenderer spriteRenderer = null;
    protected Animator animator = null;
    protected Rigidbody2D rigidBody = null;
    protected EnemyMovement movement = null;
    protected HitStop hitStop = null;


    protected virtual void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody2D>();
        movement = GetComponent<EnemyMovement>();
        hitStop = GetComponent<HitStop>();

        // Checking the children for sprite renderers if the main object doesn't have one
        if (spriteRenderer == null) spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        body = body ? body : transform.Find("Body");
        shadow = shadow ? shadow : transform.Find("Shadow");
    }


    // Checking for collisions
    private void OnTriggerEnter2D(Collider2D other) {
        if (
            ((1<<other.gameObject.layer) & damageLayer) != 0
                && canTakeDamage
            ) {
            Debug.Log($"[ENEMY] >>> Gotcha! {this.name} was damaged by {other.transform.parent.name}");
            OnHit(other.transform);
        }
    }

    /// <summary>
    /// Called when this Enemy takes damage.
    /// </summary>
    /// <param name="hitter">Transform of the object that hit the enemy.</param>
    protected virtual void OnHit(Transform hitter) {
        health -= 1;

        if (health <= 0) {
            OnDeath();
            return;
        }

        hitStop.Hit(100);

        StartCoroutine(DamageCooldown());
        StartCoroutine(BlinkAnimation());

        // Apply knockback rounded to 8-direction
        // Currently commented out bevause it varies a lot depending on the enemy's movement type
        // will need a specific knockback function for each type maybe
        // Vector2 knockbackVector = Vector2Int.RoundToInt((transform.position - hitter.position).normalized);
        // rigidBody.AddForce(knockbackVector * 800f);
    }

    private IEnumerator BlinkAnimation() {
        animator.SetBool("Blink", true);
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("Blink", false);
    }


    /// <summary>
    /// Called when this Enemy runs out of health and is killed.
    /// </summary>
    protected virtual void OnDeath() {
        // Longer hit stop
        hitStop.Hit(200);

        // Play death animation
        StartCoroutine(DeathAnimation());
    }

    /// <summary>
    /// Animations to play on the Enemy's corpse once it dies.
    /// </summary>
    private IEnumerator DeathAnimation() {
        yield return new WaitForSeconds(0.01f);

        // Rotate sprite away from Player
        float hitAngle = Mathf.Atan2(
            GameManager.instance.PlayerPosition.y - transform.position.y,
            GameManager.instance.PlayerPosition.x - transform.position.x
        ) * Mathf.Rad2Deg;

        Vector3 rotation = new Vector3(0, 0, hitAngle + 90f);

        // Create a Corpse Object
        CorpseFactory.Instantiate(transform.position, rotation, spriteRenderer.sprite, transform.localScale);

        // Spawn death VFX
        GameObject deathFX = Resources.Load<GameObject>("FX/Fart");
        Instantiate(deathFX, transform.position, Quaternion.identity);

        // Destroy this object
        Destroy(gameObject);
    }

    // Fail-safe to prevent enemies from taking damage more than once from the same projectile
    // (still no idea why that was even happening)
    private IEnumerator DamageCooldown() {
        canTakeDamage = false;
        yield return new WaitForSeconds(damageCooldown);
        canTakeDamage = true;
    }

}

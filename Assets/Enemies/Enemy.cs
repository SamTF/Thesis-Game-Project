using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("ENEMY")]
    [SerializeField][Tooltip("Objects in these Physics Layers will cause this object to take damage")]
    private LayerMask damageLayer;
    [SerializeField]
    private int health = 6;

    // Status
    private bool isKnockedBack = false;
    private bool canTakeDamage = true;
    private float damageCooldown = 0.05f;

    // Children
    private Transform body = null;
    private Transform shadow = null;

    // Components
    private SpriteRenderer spriteRenderer = null;
    private Animator animator = null;
    private Rigidbody2D rigidBody = null;
    private EnemyMovement movement = null;
    private HitStop hitStop = null;


    private void Awake() {
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
    /// <param name="hitter"></param>
    private void OnHit(Transform hitter) {
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


    private void OnDeath() {
        hitStop.Hit(200);

        // Disable all components
        movement.enabled = false;
        rigidBody.bodyType = RigidbodyType2D.Static;

        Collider2D[] colliders = GetComponentsInChildren<Collider2D>();
        foreach (Collider2D c in colliders) {
            c.enabled = false;
        }

        StartCoroutine(DeathAnimation());
    }

    private IEnumerator DeathAnimation() {
        yield return new WaitForSeconds(0.01f);

        // Trigger animation
        animator.SetBool("Dead", true);
        spriteRenderer.sortingLayerName = "Shadows";
        spriteRenderer.sortingOrder = -10;

        // spriteRenderer.color = Color.HSVToRGB(0, 14, 39);
        // Vector3 rotation = new Vector3(0, 0, -45f);
        // transform.Rotate(rotation);
        Destroy(shadow.gameObject);
    }

    private IEnumerator DamageCooldown() {
        canTakeDamage = false;
        yield return new WaitForSeconds(damageCooldown);
        canTakeDamage = true;
    }

}

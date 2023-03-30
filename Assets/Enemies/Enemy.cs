using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("ENEMY")]
    [SerializeField][Tooltip("Objects in these Physics Layers will cause this object to take damage")]
    private LayerMask damageLayer;

    // Status
    private bool isKnockedBack = false;

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
    }


    // Checking for collisions
    private void OnTriggerEnter2D(Collider2D other) {
        if(((1<<other.gameObject.layer) & damageLayer) != 0) {
            Debug.Log($"[ENEMY] >>> Gotcha! {this.name} was damaged by {other.name}");
            OnHit(other.transform);
        }
    }

    /// <summary>
    /// Called when this Enemy takes damage.
    /// </summary>
    /// <param name="hitter"></param>
    private void OnHit(Transform hitter) {
        hitStop.Hit(100);

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

}

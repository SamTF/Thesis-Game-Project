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
    protected EnemyMovementBase movement = null;
    protected HitStop hitStop = null;
    protected CircleCollider2D bodyCollider = null;


    protected virtual void Awake() {
        // Getting child transforms
        body = body ? body : transform.Find("Body");
        shadow = shadow ? shadow : transform.Find("Shadow");
        
        // Getting components
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody2D>();
        movement = GetComponent<EnemyMovementBase>();
        hitStop = GetComponent<HitStop>();
        bodyCollider = body.GetComponent<CircleCollider2D>();

        // Checking the children for sprite renderers if the main object doesn't have one
        if (spriteRenderer == null) spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        // Fix in case this Enemy spawns outside the level
        if (OutOfBounds())  StartCoroutine(OutOfBoundsFix());
    }

    private bool OutOfBounds() {
        return Mathf.Abs(transform.position.x) >= GameManager.instance.LevelRadius.x
            || Mathf.Abs(transform.position.y) >+ GameManager.instance.LevelRadius.y;
        
    }

    private IEnumerator OutOfBoundsFix() {
        Collider2D[] colliders = body.GetComponents<Collider2D>();

        // disable all colliders in Body
        foreach (Collider2D col in colliders) {
            col.enabled = false;      
        }

        // wait until enemy is back inside the level
        while (OutOfBounds()) {
            yield return new WaitForFixedUpdate();
        }

        // re-enable colliders once they are inside
        foreach (Collider2D col in colliders) {
            col.enabled = true;      
        }
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
        int damage = 1;
        IDamage iDmg = null;

        // checking if the hitter has an IDamage component, and if so, use its value
        if(hitter.TryGetComponent<IDamage>(out iDmg))
            damage = iDmg.Damage;
        else if (hitter.parent.TryGetComponent<IDamage>(out iDmg))
            damage = iDmg.Damage;
        
        Debug.Log($"[ENEMY] >>> [{damage}] damage");
        
        // subtract damage taken from health
        health -= damage;

        // check if dead
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

        // Spawn XP Item!
        Vector2 bounceDirection = ((Vector2)transform.position - Player.instance.Position).normalized;
        ItemFactory.Spawn(ItemType.XP, transform.position, bounceDirection);

        // Chance to spawn Heart if not at full health
        if (!Player.instance.Health.FullHealth && Player.instance.Health.SpawnHeart) {
            bounceDirection = (bounceDirection * Random.Range(-0.5f, 0.5f)).normalized;
            ItemFactory.Spawn(ItemType.Heart, transform.position, bounceDirection);
        }

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


    // Public Getters
    public SpriteRenderer SpriteRenderer => spriteRenderer;

}

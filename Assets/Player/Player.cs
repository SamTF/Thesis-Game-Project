using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Components
    private Rigidbody2D     rb              = null;
    private InputManager    input           = null;
    private Stats           stats           = null;
    private Status          status          = null;
    private CircleCollider2D collider       = null;
    private Health          health          = null;
    private Animator        animator        = null;
    private HitStop         hitStop         = null;

    // Children
    [SerializeField]
    private Transform spriteObject = null;
    [SerializeField]
    private Transform shadow = null;



    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        input = GetComponent<InputManager>();
        stats = GetComponent<Stats>();
        status = GetComponent<Status>();
        collider = GetComponent<CircleCollider2D>();
        health = GetComponent<Health>();
        animator = GetComponent<Animator>();
        hitStop = GetComponentInChildren<HitStop>();

        spriteObject = spriteObject ? spriteObject : transform.Find("Sprite");
        shadow = shadow ? shadow : transform.Find("Shadow");
    }


    // Checking for collisions
    private void OnTriggerEnter2D(Collider2D other) {
        if(((1<<other.gameObject.layer) & health.DamageLayer) != 0) {
            Debug.Log($"[HEALTH] >>> Ouch! Player was damaged by {other.name}");
            OnHit(other.transform);
        }
    }

    /// <summary>
    /// Triggers when the Player is hit and takes damage.
    /// <param name="hitter">The Object that hit the player</param>
    /// </summary>
    private void OnHit(Transform hitter) {
        // Checking if the player can take damage.
        if (status.IsInvulnerable) {
            Debug.Log("Player can't take damage right now. They are invulnerable!");
            return;
        }

        // Take Damage on the Health component
        health.TakeDamage();

        if (health.HP <= 0 ) {
            OnDeath();
            return;
        }

        // Hit Stop! Freeze frame! Game feel! Oh yeah!
        hitStop.Hit();
        
        // Knockback away from the damage source
        status.IsKnockedBack = true;
        Vector2 knockbackVector = Vector2Int.RoundToInt((transform.position - hitter.position).normalized);
        rb.AddForce(knockbackVector * 800f);
    }

    private void OnDeath() {
        print("i am dead");

        // Long Freeze frame
        hitStop.Hit(2000);

        // Disable components
        input.enabled = false;
        collider.enabled = false;

        // Death Animation
        spriteObject.GetComponent<SpriteRenderer>().sortingLayerName = "Foreground";            // Change the body sprite sorting layer to Foreground
        shadow.parent = null;                                                                   // Unparent the shadow so it stays on the ground

        GameObject wings = Resources.Load("FX/Wings") as GameObject;                            // Load the Wings prefab
        Vector3 position = new Vector3(0, -0.5f, 0);
        Instantiate(wings, spriteObject.position + position, Quaternion.identity, spriteObject);// Instantiate the wins prefab

        GameObject fade = Resources.Load("FX/Fades/FadeToBlack") as GameObject;                 // Fade to black
        Instantiate(fade, transform.position, Quaternion.identity, null);

        rb.velocity = Vector2.up;                                                               // Make the player move up towards heaven
    }


    // Component Getters
    public Rigidbody2D RigidBody => rb;
    public InputManager Input => input;
    public Stats Stats => stats;
    public Status Status => status;
    public Health Health => health;
    public Animator Animator => animator;

    // Child getters
    public Transform SpriteObject => spriteObject;
    public Transform Shadow => shadow;
}

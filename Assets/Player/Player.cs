using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("PLAYER")]

    [Header("Properties")]
    [SerializeField][Tooltip("Physics Layer(s) that contains pickup-able Items.")]
    private LayerMask itemLayer = 0;

    [Header("Children")]
    [SerializeField][Tooltip("Child transform containing the Player Sprite (AKA Body).")]
    private Transform body = null;
    [SerializeField][Tooltip("Child Transform containing the Shadow sprite.")]
    private Transform shadow = null;

    // Components
    private Rigidbody2D     rb              = null;
    private InputManager    input           = null;
    private Stats           stats           = null;
    private Status          status          = null;
    private CircleCollider2D collider       = null;
    private Health          health          = null;
    private Stamina         stamina         = null;
    private Animator        animator        = null;
    private HitStop         hitStop         = null;
    private LevelSystem     levelSystem     = null;
    private ModdableSprite  moddableSprite  = null;
    private SpriteRenderer  spriteRenderer  = null;

    // Events
    /// <summary>Triggered when the Player updates their Sprite drawing.</summary>
    public static event System.Action onSpriteUpdated;

    /// Singleton thing
    private static Player _instance = null;
    public static Player instance
    {
        get {return _instance;}
    }


    private void Awake()
    {
        // Singleton - there can only be one!
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        else
            _instance = this;

        // Getting Components - Unity
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<CircleCollider2D>();
        animator = GetComponent<Animator>();

        // Getting my Components
        input = GetComponent<InputManager>();
        stats = GetComponent<Stats>();
        status = GetComponent<Status>();
        health = GetComponent<Health>();
        stamina = GetComponent<Stamina>();
        hitStop = GetComponentInChildren<HitStop>();
        levelSystem = LevelSystem.instance;
        moddableSprite = GetComponent<ModdableSprite>();

        // Children
        body = body ? body : transform.Find("Body");
        shadow = shadow ? shadow : transform.Find("Shadow");

        // Components in children
        spriteRenderer = body.GetComponent<SpriteRenderer>();

    }


    // Checking for collisions
    private void OnTriggerEnter2D(Collider2D other) {
        // With enemy / damaging objects
        if ( ((1<<other.gameObject.layer) & health.DamageLayer) != 0 ) {
            Debug.Log($"[HEALTH] >>> Ouch! Player was damaged by {other.name}");
            OnHit(other.transform);
        }

        // With Items
        if ( ((1<<other.gameObject.layer) & itemLayer) != 0 ) {
            Debug.Log($"[PLAYER] >>> Collided with {other.name}");
            Item item = other.GetComponent<Item>();
            ItemType itemType = item.ItemType;
            Debug.Log($"[PLAYER] >>> Picked up a {itemType}!!");

            // Do the item's effect
            OnItemPickup(itemType);

            // Despawn the item right away
            item.Despawn();
        }
    }

    // When the player remains inside a collider (only needed for enemies)
    private void OnTriggerStay2D(Collider2D other) {
        // With enemy / damaging objects
        if ( ((1<<other.gameObject.layer) & health.DamageLayer) != 0 ) {
            Debug.Log($"[HEALTH] >>> Ouch! Player was damaged by {other.name}");
            OnHit(other.transform);
        }
    }

    /// <summary>
    /// Triggered when an item is picked up.
    /// </summary>
    /// <param name="itemType">What kind of item has been picked up?</param>
    private void OnItemPickup(ItemType itemType) {
        // XP pick up
        if (itemType == ItemType.XP) {
            levelSystem.GainXP();
        }
        // Health pick up
        else if (itemType == ItemType.Heart) {
            health.Heal();
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
        body.GetComponent<SpriteRenderer>().sortingLayerName = "Foreground";            // Change the body sprite sorting layer to Foreground
        shadow.parent = null;                                                                   // Unparent the shadow so it stays on the ground

        GameObject wings = Resources.Load("FX/Wings") as GameObject;                            // Load the Wings prefab
        Vector3 position = new Vector3(0, -0.5f, 0);
        Instantiate(wings, body.position + position, Quaternion.identity, body);// Instantiate the wins prefab

        GameObject fade = Resources.Load("FX/Fades/FadeToBlack") as GameObject;                 // Fade to black
        Instantiate(fade, transform.position, Quaternion.identity, null);

        rb.velocity = Vector2.up;                                                               // Make the player move up towards heaven
    }


    /// <summary>
    /// Update the Player Sprite with a new one and re-calculate stats!
    /// </summary>
    /// <param name="newTexture">Texture to update the Sprite with.</param>
    public void UpdateSprite(Texture2D newTexture) {
        moddableSprite.ReplaceSprite(newTexture);
        
        if (HUDController.instance == null) {
            Debug.Log("where is the hud controller ????");
        } else {
            HUDController.instance.UpdatePlayerSprite();
        }

        onSpriteUpdated?.Invoke();

        ResetMovement();

        // become invulnerable for a short time after creating a new sprite
        StartCoroutine(BlinkingAnimation());
    }

    /// <summary>
    /// Stop dodging or backflipping
    /// </summary>
    public void ResetMovement() {
        // reset dodging or backflipping
        status.IsDodging = false;
        status.IsBackflipping = false;
    }

    /// <summary>
    /// Trigger a nice Blinking animation on the player sprite.
    /// </summary>
    private IEnumerator BlinkingAnimation() {
        if (status.IsInvulnerable)   yield return null;

        animator.SetBool("Blink", true);
        yield return new WaitForSeconds(status.InvulnerableCooldown);
        animator.SetBool("Blink", false);
    }


    /// <summary>Reset the singleton if the object is ever destroyed</summary>
    private void OnDestroy() {
        if (this == _instance) { _instance = null; }
    }


    // Component Getters
    public Vector2 Position => transform.position;
    public Rigidbody2D RigidBody => rb;
    public InputManager Input => input;
    public Stats Stats => stats;
    public Status Status => status;
    public Health Health => health;
    public Animator Animator => animator;
    public ModdableSprite ModdableSprite => moddableSprite;
    public Stamina Stamina => stamina;

    // Child getters
    public Transform Body => body;
    public Transform Shadow => shadow;
    public Sprite Sprite => spriteRenderer.sprite;
    public SpriteRenderer SpriteRenderer => spriteRenderer;
}

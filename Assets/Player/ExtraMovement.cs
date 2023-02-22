using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class controls special movement beyond walking around. It includes dodging, jumping, and backflipping!
/// </summary>
public class ExtraMovement : MonoBehaviour
{
    // Vars
    private float timeToFlip = 0.15f;

    // Components
    private Player player = null;
    private InputManager input = null;
    private Rigidbody2D rb = null;
    private Backflip backflip = null;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Player>();
        input = GetComponent<InputManager>();
        rb = GetComponent<Rigidbody2D>();
        backflip = GetComponent<Backflip>();
    }

    // Update is called once per frame
    void Update()
    {
        // Movement Input
        Vector2 movement = input.Movement;

        // FLIP!!!
        if (
            input.JumpPress
            && input.TimeSinceDirectionSwitch.x <= timeToFlip
            && player.Status.CanBackflip
            && !player.Status.IsDodging
            && !player.Status.IsBackflipping
        ) {
            backflip.PerformBackflip(movement);
            StartCoroutine(Roll());
        }

        // Dodging
        else if (
            input.JumpPress
            && !player.Status.IsDodging
            && !player.Status.IsBackflipping
            && player.Status.CanDodge
        ) {
            Vector2 direction = movement;
            Dodge(direction);
        }
    }

    /// <summary>
    /// Perform a dodge roll with i-frames. Speed/Length based on MoveSpeed stat.
    /// </summary>
    /// <param name="direction">Direction in which to apply the dodge motion</param>
    public void Dodge(Vector2 direction) {
        // Dodging needs a direction
        if (direction == Vector2.zero) return;

        Debug.Log($"Jump in direction {direction}");
        rb.AddForce(direction * 10f, ForceMode2D.Impulse);

        // StartCoroutine(DodgeCooldown());
        StartCoroutine(DodgeLength());
        StartCoroutine(Roll());
    }

    /// <summary>
    /// Controls how long the player can dodge for (in seconds)
    /// </summary>
    private IEnumerator DodgeLength() {
        player.Status.IsDodging = true;
        yield return new WaitForSeconds(0.25f);
        player.Status.IsDodging = false;
    }


    /// <summary>
    /// Make the sprite perform a centered rolling animation.
    /// </summary>
    private IEnumerator Roll() {
        Transform t = player.SpriteObject;
        SpriteRenderer sr = player.SpriteObject.GetComponent<SpriteRenderer>();

        // Original values
        Vector2 originalPostion = t.position;
        Vector3 originalRotation = t.eulerAngles;
        Sprite originalSprite = sr.sprite;
        Texture2D tex = sr.sprite.texture;

        // New centered sprite
        Sprite centeredSprite = ImageLoader.CreateSprite(tex, Pivot.Center);
        sr.sprite = centeredSprite;

        // Sprite offset
        Vector2 offset = t.position;
        offset.y += 0.5f;
        t.position = offset;

        // Rotate the Sprite transform object
        while (player.Status.IsJumping || player.Status.IsDodging || player.Status.IsBackflipping) {
            t.Rotate(new Vector3(0, 0, -5f));
            yield return new WaitForEndOfFrame();
        }

        // Resetting values to their original state
        // t.position = Vector3.zero;
        t.localPosition = Vector3.zero;
        t.eulerAngles = originalRotation;
        sr.sprite = originalSprite;
    }
}

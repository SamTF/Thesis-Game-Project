using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class controls special movement beyond walking around. It includes dodging, jumping, and backflipping!
/// </summary>
public class ExtraMovement : MonoBehaviour
{
    // Constants
    private const float timeToFlip = 0.15f;
    private const float rollSpeed = 20f;

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
            backflip.PerformBackflip(movement);             // start the backflip movement
            Axis axis = movement.x != 0 ? Axis.Z : Axis.X;  // getting the correct axis for the current direction
            bool clockwise = movement.x > 0;                // setting correct rotation direction
            StartCoroutine( Roll(rollSpeed/2, axis, clockwise) );   // start the rolling animation
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

        // Set dodging status to true
        player.Status.IsDodging = true;

        // Dodge speed
        rb.AddForce(direction * 10f, ForceMode2D.Impulse);

        // Roll animation
        bool clockwise = direction.x > 0;
        StartCoroutine(Roll(clockwise:clockwise));
    }


    /// <summary>
    /// Make the sprite perform a centered rolling animation.
    /// </summary>
    private IEnumerator Roll(float speed=rollSpeed, Axis axis=null, bool clockwise=true) {
        Transform body = player.SpriteObject;
        SpriteRenderer sr = player.SpriteObject.GetComponent<SpriteRenderer>();

        // Original values
        Vector2 originalPostion = body.position;
        Vector3 originalRotation = body.eulerAngles;
        Sprite originalSprite = sr.sprite;
        Texture2D tex = sr.sprite.texture;

        // New centered sprite
        Sprite centeredSprite = ImageLoader.CreateSprite(tex, Pivot.Center);
        sr.sprite = centeredSprite;

        // Sprite offset
        Vector2 offset = body.position;
        offset.y += 0.5f;
        body.position = offset;

        // Rotate the Sprite transform object
        while (player.Status.IsJumping || player.Status.IsDodging || player.Status.IsBackflipping) {
            // Getting right axis
            Vector3 rotationAxis;
            if (axis == null)   rotationAxis = Axis.Z.value;
            else                rotationAxis = axis.value;
            
            // Creating the rotation vectoe
            float direction = clockwise ? -1 : 1;
            // Vector3 rotationVector = new Vector3(0, 0, speed * direction);
            Vector3 rotationVector = rotationAxis * (speed * direction);

            body.Rotate(rotationVector);

            yield return new WaitForFixedUpdate();
        }

        // Resetting values to their original state
        sr.sprite = originalSprite;
        body.localPosition = Vector3.zero;
        body.eulerAngles = originalRotation;
    }
}

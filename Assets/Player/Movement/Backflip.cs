using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Backflip : MonoBehaviour
{
    // Constants
    /// <summary>Speed at which the character moves vertically as the backflip is performed.</summary>
    private const float verticalVelocity = 0.5f;
    /// <summary>Downwards force applied to vertical movement.</summary>
    private const float gravity = -(verticalVelocity * 1/16f);
    /// <summary>How far you travel during the backflip.</summary>
    private const float backflipDistance = 9f;

    // Vars
    private float newVerticalVelocity = verticalVelocity;
    private Vector2 originPos;
    private Vector3 originRotation;
    

    // Components
    private Player player = null;
    private Rigidbody2D rb = null;
    private InputManager input = null;
    private Collider2D collider = null;

    private void Awake() {
        player = GetComponent<Player>();
        rb = GetComponent<Rigidbody2D>();
        input = GetComponent<InputManager>();
        collider = GetComponent<Collider2D>();
    }

    private void FixedUpdate() {
        if (player.Status.IsBackflipping) {            
            // Fake vertical movement
            newVerticalVelocity += gravity;
            player.Body.localPosition += new Vector3(0, newVerticalVelocity, 0);

            // Disable collider
            collider.enabled = false;

            // Reset when the player touches the ground
            if (player.Body.localPosition.y <= 0) {
                player.Status.IsBackflipping = false;               // setting backflipping status to false to stop the movement

                player.Body.localPosition = Vector3.zero;           // resetting sprite transform to original position
                player.transform.eulerAngles = originRotation;      // ... and rotation
                player.Shadow.localScale = Vector2.one;             // resetting shadow sprite scale

                newVerticalVelocity = verticalVelocity;             // resetting vertical velocity

                collider.enabled = true;                            // re-enable collider
            }

        }
    }

    /// <summary>
    /// Performs a cool blackflip! WOW!!
    /// </summary>
    /// <param name="direction">Vector2 direction to flip in. Only X-Axis actually matters</param>
    public void PerformBackflip(Vector2 direction) {
        // Can only backflip in 4 directions (not diagonaly) - for now anyway
        if (direction.magnitude != 1)   return;

        // getting the correct axis for the current direction - less speed on vertical backflip (X Axis)
        Axis axis = direction.x != 0 ? Axis.Z : Axis.X;
        float speedMod = axis == Axis.Z ? 1f : 0.5f;

        // storing original transform position
        originPos = player.Body.position;
        originRotation = player.transform.eulerAngles;

        // Using Speed stat to increase backflip distance
        float backflipForce = backflipDistance * (player.Stats.MoveSpeed.Value / (player.Stats.MoveSpeed.valueModifier * 3f));
        backflipForce = Mathf.Clamp(backflipForce, backflipDistance, 20f);

        // adding horizontal force and setting flipping status
        rb.AddForce(direction * (backflipForce) * speedMod, ForceMode2D.Impulse);
        player.Status.IsBackflipping = true;
    }       
    
}

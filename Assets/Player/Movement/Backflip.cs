using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Backflip : MonoBehaviour
{
    // Constants
    private const float gravity = -(0.4f * 1/16f);
    private const float verticalVelocityOG = 0.4f;
    private const float backflipSpeed = 7.5f;

    // Vars
    private float verticalVelocity = verticalVelocityOG;
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
            verticalVelocity += gravity;
            player.Body.localPosition += new Vector3(0, verticalVelocity, 0);

            // Disable collider
            collider.enabled = false;

            // Reset when the player touches the ground
            if (player.Body.localPosition.y <= 0) {
                player.Status.IsBackflipping = false;               // setting backflipping status to false to stop the movement

                player.Body.localPosition = Vector3.zero;   // resetting sprite transform to original position
                player.transform.eulerAngles = originRotation;      // ... and rotation
                player.Shadow.localScale = Vector2.one;             // resetting shadow sprite scale

                verticalVelocity = verticalVelocityOG;              // resetting vertical velocity

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

        // adding horizontal force and setting flipping status
        rb.AddForce(direction * backflipSpeed * speedMod, ForceMode2D.Impulse);
        player.Status.IsBackflipping = true;
    }       
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Backflip : MonoBehaviour
{
    // Constants
    private const float gravity = -(0.33f * 1/16f);
    // private const float verticalVelocityOG = 0.25f;
    private const float verticalVelocityOG = 0.33f;
    private const float horizontalVelocity = 5f;

    // Vars
    private float verticalVelocity = verticalVelocityOG;
    private Vector2 originPos;
    private Vector3 originRotation;
    

    // Components
    private Player player = null;
    private Rigidbody2D rb = null;
    private InputManager input = null;

    private void Awake() {
        player = GetComponent<Player>();
        rb = GetComponent<Rigidbody2D>();
        input = GetComponent<InputManager>();
    }

    private void FixedUpdate() {
        if (player.Status.IsBackflipping) {            
            // Fake vertical movement
            verticalVelocity += gravity;
            player.SpriteObject.localPosition += new Vector3(0, verticalVelocity, 0);

            // Reset when the player touches the ground
            if (player.SpriteObject.localPosition.y <= 0) {
                player.Status.IsBackflipping = false;               // setting backflipping status to false to stop the movement

                player.SpriteObject.localPosition = Vector3.zero;   // resetting sprite transform to original position
                player.transform.eulerAngles = originRotation;      // ... and rotation
                player.Shadow.localScale = Vector2.one;             // resetting shadow sprite scale

                verticalVelocity = verticalVelocityOG;              // resetting vertical velocity
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

        Axis axis = direction.x == 1? Axis.Z : Axis.X;

        // storing original transform position
        originPos = player.SpriteObject.position;
        originRotation = player.transform.eulerAngles;

        // adding horizontal force and setting flipping status
        rb.AddForce(direction * 5f, ForceMode2D.Impulse);
        player.Status.IsBackflipping = true;
    }       
    
}

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
                player.SpriteObject.localPosition = Vector3.zero;
                player.Status.IsBackflipping = false;
                verticalVelocity = verticalVelocityOG;
            }

        }
    }

    /// <summary>
    /// Performs a cool blackflip! WOW!!
    /// </summary>
    /// <param name="direction">Vector2 direction to flip in. Only X-Axis actually matters</param>
    public void PerformBackflip(Vector2 direction) {
        // storing original transform position
        originPos = player.SpriteObject.position;

        // adding horizontal force and setting flipping status
        direction = new Vector2(direction.x, 0f);
        rb.AddForce(direction * 5f, ForceMode2D.Impulse);
        player.Status.IsBackflipping = true;
    }       
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Backflip : MonoBehaviour
{
    // Vars
    // private Direction[] directionHistory = new Direction[2];
    [SerializeField]
    private float[] directionHistory = new float[2];
    private float movementX;
    private float movementY;
    private float timeToFlip = 0.15f;
    [SerializeField]
    private float timeSinceDirectionSwitch = 0.5f;

    private float timeA;

    private Dictionary<float, Direction> dict = new Dictionary<float, Direction> {
        {1f, Direction.Right},
        {-1f, Direction.Left}
    };

    // Components
    private Player player = null;
    private Rigidbody2D rb = null;
    private InputManager input = null;

    private void Awake() {
        player = GetComponent<Player>();
        rb = GetComponent<Rigidbody2D>();
        input = GetComponent<InputManager>();
    }

    private void Update() {
        /// Movement Input
        movementX = input.MovementX;
        movementY = input.MovementY;

        // Add to the timer is moving in the same direction or not moving at all
        if (movementX == 0 || movementX == directionHistory[0]) {
            timeSinceDirectionSwitch += Time.deltaTime;
        }

        // checking if a new key is being pressed
        if (movementX != directionHistory[0]) {
            // ignore standing still
            if (movementX == 0) return;
           
            // storing the old direction (left/right only, no zero allowed)
            if (directionHistory[0] != 0) {
                directionHistory[1] = directionHistory[0];

                // reset timer when changing direction
                timeSinceDirectionSwitch = 0f;
            }
            
            // New current direction
            directionHistory[0] = movementX;
        };

        // Time Frame Visualiser
        if (timeSinceDirectionSwitch <= timeToFlip) {
            GetComponentInChildren<SpriteRenderer>().color = Color.cyan;
        } else {
            GetComponentInChildren<SpriteRenderer>().color = Color.white;
        }

        // FLIP!!!
        if (input.JumpPress && timeSinceDirectionSwitch <= timeToFlip) {
            Debug.Log("!!! FLIPP !!!");
            rb.AddForce(Vector2.up * 10f, ForceMode2D.Impulse);
        }

        
    }
}

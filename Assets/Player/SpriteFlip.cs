using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteFlip : MonoBehaviour
{
    private SpriteRenderer spriteRenderer = null;
    private Player player = null;
    private InputManager input = null;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = GetComponentInParent<Player>();
        input = GetComponentInParent<InputManager>();
    }

    private void Update() {
        // Flip sprite to face attack (prioritiy over movement)
        if      (input.AttackX < 0) spriteRenderer.flipX = true;
        else if (input.AttackX > 0) spriteRenderer.flipX = false;

        // Flip sprite to face direction of movement (if not attacking)
        else if (input.MovementX < 0) spriteRenderer.flipX = true;
        else if (input.MovementX > 0) spriteRenderer.flipX = false;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : Enemy
{
    [SerializeField][Tooltip("How many seconds until this Enemy respawns after leaving the screen.")][Range(1f, 10f)]
    private float respawnTime = 5f;

    // Status
    private bool isMoving = false;

    // Components
    private GhostMovement ghostMovement = null;


    private void Start() {
        ghostMovement = GetComponent<GhostMovement>();
    }

    

    /// <summary>
    /// Toggles this Enemy's visual and physical components on and off.
    /// </summary>
    /// <param name="value">On or off.</param>
    private void ToggleComponents(bool value) {
        bodyCollider.enabled = value;
        spriteRenderer.enabled = value;
        shadow.GetComponent<SpriteRenderer>().enabled = value;
    }


    // Public Getters / Setters

    /// <summary>Whether this enemy is moving or not. Setting its value also enables/disable components.</summary>
    /// <value>True or false.</value>
    public bool IsMoving {
        get { return isMoving; }
        set {
            isMoving = value;
            ToggleComponents(value);
        }
    }

    /// <summary>How many seconds until this Enemy respawns after leaving the screen.</summary>
    public float RespawnTime => respawnTime;
}

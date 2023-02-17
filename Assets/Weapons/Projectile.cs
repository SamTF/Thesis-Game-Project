using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Generic projectile with constant movement direction and speed
/// </summary>
public class Projectile : MonoBehaviour
{
    // Stats
    private float mySpeed = 0f;
    private Vector2 myDirection;
    
    // Components
    private Rigidbody2D rb = null;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start() {
        rb.velocity = Vector2.zero;
    }

    private void FixedUpdate() {
        rb.velocity = (myDirection * mySpeed);
    }

    /// <summary>
    /// Basic setup function for the projectile to determine it's direction and speed.
    /// </summary>
    /// <param name="direction">Vector2 direction the projectile will travel in</param>
    /// <param name="speed">Float speed at which it will travel</param>
    public void Shoot(Vector2 direction, float speed) {
        myDirection = direction;
        mySpeed = speed;
    }

    /// Destroy it when it leaves the screen
    private void OnBecameInvisible() {
        Destroy(gameObject);
    }
}

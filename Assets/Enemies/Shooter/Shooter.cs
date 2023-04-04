using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Ranged enemy that shoots projectiles at the Player and prefers medium-range combat.
/// Follows the player, but not too closely, and runs away if the Player closes the distance.
/// </summary>
public class Shooter : Enemy
{
    [Header("SHOOTER")]
    [SerializeField][Tooltip("Projectile that this gun shoots")]
    private GameObject projectilePrefab = null;

    /// <summary>Objects in these Physics Layers will cause this Enemy to take damage on hit.</summary>
    [SerializeField][Tooltip("The projectile will hit objects in these physics layers.")]
    private LayerMask projectileLayer;

    [SerializeField][Tooltip("Speed at which the projectile will travel.")]
    private float projectileSpeed = 6f;


    private void Start() {
        StartCoroutine(ShootBehaviour());
    }


    private IEnumerator ShootBehaviour() {
        while (true) {
            yield return new WaitForSeconds(1f);

            Vector2 shootingVector = (GameManager.instance.PlayerPosition - (Vector2)transform.position).normalized;
            Shoot(shootingVector, projectileSpeed, projectileLayer);   
        }
    }

    /// <summary>
    /// Shoot a projectile.
    /// </summary>
    /// <param name="shootingVector">Direction for the projectile to travel in.</param>
    /// <param name="speed">Speed for the projectile to travel at.</param>
    /// <param name="targetLayer">Physics Layers for the porjectile to check for collisions with.</param>
    private void Shoot(Vector2 shootingVector, float speed, LayerMask targetLayer) {
        // Instantiate the Projectile
        GameObject projectile = ProjectileFactory.Instantiate(
            projectilePrefab,
            transform.position,
            null,
            shootingVector,
            speed,
            targetLayer,
            6
        );
    }
}

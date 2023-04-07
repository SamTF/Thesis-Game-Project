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
    [SerializeField][Tooltip("How many seconds between each shot fired")][Range(1, 5)]
    private int shootingInterval = 4;

    [SerializeField][Tooltip("Projectile that this gun shoots")]
    private GameObject projectilePrefab = null;

    /// <summary>Objects in these Physics Layers will cause this Enemy to take damage on hit.</summary>
    [SerializeField][Tooltip("The projectile will hit objects in these physics layers.")]
    private LayerMask projectileLayer;

    [SerializeField][Tooltip("Speed at which the projectile will travel.")]
    private float projectileSpeed = 6f;

    [SerializeField][Tooltip("How many units the projectile travels before being affected by gravity")][Range(5,15)]
    private int projectileRange = 10;


    private void Start() {
        StartCoroutine(ShootBehaviour());
    }


    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private IEnumerator ShootBehaviour() {
        while (true) {
            yield return new WaitForSeconds(shootingInterval);

            Vector2 shootingVector = (GameManager.instance.PlayerPosition - (Vector2)transform.position).normalized;
            Shoot(shootingVector, projectileSpeed, projectileLayer, projectileRange);   
        }
    }

    /// <summary>
    /// Shoot a projectile.
    /// </summary>
    /// <param name="shootingVector">Direction for the projectile to travel in.</param>
    /// <param name="speed">Speed for the projectile to travel at.</param>
    /// <param name="targetLayer">Physics Layers for the porjectile to check for collisions with.</param>
    /// <param name="range">How many units the projectile will travel before fall-off.</param>
    private void Shoot(Vector2 shootingVector, float speed, LayerMask targetLayer, int range) {
        // Instantiate the Projectile
        GameObject projectile = ProjectileFactory.Instantiate(
            projectilePrefab,
            transform.position,
            null,
            shootingVector,
            speed,
            targetLayer,
            range
        );
    }
}

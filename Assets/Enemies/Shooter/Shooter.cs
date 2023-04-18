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

    [SerializeField][Tooltip("Speed at which the projectile will travel.")][Range(5, 15)]
    private float projectileSpeed = 6f;

    [SerializeField][Tooltip("How many units the projectile travels before being affected by gravity")][Range(5,15)]
    private int projectileRange = 10;

    [SerializeField][Tooltip("Whether this enemy is allowed to Shoot projectile while moving")]
    private bool canShootWhileMoving = false;
    private bool canShootWhileRunningAway = false;

    // Components
    private ShooterMovement myMovement = null;

    
    // Start shooting behaviour
    private void Start() {
        myMovement = GetComponent<ShooterMovement>();
        
        StartCoroutine(ShootBehaviour(1f));
    }

    /// <summary>
    /// Starts the shooting pattern for this enemy.
    /// </summary>
    /// <param name="startDelay">How many seconds before the shooting behaviour begins</param>
    private IEnumerator ShootBehaviour(float startDelay=1f) {
        // Add some randomised delay (if delay is wanted) so that enemies don't all shoot at the exact same time
        if (startDelay > 0f) startDelay += Random.Range(0f, 2f);
        yield return new WaitForSeconds(startDelay);

        while (true) {
            yield return new WaitForSeconds(0.1f);
            
            // Don't shoot while moving or running away
            if (!CanShoot())    continue;

            // Attack telegraph -> let the Player know that this enemy is about to SHOOT
            animator.SetBool("AttackTelegraph", true);
            yield return new WaitForSeconds(0.5f);
            animator.SetBool("AttackTelegraph", false);

            // Shoot
            Vector2 shootingVector = (GameManager.instance.PlayerPosition - (Vector2)transform.position).normalized;
            Shoot(shootingVector, projectileSpeed, projectileLayer, projectileRange);

            // Shooting cooldown
            yield return new WaitForSeconds(shootingInterval);
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


    /// <summary>
    /// Check if the enemy is currently allowed to shoot.
    /// </summary>
    /// <returns>Whether the enemy is allowed to shoot.</returns>
    private bool CanShoot() {
        // False if can't shoot while running away and is currently running away
        if (!canShootWhileRunningAway && myMovement.IsRunningAway) {
            return false;
        }
        // False if can't shoot while moving and performing movement
        if (!canShootWhileMoving && myMovement.IsMoving) {
            return false;
        }

        // Otherwise true
        return true;
    }
}

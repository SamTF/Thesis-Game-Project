using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    // Prefab
    [Header("Generic Weapon")]
    [SerializeField][Tooltip("Projectile that this gun shoots")]
    private GameObject projectilePrefab = null;
    [SerializeField][Tooltip("How fast the projectile will travel")]
    private float baseShotSpeed = 6f;
    [SerializeField][Tooltip("The spawn points for the projectiles.")]
    private ShootPoints shootPoints = null;
    [SerializeField][Tooltip("The Projectile will damage Objects in this layer.")]
    private LayerMask targetLayer;
    [SerializeField]
    private float minRange = 2.5f;

    // BOI-Shooting Variables
    // float timeToBeginOffset = 0.15f; // feels like this value should be affected by the character's move speed
    float timeToBeginOffset = 0.25f; // feels like this value should be affected by the character's move speed
    // float timeOffsetStep = 0.75f;
    float timeOffsetStep = 0.33f;
    Vector2 minOffset = new Vector2(0.25f, 0.25f);
    Vector2 maxOffset = new Vector2(0.75f, 1f);

    // Components
    private Player player = null;
    private InputManager input = null;

    private bool canShoot = true;

    void Start()
    {
        // Components
        player = GetComponentInParent<Player>();
        input = player.Input;

        // Stats
        baseShotSpeed = player.Stats.ShotSpeed.baseValue;
    }

    // Update is called once per frame
    void Update()
    {
        // If the player is pressing any attack buttons & is alive
        if (input.IsAttacking && Player.instance.Status.IsAlive) {
            // checking if the player can shoot
            if (!player.Status.CanShoot)  return;

            // Horizontal Attack
            if (input.AttackX > 0)      Shoot(Direction.Right);
            else if (input.AttackX < 0) Shoot(Direction.Left);
            // Vertical Attack
            else if (input.AttackY > 0) Shoot(Direction.Up);
            else if (input.AttackY < 0) Shoot(Direction.Down);
        }
    }

    /// <summary>
    /// Instantiate the projectile and move it in the given direction
    /// </summary>
    /// <param name="direction">Vector2 (Up, Down, Left, or Right)</param>
    private void Shoot(Direction direction) {
        // Direction for the projectile to move in
        Vector2 shootingVector = GetShootingVector(direction);

        // Direction the player is moving in
        Vector2 movementVector = input.Movement;
        
        // Setting Shot speed value
        float speed = baseShotSpeed + (player.Stats.ShotSpeed.Value / player.Stats.ShotSpeed.valueModifier );

        // Checking if moving and shooting in the same direction
        if (movementVector == shootingVector) {
            speed *= 1.5f;
        }

        // Setting shot range value
        float range = Mathf.Clamp(player.Stats.ShotSpeed.Value / 12f, minRange, 20);

        // Setting the damage value from the Attack stat
        int damage = player.Stats.Attack.baseValue + Mathf.FloorToInt(player.Stats.Attack.Value / player.Stats.Attack.valueModifier);

        // Debug.Log(shootingVector);

        // Getting the appropriate spawn point for the projectile given the aiming direction
        Vector2 shootPoint = shootPoints.Direction2ShootPoint(direction);

        // Instantiate the Projectile
        GameObject projectile = ProjectileFactory.Instantiate(
            projectilePrefab,
            shootPoint,
            null,
            shootingVector,
            speed,
            targetLayer,
            range,
            damage
        );

        // Cooldown until able to shoot again
        player.Status.CanShoot = false;
    }

    /// <summary>
    /// Convert Direction Enum into Vector2.
    /// </summary>
    /// <param name="direction">Direction Enum</param>
    /// <returns></returns>
    private Vector2 GetDirectionVector(Direction direction) {
        switch (direction)
        {
            case Direction.Right:
                return Vector2.right;
            case Direction.Left:
                return Vector2.left;
            case Direction.Up:
                return Vector2.up;
            case Direction.Down:
                return Vector2.down;

            default:
                return Vector2.right;
        }
    }

    /// <summary>
    /// Gets the Vector2 for the projectile direction, offset by the player's movement direction.
    /// Inspired by TBOI's shooting mechanics :)
    /// </summary>
    /// <param name="direction">Direction in which the player is aiming at</param>
    /// <returns>Vector2 for the projectile's movement direction.</returns>
    private Vector2 GetShootingVector(Direction direction) {
        // Direction the player is moving in
        Vector2 movementVector = input.Movement;

        // Direction the player is shooting in
        Vector2 shootingVector = GetDirectionVector(direction);
        // these variables need a lot of tweaking but it works perfectly!!!

        // IF SHOOTING HORIZONTALLY WHILE MOVING VERTICALLY
        if (shootingVector.x != 0 && movementVector.y != 0) {
            // checking for a minimum time to hold direction before applying offset
            if (input.TimeHoldingDirection[0] < timeToBeginOffset) {
                return shootingVector;
            };

            // creating the offset vector
            float offset = input.TimeHoldingDirection[0] * timeOffsetStep;
            offset = Mathf.Clamp(offset, minOffset.x, maxOffset.x);
            // Debug.Log(offset);
            shootingVector.y = movementVector.y * offset;
        }
        // SHOOTING VERTICALLY WHILE MOVING HORIZONTALLY
        else if (shootingVector.y != 0 && movementVector.x !=0 ) {
            // checking for a minimum time to hold direction before applying offset
            if (input.TimeHoldingDirection[0] < timeToBeginOffset) {
                return shootingVector;
            }

            // creating the offset vector
            float offset = input.TimeHoldingDirection[0] * timeOffsetStep;
            offset = Mathf.Clamp(offset, minOffset.y, maxOffset.y);
            // Debug.Log(offset);
            shootingVector.x = movementVector.x * offset;
        }

        // Return the vector after all these calculations
        return shootingVector;
    }

}

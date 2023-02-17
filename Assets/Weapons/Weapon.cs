using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    // Prefab
    [Header("Generic Weapon")]
    [SerializeField]
    private GameObject projectilePrefab = null;
    [SerializeField]
    private float shootingCooldown = 0.5f;
    [SerializeField]
    private float shotSpeed = 300f;

    // Components
    private Player player = null;
    private InputManager input = null;

    private bool canShoot = true;

    void Start()
    {
        player = GetComponentInParent<Player>();
        input = player.Input;
    }

    // Update is called once per frame
    void Update()
    {
        // If the player is pressing any attack buttons
        if (input.AttackX != 0 || input.AttackY != 0) {
            // checking if the player can shoot
            if (!canShoot)  return;

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
        Vector2 directionVector = GetDirectionVector(direction);

        // Instantiate the Projectile
        // GameObject projectile = Instantiate(projectilePrefab, transform.position, transform.rotation);
        // projectile.GetComponent<Projectile>().Shoot(directionVector, shotSpeed);
        GameObject projectile = ProjectileFactory.Instantiate(
            projectilePrefab,
            transform.position,
            null,
            directionVector,
            shotSpeed
        );

        // Cooldown until able to shoot again
        StartCoroutine(ShootingCooldown());
    }

    private IEnumerator ShootingCooldown() {
        canShoot = false;
        yield return new WaitForSeconds(shootingCooldown);
        canShoot = true;
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
}

using UnityEngine;

/// <summary>
/// Helper class that instantiates projectiles easily.
/// </summary>
public static class ProjectileFactory
{
    public static int baseRange = 3;

    /// <summary>
    /// Instantiate a projectile with attributes set already
    /// </summary>
    /// <param name="prefab">The GameObject of the projectile to spawn</param>
    /// <param name="position">Its world position</param>
    /// <param name="parent">Its transform parent</param>
    /// <param name="direction">Direction to move the projectile at</param>
    /// <param name="speed">Speed at which to move the projectile</param>
    /// <param name="targetLayer">Physics layer that this projectile will collide with</param>
    /// <returns>GameObject instance of the projectile</returns>
    public static GameObject Instantiate(
        GameObject prefab,
        Vector2 position,
        Transform parent,
        Vector2 direction,
        float speed,
        LayerMask targetLayer
        )
    {
        GameObject projectile = Object.Instantiate(prefab, position, Quaternion.identity, parent);
        float range = baseRange + Random.Range(-0.5f, 0.5f);
        projectile.GetComponent<Projectile>().Shoot(direction, speed, range, targetLayer);

        return projectile;
    }
}

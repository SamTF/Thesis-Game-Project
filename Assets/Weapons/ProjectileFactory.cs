using UnityEngine;

/// <summary>
/// Helper class that instantiates projectiles easily.
/// </summary>
public static class ProjectileFactory
{
    /// <summary>
    /// Instantiate a projectile with attributes set already
    /// </summary>
    /// <param name="prefab">The GameObject of the projectile to spawn</param>
    /// <param name="position">Its world position</param>
    /// <param name="parent">Its transform parent</param>
    /// <param name="direction">Direction to move the projectile at</param>
    /// <param name="speed">Speed at which to move the projectile</param>
    /// <param name="targetLayer">Physics layer that this projectile will collide with</param>
    /// <param name="baseRange">How far this projectile will travel before gravity fall-off is applied</param>
    /// <param name="damage">How much damage this projectile inflicts on its target</param>
    /// <returns>GameObject instance of the projectile</returns>
    public static GameObject Instantiate(
        GameObject prefab,
        Vector2 position,
        Transform parent,
        Vector2 direction,
        float speed,
        LayerMask targetLayer,
        float baseRange = 3,
        int damage = 1
        )
    {
        GameObject projectile = Object.Instantiate(prefab, position, Quaternion.identity, parent);
        float range = baseRange + Random.Range(-0.5f, 0.5f);
        projectile.GetComponent<Projectile>().Shoot(direction, speed, range, targetLayer, damage);

        return projectile;
    }
}

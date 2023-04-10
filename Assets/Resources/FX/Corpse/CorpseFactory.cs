using UnityEngine;

/// <summary>
/// Helper class to easily instantiate Corpses.
/// </summary>
public static class CorpseFactory
{
    /// <summary>
    /// Creates a Corpse FX Object on the battlefield, which is a sprite with some effects applied and no components.
    /// </summary>
    /// <param name="position">Position to place the Corpse.</param>
    /// <param name="rotation">Its angle rotation.</param>
    /// <param name="sprite">The sprite image to give to the Corpse.</param>
    /// <param name="scale">The size of the Corpse transform (Default: 1,1)</param>
    /// <returns>A Corpse GameObject</returns>
    public static GameObject Instantiate(Vector3 position, Vector3 rotation, Sprite sprite, Vector2? scale=null) {
        GameObject corpsePrefab = Resources.Load<GameObject>("FX/Corpse/Corpse");

        GameObject corpse = Object.Instantiate(
            corpsePrefab,
            position,
            Quaternion.Euler(rotation.x, rotation.y, rotation.z),
            null
        );

        corpse.GetComponent<Corpse>().Init(
            sprite,
            scale == null ? Vector2.one : scale.Value
            );

        return corpse;
    }
}

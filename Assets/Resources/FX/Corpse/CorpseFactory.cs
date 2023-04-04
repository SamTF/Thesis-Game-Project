using UnityEngine;

/// <summary>
/// Helper class to easily instantiate Corpses.
/// </summary>
public static class CorpseFactory
{
    public static GameObject Instantiate(Vector3 position, Vector3 rotation, Sprite sprite) {
        GameObject corpsePrefab = Resources.Load<GameObject>("FX/Corpse/Corpse");

        GameObject corpse = Object.Instantiate(
            corpsePrefab,
            position,
            Quaternion.Euler(rotation.x, rotation.y, rotation.z),
            null
        );

        corpse.GetComponent<Corpse>().Init(sprite);

        return corpse;
    }
}

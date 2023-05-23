using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Helper Class that spawns Items
/// </summary>
public static class ItemFactory
{
    /// <summary>
    /// Spawns a type of Item into the game.
    /// </summary>
    /// <param name="itemType">Which kind of Item to spawn.</param>
    /// <param name="position">Where to spawn the Item.</param>
    /// <param name="bounceDirection">Direction for the Item Bounce method.</param>
    /// <returns></returns>
    public static GameObject Spawn(ItemType itemType, Vector3 position, Vector2 bounceDirection) {
        GameObject itemPrefab = Resources.Load<GameObject>("Items/XP");

        switch (itemType)
        {
            case ItemType.Heart:
                itemPrefab = Resources.Load<GameObject>("Items/Heart");
                break;

            case ItemType.XP:
            default:
                itemPrefab = Resources.Load<GameObject>("Items/XP");
                break;
        }

        // Spawn the Item
        GameObject item = Object.Instantiate(
            itemPrefab,
            position,
            Quaternion.identity
        );

        // Initialise the Item
        item.GetComponent<Item>().Spawn(bounceDirection);

        // Return the spawned Item
        return item;
    }
}

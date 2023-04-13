using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Helper Class that spawns Items
/// </summary>
public static class ItemFactory
{
    public enum ItemType {
        XP, Heart
    }


    public static GameObject Spawn(ItemType itemType, Vector3 position, Vector2 bounceDirection) {
        GameObject itemPrefab = Resources.Load<GameObject>("Items/XP");

        // Spawn the Item
        GameObject item = Object.Instantiate(
            itemPrefab,
            position,
            Quaternion.identity
        );

        // Initialise the Item
        item.GetComponent<XP>().Spawn(bounceDirection);

        // Return the spawned Item
        return item;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Tiles
{
    // Tile Sprites
    private static Vector2Int spriteSize = new Vector2Int(16, 16);
    private static Sprite[] grass;
    private static Sprite[] obstacles;
    private static Sprite[] tree;

    // Prefabs
    private static GameObject tilePrefab = null;
    private static GameObject obstaclePrefab = null;

    static Tiles() {
        Debug.Log("Tiles has awakened!");

        tilePrefab = Resources.Load<GameObject>("Tiles/Tile");
        obstaclePrefab = Resources.Load<GameObject>("Tiles/Obstacle");
    }

    /// <summary>
    /// Creates the Tile Sprites from a spritesheet at runtime.
    /// </summary>
    public static void LoadTiles() {
        grass = ImageLoader.CreateAllSprites("Grass", "Tiles", spriteSize);
        obstacles = ImageLoader.CreateAllSprites("Obstacles", "Tiles", spriteSize);
    }



    /// <summary>Array of the Grass tile sprites.</summary>
    public static Sprite[] Grass => grass;
    /// <summary>Array of the Obstacle tile sprites.</summary>
    public static Sprite[] Obstacles => obstacles;

    // GameObject Prefabs
    public static GameObject TilePrefab => tilePrefab;
    public static GameObject ObstaclePrefab => obstaclePrefab;

    // Colours
    public static Color ObstacleColour => Color.white;
}

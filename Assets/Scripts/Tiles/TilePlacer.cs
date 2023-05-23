using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simple class that randomly spawns environment tiles in game world.
/// </summary>
public class TilePlacer : MonoBehaviour
{
    [Header("TILE PLACER")]
    [Header("Dimensions")]
    [SerializeField][Tooltip("Size of each square spawning sector")]
    private int sectorSize = 8;
    [SerializeField][Tooltip("How many sectors to spawn in each direction")]
    private int radius = 4;

    [Header("Tiles")]
    [SerializeField][Tooltip("Tile GameObject to spawn")]
    private GameObject tilePrefab = null;
    [SerializeField][Tooltip("Color overlay to apply to all tiles")]
    private Color tileColour = Color.white;
    

    private void Awake() {
        if (tilePrefab == null)
            tilePrefab = Resources.Load<GameObject>("Tiles/Tile");
        
        Tiles.LoadTiles();
    }

    void Start() {
        // Center the tiles in the middle of a sector, not an edge
        transform.position = new Vector2(
            -sectorSize / 2,
            sectorSize / 2
        );

        // spawn the tiles!
        SectorBased2();
    }

    private void SectorBasedLocal() {
        for (int x = -radius; x < radius; x++) {
            for (int y = -radius; y < radius; y++) {
                bool addSector = Random.Range(1, 7) >= 4;

                if (!addSector) continue;

                GameObject sectorObj = new GameObject($"Sector ({x}, {y})");
                Transform sector = sectorObj.transform;
                sector.parent = transform;
                sector.position = new Vector2(x * sectorSize, y * sectorSize);

                Color sectorColour = Random.ColorHSV();

                Sprite sectorTile = Tiles.Grass[Random.Range(0, Tiles.Grass.Length)];

                for (int sx = 0; sx < sectorSize; sx++) {
                    for (int sy = 0; sy < sectorSize; sy++) {
                        bool addTile = Random.Range(1, 7) >= 5;
                        if (!addTile)   continue;

                        // Vector2 position = new Vector2( (x * sectorSize) + sx, (y * sectorSize) + sy );
                        Vector3 position = new Vector3(sx, sy) + sector.position;

                        GameObject tile = Instantiate(tilePrefab, position, Quaternion.identity, sector);
                        tile.GetComponent<SpriteRenderer>().sprite = sectorTile;
                        tile.GetComponent<SpriteRenderer>().color = tileColour;
                        // tile.GetComponent<SpriteRenderer>().color = sectorColour;
                    }
                }
            }
        } 
    }

    /// <summary>
    /// Spawn Tile Sectors with given tile properties, who themselves will spawn the tiles into the game world.
    /// </summary>
    private void SectorBased2() {
        for (int x = -radius; x < radius; x++) {
            for (int y = -radius; y < radius; y++) {
                int spawnChance = Random.Range(0, 7) >= 5 ? 5 : 6;

                Sprite sectorTile = Tiles.Grass[Random.Range(0, Tiles.Grass.Length)];

                GameObject sectorObj = new GameObject($"Sector ({x}, {y})");
                sectorObj.transform.parent = transform;
                sectorObj.transform.position = new Vector2(x * sectorSize, y * sectorSize) + (Vector2)(transform.position);

                TileSector sector = sectorObj.AddComponent<TileSector>();
                sector.Init(Tiles.TilePrefab, sectorTile, tileColour, sectorSize, spawnChance);
            }
        }
    }

    
}

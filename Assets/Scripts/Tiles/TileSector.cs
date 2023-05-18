using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Helper class to concurrently spawn tiles randomly inside a grid.
/// </summary>
public class TileSector : MonoBehaviour
{
    private GameObject tilePrefab;
    private Sprite tileSprite;
    private Color tileColour;
    private int sectorSize;
    private int spawnChance;

    /// <summary>
    /// Initialise the Sector properties and begin spawning tiles.
    /// </summary>
    /// <param name="tilePrefab">GameObject of tile to spawn</param>
    /// <param name="tileSprite">Sprite to assign to tile</param>
    /// <param name="tileColour">Colour overlay to apply to tile sprite</param>
    /// <param name="sectorSize">How big this sector is</param>
    /// <param name="spawnChance">Dice roll required to spawn a tile (1 = 100% spawn chance, 6 = 16%)</param>
    public void Init(GameObject tilePrefab, Sprite tileSprite, Color tileColour, int sectorSize, int spawnChance=5) {
        // set properties
        this.tilePrefab = tilePrefab;
        this.tileSprite = tileSprite;
        this.tileColour = tileColour;
        this.sectorSize = sectorSize;
        this.spawnChance = Mathf.Clamp(spawnChance, 0, 6);

        // start placing tiles
        StartCoroutine(PlaceTiles());
    }

    // Executed as a coroutine so several instances can run at the same time
    private IEnumerator PlaceTiles() {
        for (int x = 0; x < sectorSize; x++) {
            for (int y = 0; y < sectorSize; y++) {
                bool addTile = Random.Range(0, 7) >= spawnChance;

                if (addTile) {
                    Vector2 position = new Vector2(x, y) + (Vector2)transform.position;
                    GameObject tile = Instantiate(tilePrefab, position, Quaternion.identity, transform);
                    tile.GetComponent<SpriteRenderer>().sprite = tileSprite;
                    tile.GetComponent<SpriteRenderer>().color = tileColour;
                }
            }
        }

        yield return null;
    }    
}

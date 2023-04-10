using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilePlacer : MonoBehaviour
{
    [SerializeField]
    private GameObject tilePrefab = null;

    [SerializeField]
    private int radius = 50;

    // Start is called before the first frame update
    void Start()
    {
        for (int x = -radius; x < radius; x++) {
            for (int y = -radius; y < radius; y++) {
                bool addTile = Random.Range(0, 11) >= 8;

                if (addTile) {
                    Instantiate(tilePrefab, new Vector2(x, y), Quaternion.identity, transform);
                }
            }
        }
    }

    
}

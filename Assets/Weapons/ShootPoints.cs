using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class holds the transform positions for the spawn points of projectiles in all 4 directions.
/// </summary>
public class ShootPoints : MonoBehaviour
{
    [SerializeField]
    private Transform up = null;
    [SerializeField]
    private Transform down = null;
    [SerializeField]
    private Transform right = null;
    [SerializeField]
    private Transform left = null;
    
    // Getters
    public Transform Up => up;
    public Transform Down => down;
    public Transform Right => right;
    public Transform Left => left;

    /// <summary>
    /// Gets the approrite shoot point for the given shooting direction.
    /// </summary>
    /// <param name="direction">Enum Direction the player is aiming at.</param>
    /// <returns>Vector2 spawn point</returns>
    public Vector2 Direction2ShootPoint(Direction direction) {
        switch (direction)
        {
            case Direction.Up:
                return up.position;
            case Direction.Down:
                return down.position;
            case Direction.Right:
                return right.position;
            case Direction.Left:
                return left.position;

            default:
                return transform.position;
        }
    }
}

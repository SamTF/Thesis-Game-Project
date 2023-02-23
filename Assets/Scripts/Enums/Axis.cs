using UnityEngine;

/// <summary>
/// Helper class to choose a certain axis vector.
/// </summary>
public class Axis
{
    private Vector3 _value;

    public Axis(Vector3 value) {
        _value = value;
    }

    public Vector3 value => _value;


    // Creating Axis objects
    public static Axis X = new Axis(Vector3.right);
    public static Axis Y = new Axis(Vector3.up);
    public static Axis Z = new Axis(Vector3.forward);
}

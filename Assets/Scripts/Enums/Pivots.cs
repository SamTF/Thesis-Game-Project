using UnityEngine;

public class Pivot {
    private Vector2 _value;

    public Pivot(Vector2 value) {
        _value = value;
    }

    public Vector2 value => _value;


    public static Pivot Center = new Pivot(new Vector2(0.5f, 0.5f));
    public static Pivot BottomCenter = new Pivot(new Vector2(0.5f, 0f));
}

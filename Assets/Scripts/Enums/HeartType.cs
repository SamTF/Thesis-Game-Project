using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contains the fill value and sprite image for this heart type.
/// </summary>
[System.Serializable]
public class HeartType
{
    private HeartFill _value = HeartFill.Full;
    private Sprite _sprite = null;

    public HeartType(HeartFill heartFill, Sprite sprite) {
        _value = heartFill;
        _sprite = sprite;
    }

    /// <summary>The Fill value of this Heart (Full, Half, or Empty)</summary>
    public HeartFill value => _value;
    /// <summary>The Icon Sprite for this heart type.</summary>
    public Sprite sprite => _sprite;

}

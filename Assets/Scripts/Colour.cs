using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colour
{
    public Color colour;
    public int value;
    public string hexColour => ColorUtility.ToHtmlStringRGB(colour);
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Barebones class to allow to extend UI Buttons with custom attributes.
/// </summary>
public class MyButton : Button
{
    public new class UxmlFactory : UxmlFactory<MyButton, Button.UxmlTraits>{}

    public MyButton() {

    }
}

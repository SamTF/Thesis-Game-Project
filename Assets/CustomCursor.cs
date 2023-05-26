using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CustomCursor
{
    // Cursor images
    private static Texture2D cursorDefault = null;
    private static Texture2D cursorPointer = null;
    private static Texture2D cursorPencil = null;
    private static Texture2D cursorBrush = null;

    private static Texture2D activeCursor = null;
    private static Vector2 activeOffset;

    // cursor style
    public enum Style {
        auto, pointer, pencil, brush
    };

    static CustomCursor() {
        cursorDefault = Resources.Load<Texture2D>("UI/Cursors/cursor_default");
        cursorPointer = Resources.Load<Texture2D>("UI/Cursors/cursor_pointer");
        cursorPencil = Resources.Load<Texture2D>("UI/Cursors/cursor_pencil");
        cursorBrush = Resources.Load<Texture2D>("UI/Cursors/cursor_brush");

        SetCursor(Style.auto);
    }

    /// <summary>
    /// Change current cursor style.
    /// </summary>
    /// <param name="style">New style</param>
    public static void SetCursor(Style style) {
        Texture2D cursor = null;
        Vector2 offset = Vector2.zero;

        switch (style)
        {
            case Style.pointer:
                cursor = cursorPointer;
                break;

            case Style.pencil:
                cursor = cursorPencil;
                offset = new Vector2(0, cursorPencil.height);
                break;

            case Style.brush:
                cursor = cursorBrush;
                offset = new Vector2(0, cursorBrush.height);
                break;

            case Style.auto:
            default:
                cursor = cursorDefault;
                break;
        }

        Cursor.SetCursor(cursor, offset, CursorMode.Auto);
        activeCursor = cursor;
        activeOffset = offset;
    }

    /// <summary>
    /// Change the accent colour of the current cursor!!
    /// </summary>
    /// <param name="newColour">Colour to set all non-alpha non-black pixels to.</param>
    public static void CursorTint(Color newColour) {
        Debug.Log("CURSOR TINT!!");

        // create empty texture of correct length
        Texture2D tex = new Texture2D(activeCursor.width, activeCursor.height, TextureFormat.ARGB32, false);

        // re-colour any non-alpha non-black pixel to new colour
        for (int x = 0; x < activeCursor.width; x++) {
            for (int y = 0; y < activeCursor.height; y++) {
                Color pixelColour = activeCursor.GetPixel(x, y);

                if (pixelColour.a < 1 || pixelColour == Color.black) {
                    tex.SetPixel(x, y, pixelColour);
                    continue;
                }
                    
                tex.SetPixel(x, y, newColour);
            }
        }

        // Apply changes to texture
        tex.Apply();

        // set cursor
        Cursor.SetCursor(tex, activeOffset, CursorMode.Auto);
    }

    /// <summary>Whether the Cursor is rendered or not.</summary>
    public static bool visible {
        get { return Cursor.visible; }
        set { Cursor.visible = value; }
    }


}

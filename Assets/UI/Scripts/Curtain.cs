using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Curtain : MonoBehaviour
{
    private HSVColour colour;
    private SpriteRenderer spriteRenderer = null;

    
    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    void Start() {
        // set position
        // Set position
        Vector2 screenSize = new Vector2(Screen.width, Screen.height);
        Vector2 screenCenter = Camera.main.ScreenToWorldPoint(screenSize/2);
        transform.position = screenCenter;

        // Set random Colour
        Color randomColor = Random.ColorHSV(0f, 1f, 0.5f, 1f, 0.5f, 1f);
        colour = new HSVColour(randomColor);
        spriteRenderer.color = colour.Colour;
        
        // start hue cycle animation
        StartCoroutine(ColourAnimation(10f));
    }

    /// <summary>
    /// Smoothly transitions between all Hues while keeping the same Saturation and Brightness
    /// </summary>
    /// <param name="speed">How quickly to cycle between hues</param>
    private IEnumerator ColourAnimation(float speed = 10f) {
        int startHue = colour.Hue;

        while (true) {
            int newHue = (int)Mathf.PingPong(Time.unscaledTime * speed, 360-startHue) + startHue;
            UpdateHue(newHue);

            yield return null;
        }
    }

    private void UpdateHue(int hue) {
        colour.Hue = hue;
        spriteRenderer.color = colour.Colour;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class CameraMan : MonoBehaviour
{
    [Header("Camera Controller")]
    // Player AKA Camera Target
    [SerializeField][Tooltip("The GameObject that the camera will follow")]
    private Player player = null;

    [SerializeField][Range(0f, 1f)][Tooltip("How smoothly to follow the player. Higher values = smoother more delay. Lower values = faster sharper follow.")]
    private float followDelay = 0.75f;
    private Vector3 velocity = Vector3.zero;

    // Components
    [Header("Components")]
    [SerializeField][Tooltip("The Camera component")]
    private Camera myCamera = null;
    [SerializeField][Tooltip("The Pixel Perfect Camera component")]
    private PixelPerfectCamera pixelPerfect = null;


    void Start() {
        // Getting the Player object in the scene
        if (player == null) player = Player.instance;

        // get components
        if (myCamera == null)       myCamera = GetComponent<Camera>();
        if (pixelPerfect == null)   pixelPerfect = GetComponent<PixelPerfectCamera>();

        // set properties
        myCamera.backgroundColor = Palette.BackgroundColour.Colour;

        if (PlayerPrefs.HasKey("pixelPerfect"))
            pixelPerfect.enabled = PlayerPrefs.GetInt("pixelPerfect") == 1;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerPos = GameManager.instance.PlayerPosition;
        Vector3 currentPos = transform.position;
        Vector3 offset = Vector3.zero;

        // Camera lookahead in the direction of shooting
        if (player.Input.IsAttacking) {
            offset = player.Input.Attack * 2.5f;
        }

        // Position for the camera to smoothly move towards
        Vector3 newCameraPos = new Vector3(playerPos.x, playerPos.y, -10f) + offset;
        
        // Smooth follow  
        transform.position = Vector3.SmoothDamp(currentPos, newCameraPos, ref velocity, followDelay);
    }


    /// <summary>
    /// This function rounds a value to a multiple of pixel screen value based on the pixels per unit
    /// </summary>
    /// <param name="value">The value to round</param>
    /// <param name="multipleOf">1 / Pixels per unit</param>
    /// <returns>A rounded float</returns>
    /// From: https://izeeware.com/2020/07/08/unity-smooth-camera-moves-with-pixel-perfect/
    private float RoundToMultiple(float value, float multipleOf = 1/16)
    {
        return (int)((value / multipleOf) + 0.5f) * multipleOf;
    }

    /// <summary>
    /// Update Camera settings
    /// </summary>
    private void UpdateSettings() {
        if (PlayerPrefs.HasKey("pixelPerfect") && pixelPerfect != null)
            pixelPerfect.enabled = PlayerPrefs.GetInt("pixelPerfect") == 1;
    }


    // Stop following if the Player is dead
    private void OnEnable() {
        Health.onPlayerDeath += OnPlayerDeath;
        OptionsMenu.onSettingChanged += UpdateSettings;
    }
    private void OnDisable() {
        Health.onPlayerDeath -= OnPlayerDeath;
        OptionsMenu.onSettingChanged -= UpdateSettings;
    }

    /// <summary>
    /// Disable camera movement after player dies.
    /// </summary>
    private void OnPlayerDeath() {
        this.enabled = false;
    }
}

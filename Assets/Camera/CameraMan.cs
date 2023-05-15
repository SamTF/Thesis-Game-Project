using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMan : MonoBehaviour
{
    [Header("Camera Controller")]
    // Player AKA Camera Target
    [SerializeField][Tooltip("The GameObject that the camera will follow")]
    private Player player = null;

    [SerializeField][Range(0f, 1f)][Tooltip("How smoothly to follow the player. Higher values = smoother more delay. Lower values = faster sharper follow.")]
    private float followDelay = 0.75f;
    private Vector3 velocity = Vector3.zero;


    void Start() {
        // Getting the Player object in the scene
        if (player == null) player = Player.instance;
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


    // Stop following if the Player is dead
    private void OnEnable() {
        Health.onPlayerDeath += OnPlayerDeath;
    }
    private void OnDisable() {
        Health.onPlayerDeath -= OnPlayerDeath;
    }
    private void OnPlayerDeath() {
        this.enabled = false;
    }
}

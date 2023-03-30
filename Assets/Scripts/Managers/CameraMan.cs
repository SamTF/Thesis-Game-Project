using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMan : MonoBehaviour
{
    [Header("Camera Controller")]
    // Player AKA Camera Target
    [SerializeField][Tooltip("The GameObject that the camera will follow")]
    private Player player = null;
    private Vector3 playerPos;

    [SerializeField][Range(0f, 1f)]
    private float followDelay = 0.75f;

    [SerializeField][Tooltip("Whether the game is in Pixel Perfect mode or not")]
    private bool pixelPerfect = false;

    private Vector2 boundingBox = new Vector2(2,2);
    private bool followingPlayer = false;
    private Vector3 velocity = Vector3.zero;

    private const float pixelUnit = 16f;

    // Start is called before the first frame update
    void Start()
    {
        if (player == null) player = GameManager.instance.Player;

        if (pixelPerfect) {
            StartCoroutine(PixelPerfectFollow());
        }
    }

    // Update is called once per frame
    void Update()
    {
        // if (pixelPerfect) return;

        playerPos = player.transform.position;
        Vector3 currentPos = transform.position;
        Vector3 offset = Vector3.zero;

        // Camera lookahead in the direction of shooting
        if (player.Input.IsAttacking) {
            offset = player.Input.Attack * 2.5f;
        }

        // Position for the camera to smoothly move towards
        Vector3 newCameraPos = new Vector3(playerPos.x, playerPos.y, -10f) + offset;

        float smoothTime = followDelay;
        if (pixelPerfect) smoothTime = RoundToMultiple(followDelay, 1 / pixelUnit);
        
        transform.position = Vector3.SmoothDamp(currentPos, newCameraPos, ref velocity, smoothTime);
        
    }

    private bool PlayerLeftBox() {
        return Mathf.Abs(playerPos.x - transform.position.x) >= boundingBox.x
            ||  Mathf.Abs(playerPos.y - transform.position.y) >= boundingBox.y;
    }


    /// <summary>
    /// This function rounds a value to a multiple of pixel screen value based on the pixels per unit
    /// </summary>
    /// <param name="value">The value to round</param>
    /// <param name="multipleOf">1 / Pixels per unit</param>
    /// <returns>A rounded float</returns>
    /// From: https://izeeware.com/2020/07/08/unity-smooth-camera-moves-with-pixel-perfect/
    private float RoundToMultiple(float value, float multipleOf = 1/pixelUnit)
    {
        return (int)((value / multipleOf) + 0.5f) * multipleOf;
    }


    private IEnumerator PixelPerfectFollow() {
        while (true)
        {
            playerPos = player.transform.position;
            Vector3 currentPos = transform.position;
            Vector3 offset = Vector3.zero;

            // Camera lookahead in the direction of shooting
            if (player.Input.IsAttacking) {
                offset = player.Input.Attack * 2.5f;
            }

            // Position for the camera to smoothly move towards
            Vector3 newCameraPos = new Vector3(playerPos.x, playerPos.y, -10f) + offset;
            // transform.position = Vector3.SmoothDamp(currentPos, newCameraPos, ref velocity, followDelay);
            transform.position = Vector3.MoveTowards(currentPos, newCameraPos, 1/16f);

            yield return new WaitForFixedUpdate();
        }
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMan : MonoBehaviour
{
    // Player AKA Camera Target
    [SerializeField][Tooltip("The GameObject that the camera will follow")]
    private Player player = null;
    private Vector3 playerPos;

    [SerializeField][Range(0f, 1f)]
    private float followDelay = 0.75f;

    private Vector2 boundingBox = new Vector2(2,2);
    private bool followingPlayer = false;
    private Vector3 velocity = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        if (player == null) player = GameManager.instance.Player;
    }

    // Update is called once per frame
    void Update()
    {
        playerPos = player.transform.position;
        Vector3 currentPos = transform.position;
        Vector3 offset = Vector3.zero;

        // Camera lookahead in the direction of shooting
        if (player.Input.IsAttacking) {
            offset = player.Input.Attack * 2.5f;
        }

        Vector3 newCameraPos = new Vector3(playerPos.x, playerPos.y, -10f) + offset;
        
        transform.position = Vector3.SmoothDamp(currentPos, newCameraPos, ref velocity, followDelay);
        // transform.position = new Vector3(transform.position.x, transform.position.y, -10f);

        
        
    }

    private bool PlayerLeftBox() {
        return Mathf.Abs(playerPos.x - transform.position.x) >= boundingBox.x
            ||  Mathf.Abs(playerPos.y - transform.position.y) >= boundingBox.y;
    }
}

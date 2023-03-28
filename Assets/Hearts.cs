using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Container for the Heart UI icons. Displays the Player's current health in TBOI-style hearts.
/// </summary>
public class Hearts : MonoBehaviour
{
    // Constants
    private const int iconSize = 8;
    // private const int heartTypes = 3;
    private string iconName = "Hearts";
    private string fileType = ".png";

    // Textures & Sprites
    private Texture2D heartTex = null;
    private Sprite heartFull = null;
    private Sprite heartHalf = null;
    private Sprite heartEmpty = null;
    [SerializeField]
    private Heart[] heartList;

    // Components
    [Header("UI Hearts Container")]
    [SerializeField]
    private GameObject heartPrefab;
    private Health playerHealth;

    // Static Objects
    public static HeartType Full;
    public static HeartType Half;
    public static HeartType Empty;
    public static HeartType[] heartTypes;

    
    private void Awake() {
        // Setting the name of the file required to mod the image
        string name = iconName != "" ? iconName : this.name;
        iconName = $"{name}.{fileType}";

        // Checking if the player created a custom heart icon
        if (ModManager.ModExists(iconName)) {
            Debug.Log("[HEART]>>> Custom heart found");
            heartTex = ImageLoader.LoadTextureFromFile(iconName);
        }
        // If not, use the default icon
        else {
            Debug.Log("[HEART]>>> Loading default Heart...");
            heartTex = Resources.Load("UI/Hearts") as Texture2D;  
        }

        // Create the 3 sprites from the single texture and save them in memory
        CreateAllSprites(Pivot.Center);

        // Initialising HeartType objects
        Full = new HeartType(HeartFill.Full, heartFull);
        Half = new HeartType(HeartFill.Half, heartHalf);
        Empty = new HeartType(HeartFill.Empty, heartEmpty);
        heartTypes = new HeartType[] { Empty, Half, Full };
    }

    private void Start() {
        playerHealth = GameManager.instance.Player.Health;

        DrawHearts();
    }

    // Subscribing and unsubscribing to events
    private void OnEnable() {
        Health.onPlayerDamaged += DrawHearts;
    }
    private void OnDisable() {
        Health.onPlayerDamaged -= DrawHearts;
    }

    /// <summary>
    /// Create all 3 Heart sprites from the texture image.
    /// </summary>
    /// <param name="pivot">Pivot for the heart sprites.</param>
    private void CreateAllSprites(Pivot pivot) {
        heartFull = CreateSprite(heartTex, pivot, 0 * iconSize);
        heartHalf = CreateSprite(heartTex, pivot, 1 * iconSize);
        heartEmpty = CreateSprite(heartTex, pivot, 2 * iconSize);
    }


    private Sprite CreateSprite(Texture2D tex, Pivot pivot, int start=0) {
        Sprite newSprite = Sprite.Create(
            tex,
            new Rect(start, 0, iconSize, iconSize),
            pivot.value,
            16
        );

        return newSprite;
    }

    /// <summary>
    /// Clears any child objects and resets the Heart List.
    /// </summary>
    private void ClearHearts() {
        foreach (Transform t in transform) {
            Destroy(t.gameObject);
        }

        heartList = new Heart[playerHealth.MaxHearts];
    }

    /// <summary>
    /// Instantiates a new Heart UI icon and returns it.
    /// </summary>
    /// <returns>The new Heart object.</returns>
    private Heart CreateHeart(HeartType status) {
        GameObject heartObject = Instantiate(heartPrefab, transform.position, Quaternion.identity, transform);
        Heart newHeart = heartObject.GetComponent<Heart>();
        newHeart.status = status;

        return newHeart;
    }

    /// <summary>
    /// Creates all hearts and sets their respective stauses to match the Player's health
    /// </summary>
    private void DrawHearts() {
        ClearHearts();

        for (int i = 0; i < heartList.Length; i++) {
            int t = heartTypes.Length - 1;
            int statusRemainder = Mathf.Clamp(playerHealth.HP - (i*t), 0, t);
            HeartType status = heartTypes[statusRemainder];

            Heart newHeart = CreateHeart(status);
            heartList[i] = newHeart;
        }
    }
}

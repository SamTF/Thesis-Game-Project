using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class StatsUI : MonoBehaviour
{
    [Header("STATS UI")]
    [SerializeField][Tooltip("Name of the root Visual Element containing the Stats UI stuff")]
    private string mainContainerID = "StatsContainer";
    [SerializeField]
    private Sprite testIcon = null;

    // Elements
    private VisualElement mainContainer = null;
    private StatsItemUI[] statItems = null;

    private Dictionary<Color, StatsItemUI> colour2item = new Dictionary<Color, StatsItemUI>();

    // Icons
    /// <summary>The Icons for each stats as Sprite objects</summary>
    private Sprite[] iconSprites = new Sprite[6];
    /// <summary>The dimensions of the XP Icon sprite in px.</summary>
    private Vector2Int iconSize = new Vector2Int(16, 16);
    /// <summary>Name of the Stat Icons spritesheet file.</summary>
    private string iconName = "StatIcons";
    /// <summary>File extension of the image files.</summary>
    private string fileType = "png";


    private void Start() {
        // Get elements
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        mainContainer = root.Q<VisualElement>(mainContainerID);

        // Create Icon sprites
        iconSprites = CreateAllSprites(iconName, iconSize, iconSprites.Length);

        // Instantiate Stat Items
        InitItems();
    }

    /// <summary>
    /// Create all Sprites from a spritesheet file of a given name, and return them as an array.
    /// </summary>
    /// <param name="textureName">Name of the image file (must be identical in both Resources and CUSTOM folder)</param>
    /// <param name="iconSize">Size of the icons inside the spritesheet.</param>
    /// <param name="numOfIcons">How many icons to create.</param>
    /// <returns>Array of sprites.</returns>
    private Sprite[] CreateAllSprites(string textureName, Vector2Int iconSize, int numOfIcons) {
        Texture2D tex = null;

        // Checking if the player created a custom texture
        if (ModManager.ModExists($"{textureName}.{fileType}")) {
            Debug.Log("[STAT ICONS]>>> Custom texture found");
            tex = ImageLoader.LoadTextureFromFile($"{textureName}.{fileType}");
        }
        // If not, use the default texture
        else {
            Debug.Log("[STAT ICONS]>>> Loading default texture...");
            tex = Resources.Load($"UI/{textureName}") as Texture2D;  
        }

        // Create a sprite for each element in the array, and assign to that element
        Sprite[] spriteArray = new Sprite[numOfIcons];
        for (int i = 0; i < numOfIcons; i++) {
            Sprite s = ImageLoader.CreateSprite(tex, Pivot.Center, iconSize.x * i, iconSize);
            spriteArray[i] = s;
        }

        // Return the sprites
        return spriteArray;
    }

    /// <summary>
    /// Initalise the Stat Item visual elements
    /// </summary>
    private void InitItems() {
        // clear
        mainContainer.Clear();

        // fetch stats (i really need to make the player a singleton)
        Stat[] stats = GameManager.instance.Player.Stats.StatsArray;

        statItems = new StatsItemUI[stats.Length];

        // creating the stat item elements
        int i = 0;
        foreach (Stat stat in stats) {
            // create element and add it to container
            // StatsItemUI item = new StatsItemUI(stat.Colour, stat.Value, iconSprites[i], stat.Name);
            StatsItemUI item = new StatsItemUI(stat.Colour, 0, iconSprites[i], stat.Name);
            mainContainer.Add(item);

            // add element reference to array and dictionary
            statItems[i] = item;
            colour2item.Add(stat.Colour, statItems[i]);
            i++;
        }
    }


    /// <summary>
    /// Update the Label text of a stat item.
    /// </summary>
    /// <param name="colour">Colour of the stat item to be updated.</param>
    /// <param name="increment">Whether to increment or decrement the value.</param>
    public void UpdateStatValue(Color colour, bool increment) {
        // error handling: check if the given colour exists in the dict
        if (!colour2item.ContainsKey(colour))
            return;

        StatsItemUI item = colour2item[colour];

        if (increment)  item.Value++;
        else            item.Value--;
    }

    /// <summary>
    /// Updates ALL stats from scratch by re-fetching the value from the Stats.
    /// </summary>
    public void RefreshAllStats() {
        foreach (StatsItemUI item in statItems) {
            Stat stat = GameManager.instance.Player.Stats.Colour2Stat[item.Colour];
            item.Value = stat.Value;
        }
    }
}

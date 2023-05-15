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
        // iconSprites = CreateAllSprites(iconName, iconSize, iconSprites.Length);
        iconSprites = ImageLoader.CreateAllSprites(iconName, "UI", iconSprites.Length, iconSize);

        // Instantiate Stat Items
        InitItems();
    }


    /// <summary>
    /// Initalise the Stat Item visual elements
    /// </summary>
    private void InitItems() {
        // clear
        mainContainer.Clear();

        // fetch stats (i really need to make the player a singleton)
        Stat[] stats = Player.instance.Stats.StatsArray;

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
            Stat stat = Player.instance.Stats.Colour2Stat[item.Colour];
            item.Value = stat.Value;
        }
    }
}

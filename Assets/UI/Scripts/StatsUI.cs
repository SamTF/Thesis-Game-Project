using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System.Linq;

public class StatsUI : MonoBehaviour
{
    [Header("STATS UI")]
    [SerializeField][Tooltip("Name of the root Visual Element containing the Stats UI stuff")]
    private string mainContainerID = "StatsContainer";
    [SerializeField][Tooltip("Whether to show all Stats (True) or only Unlocked Stats (False)")]
    private bool showAllStats = true;
    [SerializeField][Tooltip("Compact version of the Stat Item that only display text and needs only 6px height")]
    private bool compactMode = false;


    private VisualElement mainContainer = null;
    private StatsItemUI[] statItems = null;
    private Dictionary<Color, StatsItemUI> colour2item = new Dictionary<Color, StatsItemUI>();
    private StatsItemCompactUI[] compactItems = null;


    // Subscribing to events
    private void OnEnable() {
        Player.onSpriteUpdated += UpdateCompactStats;
    }
    private void OnDisable() {
        Player.onSpriteUpdated -= UpdateCompactStats;
    }


    private void Start() {
        // Get elements
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        mainContainer = root.Q<VisualElement>(mainContainerID);

        // Instantiate Stat Items
        InitItems();
    }


    /// <summary>
    /// Initalise the Stat Item visual elements
    /// </summary>
    private void InitItems() {
        // clear
        mainContainer.Clear();

        Stat[] stats;
        
        // fetch stats from Player if all attributes must be shown
        if (showAllStats)
            stats = Player.instance.Stats.StatsArray;
        // fetch stats from level system if only unlocked attributes should be shown
        else
            stats = LevelSystem.instance.UnlockedStats;

        // Creating compact items in compact mode
        if (compactMode) {
            InitCompactItems(stats);
            return;
        }
        
        // creating the stat item elements
        statItems = new StatsItemUI[stats.Length];

        int i = 0;
        foreach (Stat stat in stats) {
            // create element and add it to container
            StatsItemUI item = new StatsItemUI(stat.Colour, stat.Value, stat.Icon, stat.Name);
            mainContainer.Add(item);

            // add element reference to array and dictionary
            statItems[i] = item;
            colour2item.Add(stat.Colour, statItems[i]);
            i++;
        }
    }

    /// <summary>
    /// Creates Compact Stat Items and adds them to the UI.
    /// </summary>
    /// <param name="stats">List of stats to display</param>
    private void InitCompactItems(Stat[] stats) {
        compactItems = new StatsItemCompactUI[stats.Length];

        int i = 0;
        foreach (Stat stat in stats) {
            StatsItemCompactUI compactItem = new StatsItemCompactUI(stat);
                mainContainer.Add(compactItem);
                compactItems[i] = compactItem;
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
    public void RefreshAllStats(Texture2D texture = null) {
        // Fetches stat valuesdirectly from the texture
        if (texture) {
            Colour[] colourValues = ImageAnalyser.Analyse(texture);

            foreach (StatsItemUI item in statItems) {
                try {
                    item.Value = colourValues.First(x => x.colour == item.Colour).value;
                } catch (System.Exception) {
                    item.Value = 0;
                }
            }

            return;
        }

        // Fetches stat values from the Player Stats component
        foreach (StatsItemUI item in statItems) {
            Stat stat = Player.instance.Stats.Colour2Stat[item.Colour];
            item.Value = stat.Value;
        }
    }

    private void UpdateCompactStats() {
        foreach (StatsItemCompactUI item in compactItems) {
            item.UpdateValue();
        }
    }
}

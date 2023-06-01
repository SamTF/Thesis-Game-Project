using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This Class is responsible for spawning enemies into the game world in the correct order
/// </summary>
public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    /// <summary>
    /// Helper Class to associate Enemies with a Character to create spawn strings.
    /// </summary>
    public class SpawnCode {
        [SerializeField][Tooltip("GameObject of this enemy type.")]
        private GameObject prefab = null;
        [SerializeField][Tooltip("Character representing this enemy in the spawn wave string.")]
        private char code;
        [SerializeField][Tooltip("Minimum amount to spawn at once.")]
        private int minAmount = 1;
        [SerializeField][Tooltip("Maximum amount to spawn at once.")]
        private int maxAmount = 1;

        public SpawnCode(GameObject prefab, char code, int minAmount = 1, int maxAmount = 1) {
            this.prefab = prefab;
            this.code = code;
            this.minAmount = minAmount;
            this.maxAmount = maxAmount;
        }

        public GameObject Prefab => prefab;
        public char Code => code;
        public int MinAmount => minAmount;
        public int MaxAmount => maxAmount;
    }


    [Header("ENEMY SPAWNER")]
    [SerializeField][TextArea(5,20)]
    private string spawnOrder = "";

    [Header("Enemies")]
    [SerializeField]
    private SpawnCode[] enemies;

    [SerializeField]
    private SpawnCode chaser = null;
    [SerializeField]
    private SpawnCode shooter = null;
    [SerializeField]
    private SpawnCode ghost = null;
    [SerializeField]
    private SpawnCode bouncer = null;

    [SerializeField][Tooltip("How far away enemies spawn from the player")][Range(10,20)]
    private int spawnDistance = 15;

    [SerializeField][Tooltip("How long to wait when skipping (in seconds)")][Range(1, 10)]
    private int skipTime = 10;

    private string[] spawnWaves;
    private Timer timer = null;
    private Dictionary<char, SpawnCode> charToEnemy = new Dictionary<char, SpawnCode>();

    // Events
    public static event System.Action onAllEnemiesDefeated;
    private bool allEnemiesSpawned = false;
    private bool allEnemiesDefeated = false;


    private void Start() {
        // initialise dictionary
        foreach (SpawnCode enemy in enemies) {
            charToEnemy.Add(enemy.Code, enemy);
        }

        // start spawning enemies
        StartCoroutine(SpawnWave());

        // spawnWaves = spawnOrder.Split('\n');
        // foreach (string item in spawnWaves) {
        //     Debug.Log(item);
        // }
    }

    /// <summary>
    /// Loop thru every character in the SpawnWave string and spawn the appropriate enemy or wait.
    /// </summary>
    private IEnumerator SpawnWave() {
        // Spawn enemies
        foreach (char c in spawnOrder) {
            switch (c) {
                case char when c == chaser.Code:
                    Spawn(chaser.Prefab);
                    break;

                case char when c == shooter.Code:
                    Spawn(shooter.Prefab);
                    break;
                
                case char when c == ghost.Code:
                    Spawn(ghost.Prefab);
                    break;

                case char when c == bouncer.Code:
                    Spawn(bouncer.Prefab);
                    break;
                
                default:
                    Debug.Log("skipping...");
                    yield return new WaitForSeconds(skipTime);
                    break;
            }
        }

        Debug.Log("FINISHED SPAWNING!!");
        Debug.Log("FINISHED SPAWNING!!");
        Debug.Log("FINISHED SPAWNING!!");

        // When spawning has finished
        allEnemiesSpawned = true;
        StartCoroutine(CheckForWin());

        yield return null;
    }

    /// <summary>
    /// Randomly generated spawn waves
    /// </summary>
    /// <returns></returns>
    private IEnumerator RandomSpawning() {
        string enemyString = "ccssgb";

        // continue spawning enemies while the player is still alive
        while (Player.instance.Status.IsAlive) {
            // rng should an enemy be spawned or not
            if (Random.Range(0, 7) >= 4)
                continue;

            // fetch random enemy from the enemy string
            int i = Random.Range(0, enemyString.Length-1);
            char enemyChar = enemyString[i];

            // error handling
            if (charToEnemy.ContainsKey(enemyChar)) {
                SpawnCode enemy = charToEnemy[enemyChar];
                Spawn(enemy);
            // default enemy if the given character doesn't exist
            } else {
                Spawn(chaser.Prefab);
            }
            
            yield return new WaitForSeconds(skipTime/2f);
        }
    }

    /// <summary>
    /// Spawns a single enemy from a prefab.
    /// </summary>
    /// <param name="prefab">Enemy prefab to spawn.</param>
    private void Spawn(GameObject prefab) {
        Debug.Log($"[ENEMY SPAWNER] >>> Spawning {prefab.name}");

        Vector2 position = (Random.insideUnitCircle.normalized * spawnDistance) + Player.instance.Position;

        GameObject enemyObject = Instantiate(prefab, position, Quaternion.identity, transform);
        Enemy enemy = enemyObject.GetComponent<Enemy>();
    }
    /// <summary>
    /// Spawn Enemy using its spawn code.
    /// Spawns multiple enemies according to their Minimum and Maxmium amount values.
    /// </summary>
    /// <param name="spawnCode">Spawn Code</param>
    private void Spawn(SpawnCode spawnCode) {
        // spawn a random amount of enemies between the min and max values
        int enemiesToSpawn = Random.Range(spawnCode.MinAmount, spawnCode.MaxAmount + 1);

        for (int i = 0; i < enemiesToSpawn; i++) {
            Vector2 position = (Random.insideUnitCircle.normalized * spawnDistance) + Player.instance.Position;
            GameObject enemyObject = Instantiate(spawnCode.Prefab, position, Quaternion.identity, transform);
        }
    }

    /// <summary>
    /// Checks if this Transform has no more child GameObjects after everything has spawned
    /// </summary>
    /// <returns></returns>
    private IEnumerator CheckForWin() {
        while (!allEnemiesDefeated) {
            if (transform.childCount == 0) {
                onAllEnemiesDefeated?.Invoke();
                allEnemiesDefeated = true;
                Debug.Log("YOU WIN!!!");
                Debug.Log("YOU WIN!!!");
                Debug.Log("YOU WIN!!!");
            }

            yield return new WaitForSeconds(1f);
        }
    }
}

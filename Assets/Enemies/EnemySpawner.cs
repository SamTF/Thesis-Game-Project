using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This Class is responsible for spawning enemies into the game world in the correct order
/// </summary>
public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class SpawnCode {
        [SerializeField]
        private GameObject prefab = null;
        [SerializeField]
        private char code;

        public SpawnCode(GameObject prefab, char code) {
            this.prefab = prefab;
            this.code = code;
        }

        public GameObject Prefab => prefab;
        public char Code => code;
    }


    [Header("ENEMY SPAWNER")]
    [SerializeField][TextArea(5,20)]
    private string spawnOrder = "";
    [SerializeField]
    private SpawnCode chaser = null;
    [SerializeField]
    private SpawnCode shooter = null;
    [SerializeField]
    private SpawnCode ghost = null;
    [SerializeField]
    private SpawnCode bouncer = null;

    private Timer timer = null;


    private void Start() {
        StartCoroutine(SpawnWave());

        timer = new Timer(0);
        timer.Start();
    }

    private void Update() {
        Debug.Log(timer.currentTime.String);
    }


    private IEnumerator SpawnWave() {
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
                    yield return new WaitForSeconds(10f);
                    break;
            }
        }

        yield return null;
    }


    private void Spawn(GameObject prefab) {
        Debug.Log($"[ENEMY SPAWNER] >>> Spawning {prefab.name}");

        Vector2 position = (Random.insideUnitCircle.normalized * 20f) + GameManager.instance.PlayerPosition;

        GameObject enemyObject = Instantiate(prefab, position, Quaternion.identity);
        Enemy enemy = enemyObject.GetComponent<Enemy>();
    }

}

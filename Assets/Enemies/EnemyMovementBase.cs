using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyMovementBase : MonoBehaviour
{
    [Header("BASE ENEMY MOVEMENT")]
    [SerializeField]
    protected Transform body = null;
    [SerializeField]
    protected Transform shadow = null;

    protected Rigidbody2D rb = null;


    protected void Awake() {
        rb = GetComponent<Rigidbody2D>();
        body = body ? body : transform.Find("Body");
        shadow = shadow ? shadow : transform.Find("Shadow");
    }


    ///// Stop following if the Player is dead
    private void OnEnable() {
        Health.onPlayerDeath += OnPlayerDeath;
    }
    private void OnDisable() {
        Health.onPlayerDeath -= OnPlayerDeath;
        StopAllCoroutines();
    }
    private void OnPlayerDeath() {
        this.enabled = false;
    }
}

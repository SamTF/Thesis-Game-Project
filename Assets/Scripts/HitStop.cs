using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitStop : MonoBehaviour
{
    [Header("Hit Stop")]
    [SerializeField]
    private Material flashMaterial = null;
    private Material ogMaterial = null;

    private bool isStopped = false;
    private SpriteRenderer spriteRenderer = null;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        ogMaterial = spriteRenderer.material;
    }

    /// <summary>
    /// Freeze the game for a duration for game feel!
    /// </summary>
    /// <param name="duration">Time in milliseconds. Default: 100ms</param>
    public void Hit(float duration=200) {
        if (isStopped) return;

        Debug.Log("HIT!!!");

        duration = duration/1000;
        StartCoroutine(Freeze(duration));
    }

    private IEnumerator Freeze(float duration) {
        isStopped = true;
        spriteRenderer.material = flashMaterial;
        // Time.timeScale = 1/20f;
        Time.timeScale = 0f;

        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1f;
        isStopped = false;
        spriteRenderer.material = ogMaterial;
    }
}

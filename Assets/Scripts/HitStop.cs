using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitStop : MonoBehaviour
{
    [Header("Hit Stop")]
    [SerializeField][Tooltip("Specify 'Flash' material to use. Uses default material if not specified")]
    private Material flashMaterial = null;
    [SerializeField][Tooltip("Whether to trigger an Animation clip or change the Material directly via code. (Must have Boolean called 'Flash' in Animator.)")]
    private bool useAnimator = false;

    private Material ogMaterial = null;
    private bool isStopped = false;
    private SpriteRenderer spriteRenderer = null;
    private Animator animator = null;

    private void Awake() {
        // Get the appropriate sprite renderer
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null) spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        // Load material shader from Resources
        if (flashMaterial == null) flashMaterial = Resources.Load("Shaders/FlashMaterial") as Material;

        if (useAnimator)    animator = GetComponent<Animator>();

        // storing the original sprite material
        ogMaterial = spriteRenderer.material;
    }

    /// <summary>
    /// Freeze the game for a duration for game feel!
    /// </summary>
    /// <param name="duration">Time in milliseconds. Default: 100ms</param>
    public void Hit(float duration=200) {
        if (isStopped) return;

        duration = duration/1000;
        StartCoroutine(Freeze(duration));
    }

    /// <summary>
    /// Freezes the game for X seconds, during which the Flash sprite material is applied.
    /// </summary>
    /// <param name="duration">Time in seconds to freeze the game for.</param>
    private IEnumerator Freeze(float duration) {
        isStopped = true;
        SetFlashAnim(true);
        // Time.timeScale = 1/20f;
        Time.timeScale = 0f;

        yield return new WaitForSecondsRealtime(duration);

        Time.timeScale = 1f;
        isStopped = false;
        SetFlashAnim(false);
    }

    /// <summary>
    /// Toggle the Flash sprite material animation
    /// </summary>
    /// <param name="toggle">Trigger or end the animation</param>
    private void SetFlashAnim(bool toggle) {
        if (toggle) {
            spriteRenderer.material = flashMaterial;
            animator?.SetBool("Flash", true);
        }
        else {
            spriteRenderer.material = ogMaterial;
            animator?.SetBool("Flash", false);
        }
    }
}

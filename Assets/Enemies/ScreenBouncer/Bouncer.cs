using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bouncer : Enemy
{
    [Header("Screen Bouncer")]
    [SerializeField][Tooltip("Do a spin animation instead of the bop")]
    private bool spinAnimation = true;

    [SerializeField][Tooltip("Number of bounces required to activate this Enemy and allow it to take/receive damage.")][Range(0, 3)]
    private int bouncesToActivate = 3;

    // Status
    private bool isActivated = false;

    // Constants
    private string spriteLayer = null;
    private Color spriteColor;


    void Start() {
        // Enable Spin animation
        if (spinAnimation) {
            animator.SetBool("Spin", true);
            animator.SetBool("Bop", false);
        }

        // Store original sprite settings
        spriteLayer = spriteRenderer.sortingLayerName;
        spriteColor = spriteRenderer.color;

        // Disable everything and initiate the spawning routine
        movement.enabled = false;
        spriteRenderer.sortingLayerName = "Shadows";

        StartCoroutine(FadeIn(1));
        StartCoroutine(SpawnRoutine(2));
    }

    private void Update() {
        if (spinAnimation)
            transform.Rotate(new Vector3(0, 0, -360f) * Time.deltaTime);
    }

    /// <summary>
    /// Lerp animation that fades in the Object, overriding the animator.
    /// </summary>
    /// <param name="duration">Duration of the animation in seconds.</param>
    private IEnumerator FadeIn(float duration = 1f) {
        float time = 0;

        animator.enabled = false;
        Color startColor = new Color(1,1,1,0);
        Color endColor = spriteRenderer.color;
        endColor.a = 0.5f;

        while (time < duration) {
            spriteRenderer.color = Color.Lerp(startColor, endColor, time / duration);
            time += Time.deltaTime;
            yield return new WaitForFixedUpdate(); 
        }
    }

    /// <summary>
    /// Centers the Enemy in screen space every frame until the duration is over. Enables movements once its over.
    /// </summary>
    /// <param name="duration">Time in seconds.</param>
    private IEnumerator SpawnRoutine(float duration = 3f) {
        float time = 0;
        Vector2 cameraSize = new Vector2(Camera.main.pixelWidth, Camera.main.pixelHeight);

        bodyCollider.enabled = false;

        while (time < duration) {
            Vector2 center = Camera.main.ScreenToWorldPoint(cameraSize/2f);
            transform.position = center;

            time += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }

        spriteRenderer.sortingLayerName = spriteLayer;
        movement.enabled = true;
    }


    // Public Getter Setters
    /// <summary>Whether this Enemy is activated or not. Activating will enable colliders.</summary>
    public bool IsActivated {
        get { return isActivated; }

        set { 
            isActivated = value;

            if (value == true) {
                bodyCollider.enabled = true;
                spriteRenderer.color = spriteColor;
            }
        }
    }

    /// <summary>Number of bounces required to activate this Enemy and allow it to take/receive damage.</summary>
    public int BouncesToActivate => bouncesToActivate;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// VFX Object that self destructs after X seconds
/// </summary>
public class Poof : MonoBehaviour
{
    [Header("VFX Object that self destructs after X seconds")]
    [SerializeField][Range(0f, 5f)][Tooltip("How long until the object is destroyed (in seconds)")]
    private float lifeTime = 0.25f;
    [SerializeField]
    private bool moves = false;
    [SerializeField]
    private bool fadesOut = false;

    // Component
    private SpriteRenderer spriteRenderer = null;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        transform.Rotate(0f, 0f, Random.Range(0f, 360f));
        StartCoroutine(SelfDestruct());

        if (fadesOut)   StartCoroutine(FadeOut());
    }

    private void Update() {
        if (moves) {
            Vector3 movement = new Vector3(Mathf.Sin(Time.time * 5f), 1, 0) * Time.deltaTime;
            transform.position += movement;
        }
    }

    /// <summary>
    /// Destroys this object after its lifetime has elapsed.
    /// </summary>
    private IEnumerator SelfDestruct() {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }


    /// <summary>
    /// Fades out the sprite smoothly over its lifetime.
    /// </summary>
    private IEnumerator FadeOut()
    {
        float time = 0;
        float duration = lifeTime * 0.75f;

        Color startColor = spriteRenderer.color;
        Color endColor = new Color(1,1,1,0);

        yield return new WaitForSeconds(lifeTime * 0.25f);

        while (time < duration)
        {
            spriteRenderer.color = Color.Lerp(startColor, endColor, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        spriteRenderer.color = endColor;
    }
}

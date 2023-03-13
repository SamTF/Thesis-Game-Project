using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// VFX Object that self destructs after X seconds
/// </summary>
public class Poof : MonoBehaviour
{
    [Header("VFX Object that self destructs after X seconds")]
    [SerializeField][Range(0f, 1f)][Tooltip("How long until the object is destroyed (in seconds)")]
    private float lifeTime = 0.25f;

    // Start is called before the first frame update
    void Start()
    {
        transform.Rotate(0f, 0f, Random.Range(0f, 360f));
        StartCoroutine(SelfDestruct());
    }

    private IEnumerator SelfDestruct() {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }
}

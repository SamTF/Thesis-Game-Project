using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Components
    private Rigidbody2D rb = null;
    private InputManager input = null;
    private Stats stats = null;
    private GameObject spriteObject = null;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        input = GetComponent<InputManager>();
        stats = GetComponent<Stats>();
        spriteObject = GameObject.Find("Sprite");
    }

    // Component Getters
    public Rigidbody2D RigidBody => rb;
    public InputManager Input => input;
    public Stats Stats => stats;
    public GameObject SpriteObject => spriteObject;
}

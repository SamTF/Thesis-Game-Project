using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Components
    private Rigidbody2D     rb              = null;
    private InputManager    input           = null;
    private Stats           stats           = null;
    private Status          status          = null;

    // Children
    [SerializeField]
    private Transform spriteObject = null;
    [SerializeField]
    private Transform shadow = null;



    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        input = GetComponent<InputManager>();
        stats = GetComponent<Stats>();
        status = GetComponent<Status>();

        spriteObject = spriteObject ? spriteObject : transform.Find("Sprite");
        shadow = shadow ? shadow : transform.Find("Shadow");
    }


    // Component Getters
    public Rigidbody2D RigidBody => rb;
    public InputManager Input => input;
    public Stats Stats => stats;
    public Status Status => status;

    // Child getters
    public Transform SpriteObject => spriteObject;
    public Transform Shadow => shadow;
}

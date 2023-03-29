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
    private CircleCollider2D collider       = null;
    private Health          health          = null;
    private Animator        animator        = null;

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
        collider = GetComponent<CircleCollider2D>();
        health = GetComponent<Health>();
        animator = GetComponent<Animator>();

        spriteObject = spriteObject ? spriteObject : transform.Find("Sprite");
        shadow = shadow ? shadow : transform.Find("Shadow");
    }


    // Component Getters
    public Rigidbody2D RigidBody => rb;
    public InputManager Input => input;
    public Stats Stats => stats;
    public Status Status => status;
    public Health Health => health;
    public Animator Animator => animator;

    // Child getters
    public Transform SpriteObject => spriteObject;
    public Transform Shadow => shadow;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Components
    private Rigidbody2D rb = null;
    private InputManager input = null;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        input = GetComponent<InputManager>();
    }

    // Component Getters
    public Rigidbody2D RigidBody => rb;
    public InputManager Input => input;
}

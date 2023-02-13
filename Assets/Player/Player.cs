using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Components
    private Rigidbody2D rb = null;
    private InputManager input = null;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        input = GetComponent<InputManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Component Getters
    public Rigidbody2D RigidBody => rb;
    public InputManager Input => input;
}

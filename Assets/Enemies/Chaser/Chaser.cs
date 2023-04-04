using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Basic enemy type that chases the Player around smoothly. Attacks by contact with the Player.
/// </summary>
public class Chaser : Enemy
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(health);
        Debug.Log(spriteRenderer);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

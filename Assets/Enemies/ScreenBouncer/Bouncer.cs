using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bouncer : Enemy
{
    [Header("Screen Bouncer")]
    [SerializeField][Tooltip("Do a spin animation instead of the bop")]
    private bool spinAnimation = true;


    void Start() {
        if (spinAnimation) {
            animator.SetBool("Spin", true);
            animator.SetBool("Bop", false);
        }
    }
}

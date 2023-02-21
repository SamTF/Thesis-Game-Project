using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status : MonoBehaviour
{
    // Bools
    private bool isAlive = true;
    private bool isJumping = false;
    private bool canJump = true;
    private bool canBackflip = true;
    private bool isDodging = false;
    private bool canDodge = true;

    // Cooldowns
    private float jumpCooldown = 1f;
    private float backflipCooldown = 1f;
    private float dodgeCooldown = 1f;

    // Getter Setters
    public bool IsJumping {
        get { return isJumping; }
        set {
            isJumping = value;

            if (value == true) {
                canJump = false;
            }
            else {
                Ref<bool> r = new Ref<bool>(canJump);
                StartCoroutine(Cooldown(r, false, 1f));
            }
        }
    }

    public bool IsDodging {
        get { return isDodging; }
        set {
            isDodging = value;
            if (value == true) {
                canDodge = false;
            } else {
                Ref<bool> r = new Ref<bool>(canDodge);
                StartCoroutine(Cooldown(r, false, 1f));
            }
        }
    }

    /// <summary>
    /// Holds a reference to a variable. Useable in Coroutines :D
    /// </summary>
    /// <typeparam name="T">Variable type</typeparam>
    /// From: https://forum.unity.com/threads/passing-ref-variable-to-coroutine.379640/
    private class Ref<T>
    {
        private T backing;
        public T Value { get { return backing; } set { backing = value; } }
        public Ref(T reference)
        {
            backing = reference;
        }
    }

    /// <summary>
    /// Sets any bool status variable to a value, waits X seconds, then sets it to the opposite value.
    /// </summary>
    /// <param name="variable">Reference to the status value</param>
    /// <param name="value">True or false</param>
    /// <param name="duration">How many seconds to wait before resetting the value</param>
    /// <returns></returns>
    private IEnumerator Cooldown(Ref<bool> variable, bool value, float duration) {
        variable.Value = value;
        yield return new WaitForSeconds(duration);
        variable.Value = !value;
    }
}

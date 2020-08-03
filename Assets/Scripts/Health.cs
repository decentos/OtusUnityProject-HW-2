using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float current;

    public void ApplyDamage(float damage)
    {
        current -= damage;
        if (current < 0.0f)
            current = 0.0f;
    }
}

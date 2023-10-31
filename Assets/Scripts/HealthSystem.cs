using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public event Action OnDead;
    public event Action OnDamaged;

    [SerializeField]
    private int health = 100;

    private int healthMax;

    private void Awake()
    {
        healthMax = health;
    }

    public void Damage(int damageAmount)
    {
        if (health == 0) return;

        health = Mathf.Max(health - damageAmount, 0);

        OnDamaged?.Invoke();

        if (health == 0)
        {
            Die();
        }

        Debug.Log(health);
    }

    private void Die()
    {
        OnDead?.Invoke();
    }

    public float GetHealthNormalized()
    {
        return (float)health / healthMax;
    }
}

using System;
using UnityEngine;

public sealed class Health : MonoBehaviour
{
    public event Action OnDeath;
    public Flash flashScript;
    public float currentHealth;
    public float maxHealth;

    void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        if (damage <= 0 || currentHealth <= 0) return;

        currentHealth -= damage;

        if (flashScript != null)
        {
            StartCoroutine(flashScript.FlashEffect());
        }

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            OnDeath?.Invoke();
        }
    }

    public void Heal(float health)
    {
        if (health <= 0 || currentHealth <= 0) return;

        float newHealth = currentHealth + health;
        currentHealth = (newHealth < maxHealth) ? newHealth : maxHealth;
    }
}

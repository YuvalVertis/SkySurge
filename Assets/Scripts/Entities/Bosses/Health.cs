using UnityEngine;

public sealed class Health : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;
    public Flash flashScript;

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
            Die();
        }
    }

    public void Die()
    {

    }

    public void Heal(float health)
    {
        if (health <= 0 || currentHealth <= 0) return;

        float newHealth = currentHealth + health;
        currentHealth = (newHealth < maxHealth) ? newHealth : maxHealth;
    }
}

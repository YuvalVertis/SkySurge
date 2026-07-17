using UnityEngine;

public sealed class Health : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;
    private Flash flashScript;

    void Awake()
    {
        currentHealth = maxHealth;
        flashScript = GetComponent<Flash>();
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (flashScript != null) 
        {
            StartCoroutine(flashScript.FlashEffect()); 
        }
    }
}

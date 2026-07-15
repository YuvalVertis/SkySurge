using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;
    private Flash flashScript;
    private void Awake()
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

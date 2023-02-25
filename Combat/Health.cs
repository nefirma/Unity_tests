using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public float maxHealth;
    public float maxShield;
    public float shieldRecoverySpeed;

    public float currentHealth;
    public float currentShield;

    public Slider healthSlider;
    public Slider shieldSlider;
    public bool ShieldOn = true;

    private void Start()
    {
        currentHealth = maxHealth;
        currentShield = maxShield;
        UpdateHealthSlider();
        UpdateShieldSlider();
    }

    // в каждом кадре восстанавливаем щит
    private void Update()
    {
        RecoverShield();
        UpdateHealthSlider();
        UpdateShieldSlider();
    }
    
    public void TakeDamage(float damage)
    {
        if (currentShield > 0)
        {
            float shieldDamage = Mathf.Min(currentShield, damage);
            currentShield -= shieldDamage;
            damage -= shieldDamage;
        }

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }

    public void TakeShieldDamage(float damage)
    {
        currentShield -= damage;
        if (!(currentShield < 0)) return;
        currentShield = 0;
        ShieldOn = false;
    }


    private void RecoverShield()
    {
        if (!(currentShield < maxShield) || ShieldOn != true) return;
        if (currentShield >= 10)
        {
            currentShield += shieldRecoverySpeed * Time.deltaTime;
        }

        if (currentShield > maxShield)
        {
            currentShield = maxShield;
        }
    }


    private void UpdateHealthSlider()
    {
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth / maxHealth;
        }
    }

    private void UpdateShieldSlider()
    {
        if (shieldSlider != null)
        {
            shieldSlider.value = currentShield / maxShield;
        }
    }

    private void Die()
    {
        // TODO: Implement death behavior
    }
}
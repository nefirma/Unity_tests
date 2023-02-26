using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [HideInInspector]
    public float maxHealth; // максимальное здоровье корабля
    [HideInInspector]
    public float maxShield; // максимальный щит корабля
    [HideInInspector]
    public float shieldRecoverySpeed;   // скорость восстановления щита
    
    //Shield object
    public GameObject shieldObject;
    public GameObject shieldObjectOn;
    public GameObject shieldObjectGlow;
    // public GameObject hitGlow;

    public float shieldRecoverySpeedMod; // модификатор скорости восстановления щита
    public float baseHealth; // базовое здоровье корабля без модулей
    public float baseShield; // базовый щит корабля без модулей
    public float baseShieldRecoverySpeed; // базовая скорость восстановления щита без модулей

    public float currentHealth;
    public float currentShield;

    public Slider healthSlider;
    public Slider shieldSlider;
    public Text healthText;
    public Text shieldText;
    
    public bool shieldOn = true;

    private void Start()
    {
        maxHealth = baseHealth;
        maxShield = baseShield;
        shieldRecoverySpeed = baseShieldRecoverySpeed;
        currentHealth = maxHealth;
        currentShield = maxShield;
        UpdateHealthSlider();
        UpdateShieldSlider();
        // SwitchShield();
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
        if (currentShield > 1)
        {
            float shieldDamage = Mathf.Min(currentShield, damage);
            currentShield -= shieldDamage;
            damage -= shieldDamage;
        }

        currentHealth -= damage;

        if (currentHealth <= 1)
        {
            currentHealth = 0;
            Die();
        }
    }

    public void TakeShieldDamage(float damage)
    {
        currentShield -= damage;
        if (!(currentShield <= 1)) return;
        currentShield = 0;
        // shieldOn = false;
        Debug.Log("Shield is broken");
        shieldObjectOn.SetActive(false);
        SwitchShield();
    }


    private void RecoverShield()
    {
        if (!(currentShield < maxShield) || shieldOn != true) return;
        if (currentShield >= 10)
        {
            currentShield +=  shieldRecoverySpeedMod * shieldRecoverySpeed * Time.deltaTime;
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
            healthText.text = Mathf.RoundToInt(currentHealth) + " / " + maxHealth;
        }
    }
    private void UpdateShieldSlider()
    {
        if (shieldSlider != null)
        {
            shieldSlider.value = currentShield / maxShield;
            shieldText.text = Mathf.RoundToInt(currentShield) + " / " + maxShield;
        }
    }
    private void Die()
    {
        // TODO: Implement death behavior
    }
    
    //Switch shield on/off
    public void SwitchShield()
    {
        if (shieldOn)
        {
            shieldOn = false;
            shieldObject.SetActive(false);
        }
        else
        {
            shieldOn = true;
            shieldObject.SetActive(true);
        }
    }
}



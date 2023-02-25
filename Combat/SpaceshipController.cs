using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpaceshipController : MonoBehaviour
{
    public int weaponSlots = 2;
    public List<Weapon> availableWeapons;
    public List<Weapon> activeWeapons;
    public List<Dropdown> weaponDropdowns;
    public Transform weaponMount; // Transform, к которому будет прикреплено оружие
    public List<GameObject> weaponInstances;
    private Weapon[] initialWeapons;

    private void Start()
    {
        // Заполняем выпадающие списки оружия
        foreach (Dropdown dropdown in weaponDropdowns)
        {
            dropdown.ClearOptions();
            foreach (Weapon weapon in availableWeapons)
            {
                dropdown.options.Add(new Dropdown.OptionData(weapon.weaponName));
            }
            dropdown.onValueChanged.AddListener(delegate { OnWeaponChanged(dropdown); });
        }

        // Инициализируем активное оружие
        activeWeapons = new List<Weapon>();
        for (int i = 0; i < weaponSlots; i++)
        {
            activeWeapons.Add(availableWeapons[0]); // Выбираем первое доступное оружие
            // Инстанцируем оружие и активируем его в соответствии со списком activeWeapons
            Weapon instantiatedWeapon = Instantiate(activeWeapons[i], weaponMount, false);
            instantiatedWeapon.gameObject.SetActive(true);
            activeWeapons[i] = instantiatedWeapon;
            weaponInstances.Add(instantiatedWeapon.gameObject);
        }

        initialWeapons = activeWeapons.ToArray();
        //Write Debug.log with the Ship's name and the weapon's name in the initialWeapons array, and the activeWeapons list
        Debug.Log("SpaceshipController(Start) -> initialWeapons[0].weaponName: " + initialWeapons[0].weaponName);
        Debug.Log("SpaceshipController(Start) -> activeWeapons[0].weaponName: " + activeWeapons[0].weaponName);
        
    }

    private void OnWeaponChanged(Dropdown dropdown)
    {
        int dropdownIndex = weaponDropdowns.IndexOf(dropdown);
        Weapon newWeapon = availableWeapons[dropdown.value];
        activeWeapons[dropdownIndex] = newWeapon;

        // Обновляем уже существующий экземпляр оружия, который соответствует измененному элементу activeWeapons
        Transform oldWeaponTransform = weaponInstances[dropdownIndex].transform;
        Weapon instantiatedWeapon = Instantiate(newWeapon, oldWeaponTransform.position, oldWeaponTransform.rotation, weaponMount);
        Destroy(oldWeaponTransform.gameObject);
        instantiatedWeapon.gameObject.SetActive(true);
        activeWeapons[dropdownIndex] = instantiatedWeapon; // <- Update the active weapons list
        weaponInstances[dropdownIndex] = instantiatedWeapon.gameObject;
    }

    public void Restart()
    {

        // Reset the health
        Health health = GetComponent<Health>();
        if (health != null)
        {
            health.currentHealth = health.maxHealth;
        }

        // Reset the shield
        if (health != null)
        {
            health.currentShield = health.maxShield;
        }

        // Reactivate the weapon dropdowns
        foreach (Dropdown dropdown in weaponDropdowns)
        {
            dropdown.gameObject.SetActive(true);
        }
    }
}



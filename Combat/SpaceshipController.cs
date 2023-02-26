using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpaceshipController : MonoBehaviour
{
    public int weaponSlots = 2;
    public int moduleSlots = 3;
    public List<Weapon> availableWeapons;
    public List<Weapon> activeWeapons;
    public List<Dropdown> weaponDropdowns;
    public Transform weaponMount; // Transform, к которому будет прикреплено оружие
    public List<GameObject> weaponInstances;
    public List<Module> availableModules;
    public List<Module> activeModules;
    public List<Dropdown> moduleDropdowns;
    public List<GameObject> moduleInstances;
    public GameObject weaponPoint1;
    public GameObject weaponPoint2;
    public List<Transform> weaponMounts;

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

        // Заполняем выпадающие списки модулей
        foreach (Dropdown dropdown in moduleDropdowns)
        {
            dropdown.ClearOptions();
            foreach (Module module in availableModules)
            {
                dropdown.options.Add(new Dropdown.OptionData(module.moduleName));
            }
            dropdown.onValueChanged.AddListener(delegate { OnModuleChanged(dropdown); });
        }
        
        // Инициализируем активное оружие
        for (int i = 0; i < weaponSlots; i++)
        {
            activeWeapons.Add(availableWeapons[0]); // Выбираем первое доступное оружие
            // Инстанцируем оружие и активируем его в соответствии со списком activeWeapons
            Weapon instantiatedWeapon = Instantiate(activeWeapons[i], weaponMounts[i], false);
            instantiatedWeapon.gameObject.SetActive(true);
            activeWeapons[i] = instantiatedWeapon;
            weaponInstances.Add(instantiatedWeapon.gameObject);
        }

      
        
        // Инициализируем активные модули
        activeModules = new List<Module>();
        for (int i = 0; i < moduleSlots; i++)
        {
            activeModules.Add(availableModules[0]); // Выбираем первый доступный модуль
            // Инстанцируем модуль и активируем его в соответствии со списком activeModules
            Module instantiatedModule = Instantiate(activeModules[i], weaponMount, false);
            instantiatedModule.gameObject.SetActive(true);
            activeModules[i] = instantiatedModule;
            moduleInstances.Add(instantiatedModule.gameObject);
        }
        //Вызов метода CheckModules() для проверки активных модулей
        CheckModules();
    }

    private void OnWeaponChanged(Dropdown dropdown)
    {
        int dropdownIndex = weaponDropdowns.IndexOf(dropdown);
        Weapon newWeapon = availableWeapons[dropdown.value];
        activeWeapons[dropdownIndex] = newWeapon;

        // Обновляем уже существующий экземпляр оружия, который соответствует измененному элементу activeWeapons
        Transform oldWeaponTransform = weaponInstances[dropdownIndex].transform;
        // Weapon instantiatedWeapon = Instantiate(newWeapon, oldWeaponTransform.position, oldWeaponTransform.rotation, weaponMount);
        Weapon instantiatedWeapon = Instantiate(newWeapon, weaponMounts[dropdownIndex], false);
        Destroy(oldWeaponTransform.gameObject);
        instantiatedWeapon.gameObject.SetActive(true);
        activeWeapons[dropdownIndex] = instantiatedWeapon; // <- Update the active weapons list
        weaponInstances[dropdownIndex] = instantiatedWeapon.gameObject;
    }

    private void OnModuleChanged(Dropdown dropdown)
    {
        int dropdownIndex = moduleDropdowns.IndexOf(dropdown);
        Module newModule = availableModules[dropdown.value];
        activeModules[dropdownIndex] = newModule;

        // Обновляем уже существующий экземпляр модуля, который соответствует измененному элементу activeModules
        Transform oldModuleTransform = moduleInstances[dropdownIndex].transform;
        Module instantiatedModule = Instantiate(newModule, oldModuleTransform.position, oldModuleTransform.rotation, weaponMount);
        Destroy(oldModuleTransform.gameObject);
        instantiatedModule.gameObject.SetActive(true);
        activeModules[dropdownIndex] = instantiatedModule; // <- Update the active modules list
        moduleInstances[dropdownIndex] = instantiatedModule.gameObject;
        CheckModules();
    }

    public void Restart()
    {
        // Reset the health, shield and shield recovery speed
        Health health = GetComponent<Health>();
        if (health != null)
        {
            health.currentHealth = health.maxHealth;
            health.shieldRecoverySpeed = 1f;
            health.currentShield = health.maxShield;
        }
        
        // Reactivate the weapon dropdowns
        foreach (Dropdown dropdown in weaponDropdowns)
        {
            dropdown.gameObject.SetActive(true);
        }
        // Reactivate the module dropdowns
        foreach (Dropdown dropdown in moduleDropdowns)
        {
            dropdown.gameObject.SetActive(true);
        }

        CheckModules();
    }

    // Check the active modules and apply the effects
    private void CheckModules()
    {
        ResetModules();
        foreach (Module module in activeModules)
        {
            switch (module.moduleType)
            {
                case Module.ModuleType.Shield:
                {
                    Health health = GetComponent<Health>();
                    if (health != null)
                    {
                        //Добавляем щит к максимальному значению щита и текущему значению щита
                        health.maxShield += module.shield;
                        health.currentShield = health.maxShield;
                        Debug.Log("SpaceshipController(CheckModules) -> health.maxShield: " + health.maxShield);
                    }

                    break;
                }
                case Module.ModuleType.ShieldReload:
                {
                    Health health = GetComponent<Health>();
                    if (health != null)
                    {
                        //Изменяем скорость восстановления щита на значение модуля в процентах от текущего значения.
                        var moduleShieldReloadMod = health.shieldRecoverySpeedMod;
                        health.shieldRecoverySpeedMod += moduleShieldReloadMod * (module.shieldReloadMod / 100f);
                        Debug.Log("SpaceshipController(CheckModules) -> module.shieldReloadMod: " + module.shieldReloadMod + " health.shieldRecoverySpeedMod: " + health.shieldRecoverySpeedMod);
                        // health.shieldRecoverySpeed = health.baseShieldRecoverySpeed + (health.baseShieldRecoverySpeed * module.shieldReloadMod / 100f);
                        Debug.Log("SpaceshipController(CheckModules) -> health.shieldRecoverySpeed: " + health.shieldRecoverySpeed);
                    }
                    break;
                }
                case Module.ModuleType.Health:
                {
                    Health health = GetComponent<Health>();
                    if (health != null)
                    {
                        //Добавляем здоровье к максимальному значению здоровья и текущему значению здоровья
                        health.maxHealth += module.health;
                        health.currentHealth = health.maxHealth;
                        Debug.Log("SpaceshipController(CheckModules) -> health.maxHealth: " + health.maxHealth);
                    }
                    break;
                }
                case Module.ModuleType.ReloadTime:
                {
                    foreach (Weapon weapon in activeWeapons)
                    {
                        //Изменяем модификатор перезарядки оружия при помощи модуля
                        var weaponReloadTimeMod = weapon.reloadTimeMod;
                        weapon.reloadTimeMod += weaponReloadTimeMod*(module.reloadTimeMod / 100f);
                        Debug.Log("SpaceshipController(CheckModules) -> weapon.reloadTimeMod: " + weapon.reloadTimeMod + " weapon.reloadTime: " + weapon.reloadTime*weapon.reloadTimeMod);
                    }
                    break;
                }
            }
        }
    }
    private void ResetModules()
    {
        Health health = GetComponent<Health>();
        if (health != null)
        {
            health.maxHealth = health.baseHealth;
            health.currentHealth = health.maxHealth;
            health.maxShield = health.baseShield;
            health.currentShield = health.maxShield;
            health.shieldRecoverySpeedMod = health.baseShieldRecoverySpeed;
            health.shieldObjectOn.SetActive(true);
            health.shieldObject.SetActive(true);
            health.shieldOn = true;
        }
        foreach (Weapon weapon in activeWeapons)
        {
            weapon.reloadTime = weapon.baseReloadTime;
            weapon.reloadTimeMod = 1f;
        }
    }
}



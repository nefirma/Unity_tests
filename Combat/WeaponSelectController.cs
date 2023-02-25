//Скрипт для выбора оружия в меню

using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class WeaponSelectController : MonoBehaviour
{
    public Dropdown weaponDropdown;
    public SpaceshipController spaceshipController;
    public int weaponIndex;
    [SerializeField]
    private ColorBlock defaultColors;
    [SerializeField]
    private ColorBlock selectedColors;
    
    private void Awake()
    {
        weaponDropdown.onValueChanged.AddListener(OnWeaponSelected);
    }
    
    private bool IsWeaponSelected()
    {
        return weaponDropdown.value != 0;
    }

    
    void Start()
    {
        // Сохраняем цвета по умолчанию
        defaultColors = weaponDropdown.colors;

        // Создаем новый экземпляр структуры ColorBlock для выбранного состояния
        selectedColors = new ColorBlock();
        selectedColors.normalColor = Color.green;
        selectedColors.highlightedColor = Color.green;
        selectedColors.pressedColor = Color.green;
        selectedColors.selectedColor = Color.green;
        selectedColors.disabledColor = defaultColors.disabledColor;
        selectedColors.colorMultiplier = defaultColors.colorMultiplier;
        selectedColors.fadeDuration = defaultColors.fadeDuration;
        weaponDropdown.onValueChanged.AddListener(delegate
        {
            DropdownValueChanged(weaponDropdown);
        });

        // Устанавливаем начальное значение выпадающего списка
        if (weaponIndex >= 0 && weaponIndex < spaceshipController.activeWeapons.Count+1)
        {
            // weaponDropdown.value = spaceshipController.availableWeapons.IndexOf(spaceshipController.activeWeapons[weaponIndex]);
            // Устанавливаем начальное значение выпадающего списка как 0, те первое оружие в списке
            weaponDropdown.value = 0;
            Debug.Log("weaponIndex = " + weaponIndex);
        }

    }

    void DropdownValueChanged(Dropdown dropdown)
    {
        spaceshipController.activeWeapons[weaponIndex] = spaceshipController.availableWeapons[dropdown.value];
    }
    
    public void OnWeaponSelected(int index)
    {
        if (index >= 0) // && index < spaceshipController.activeWeapons.Count)
        {
            // Оружие было выбрано, устанавливаем цвета для выбранного состояния
            weaponDropdown.colors = selectedColors;
        }
        else
        {
            // Оружие не было выбрано, устанавливаем цвета по умолчанию
            weaponDropdown.colors = defaultColors;
        }
    }
}
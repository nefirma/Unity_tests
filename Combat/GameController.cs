using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject spaceship1;
    public GameObject spaceship2;
    public Text resultText;
    public Button startBattleButton;
    public Button restartButton;
    private bool isGameOver = false;
    public bool isBattleActive = false;

    private void Start()
    {
        // Fill the dropdown menus with available weapons for each spaceship
        SpaceshipController spaceship1Controller = spaceship1.GetComponent<SpaceshipController>();
        foreach (Weapon weapon in spaceship1Controller.activeWeapons)
        {
            AddWeaponToDropdown(spaceship1Controller.weaponDropdowns[0], weapon);
            AddWeaponToDropdown(spaceship1Controller.weaponDropdowns[1], weapon);
        }

        SpaceshipController spaceship2Controller = spaceship2.GetComponent<SpaceshipController>();
        foreach (Weapon weapon in spaceship2Controller.activeWeapons)
        {
            AddWeaponToDropdown(spaceship2Controller.weaponDropdowns[0], weapon);
            AddWeaponToDropdown(spaceship2Controller.weaponDropdowns[1], weapon);
        }

        resultText.gameObject.SetActive(false);
        startBattleButton.onClick.AddListener(OnStartBattleButtonClicked);
        restartButton.onClick.AddListener(OnRestartButtonClicked);
    }

    private void AddWeaponToDropdown(Dropdown dropdown, Weapon weapon)
    {
        dropdown.options.Add(new Dropdown.OptionData(weapon.weaponName));
    }   

    private void OnStartBattleButtonClicked()
    {
        SpaceshipController spaceship1Controller = spaceship1.GetComponent<SpaceshipController>();
        SpaceshipController spaceship2Controller = spaceship2.GetComponent<SpaceshipController>();
        
        // Hide the dropdown menus
        spaceship1Controller.weaponDropdowns[0].gameObject.SetActive(false);
        spaceship2Controller.weaponDropdowns[0].gameObject.SetActive(false);

        // Hide rest of dropdowns
        spaceship1Controller.weaponDropdowns[1].gameObject.SetActive(false);
        spaceship2Controller.weaponDropdowns[1].gameObject.SetActive(false);

        // Hide the start button and show the restart button
        startBattleButton.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(true);
        isBattleActive = true;
        Debug.Log("GameController(OnStartBattleButtonClicked) -> isBattleActive: " + isBattleActive);
        
        // Start the battle
        StartCoroutine(Battle());        
        
    }

    private void OnRestartButtonClicked()
    {
        // Restart the game. Stop coroutines and reset the game. 
        StopAllCoroutines();
        isGameOver = false;
        isBattleActive = false;

        // Reset the state of the spaceships
        spaceship1.GetComponent<SpaceshipController>().Restart();
        spaceship2.GetComponent<SpaceshipController>().Restart();
        
        // Reset the state of the game controller
        resultText.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);
        startBattleButton.gameObject.SetActive(true);
        
        // Reset the state of the dropdown menus
        SpaceshipController spaceship1Controller = spaceship1.GetComponent<SpaceshipController>();
        spaceship1Controller.weaponDropdowns[0].gameObject.SetActive(true);
        spaceship1Controller.weaponDropdowns[1].gameObject.SetActive(true);
        SpaceshipController spaceship2Controller = spaceship2.GetComponent<SpaceshipController>();
        spaceship2Controller.weaponDropdowns[0].gameObject.SetActive(true);
        spaceship2Controller.weaponDropdowns[1].gameObject.SetActive(true);

    }
     private IEnumerator Battle()
     {   
         // Debug.Log("GameController(Battle) -> isBattleActive: " + isBattleActive);

         SpaceshipController spaceship1Controller = spaceship1.GetComponent<SpaceshipController>();
         SpaceshipController spaceship2Controller = spaceship2.GetComponent<SpaceshipController>();

         while (spaceship1Controller != null && spaceship2Controller != null && !isGameOver)
         {
             // Spaceship 1 attacks Spaceship 2
             if (Vector3.Distance(spaceship1.transform.position, spaceship2.transform.position) <= 100f)
             {
                 foreach (Weapon weapon in spaceship1Controller.activeWeapons)
                 {
                     weapon.Shoot(spaceship1, spaceship2);
                 }
             }

             // Spaceship 2 attacks Spaceship 1
             if (Vector3.Distance(spaceship2.transform.position, spaceship1.transform.position) <= 100f)
             {
                 foreach (Weapon weapon in spaceship2Controller.activeWeapons)
                 {
                     weapon.Shoot(spaceship2, spaceship1);
                 }
             }

             // Check for game over
             if (spaceship1Controller == null || spaceship1Controller.GetComponent<Health>().currentHealth <= 0)
             {
                 EndGame(spaceship2);
             }
             else if (spaceship2Controller == null || spaceship2Controller.GetComponent<Health>().currentHealth <= 0)
             {
                 EndGame(spaceship1);
             }
             yield return null;
         }
     }

    private void EndGame(GameObject winner)
    {
        isGameOver = true;

        if (winner == spaceship1)
        {
            resultText.text = "Spaceship 1 wins!";
        }
        else if (winner == spaceship2)
        {
            resultText.text = "Spaceship 2 wins!";
        }
        resultText.gameObject.SetActive(true);
    }
}


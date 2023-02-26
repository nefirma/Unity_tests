using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject spaceship1;
    public GameObject spaceship2;
    public Text resultText;
    public Button startBattleButton;
    public Button restartButton;
    public Button exitButton;
    private bool isGameOver = false;
    public bool isBattleActive = false;
    
    private void Start()
    {
        // Fill the dropdown menus with available weapons for each spaceship
        var spaceship1Controller = spaceship1.GetComponent<SpaceshipController>();
        foreach (var weapon in spaceship1Controller.activeWeapons)
        {
            AddWeaponToDropdown(spaceship1Controller.weaponDropdowns[0], weapon);
            AddWeaponToDropdown(spaceship1Controller.weaponDropdowns[1], weapon);
        }

        var spaceship2Controller = spaceship2.GetComponent<SpaceshipController>();
        foreach (var weapon in spaceship2Controller.activeWeapons)
        {
            AddWeaponToDropdown(spaceship2Controller.weaponDropdowns[0], weapon);
            AddWeaponToDropdown(spaceship2Controller.weaponDropdowns[1], weapon);
        }
        
        // Fill the dropdown menus with available modules for each spaceship
        foreach (var module in spaceship1Controller.activeModules)
        {
            AddModuleToDropdown(spaceship1Controller.moduleDropdowns[0], module);
            AddModuleToDropdown(spaceship1Controller.moduleDropdowns[1], module);
            // AddModuleToDropdown(spaceship1Controller.moduleDropdowns[2], module);
        }
        foreach (var module in spaceship2Controller.activeModules)
        {
            AddModuleToDropdown(spaceship2Controller.moduleDropdowns[0], module);
            AddModuleToDropdown(spaceship2Controller.moduleDropdowns[1], module);
            AddModuleToDropdown(spaceship2Controller.moduleDropdowns[2], module);
        }
        resultText.gameObject.SetActive(false);
        startBattleButton.onClick.AddListener(OnStartBattleButtonClicked);
        restartButton.onClick.AddListener(OnRestartButtonClicked);
        restartButton.gameObject.SetActive(false);
        exitButton.onClick.AddListener(OnExitButtonClicked);
    }

    private void AddWeaponToDropdown(Dropdown dropdown, Weapon weapon)
    {
        dropdown.options.Add(new Dropdown.OptionData(weapon.weaponName));
    }   

    private void AddModuleToDropdown(Dropdown dropdown, Module module)
    {
        dropdown.options.Add(new Dropdown.OptionData(module.moduleName));
    }
    
    
    private void OnStartBattleButtonClicked()
    {
        var spaceship1Controller = spaceship1.GetComponent<SpaceshipController>();
        var spaceship2Controller = spaceship2.GetComponent<SpaceshipController>();

  
        
        // Hide the dropdown menus
        spaceship1Controller.weaponDropdowns[0].gameObject.SetActive(false);
        spaceship2Controller.weaponDropdowns[0].gameObject.SetActive(false);
        spaceship1Controller.moduleDropdowns[0].gameObject.SetActive(false);
        spaceship1Controller.moduleDropdowns[1].gameObject.SetActive(false);
        // spaceship1Controller.moduleDropdowns[2].gameObject.SetActive(false);
        spaceship2Controller.moduleDropdowns[0].gameObject.SetActive(false);
        spaceship2Controller.moduleDropdowns[1].gameObject.SetActive(false);
        spaceship2Controller.moduleDropdowns[2].gameObject.SetActive(false);
        
        
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
        var spaceship1Controller = spaceship1.GetComponent<SpaceshipController>();
        var spaceship2Controller = spaceship2.GetComponent<SpaceshipController>();
        spaceship1Controller.weaponDropdowns[0].gameObject.SetActive(true);
        spaceship1Controller.weaponDropdowns[1].gameObject.SetActive(true);
        spaceship2Controller.weaponDropdowns[0].gameObject.SetActive(true);
        spaceship2Controller.weaponDropdowns[1].gameObject.SetActive(true);
        spaceship1Controller.moduleDropdowns[0].gameObject.SetActive(true);
        spaceship1Controller.moduleDropdowns[1].gameObject.SetActive(true);
        // spaceship1Controller.moduleDropdowns[2].gameObject.SetActive(true);
        spaceship2Controller.moduleDropdowns[0].gameObject.SetActive(true);
        spaceship2Controller.moduleDropdowns[1].gameObject.SetActive(true);
        spaceship2Controller.moduleDropdowns[2].gameObject.SetActive(true);
        
    }
     private IEnumerator Battle()
     {   
         var spaceship1Controller = spaceship1.GetComponent<SpaceshipController>();
         var spaceship2Controller = spaceship2.GetComponent<SpaceshipController>();

         while (spaceship1Controller != null && spaceship2Controller != null && !isGameOver)
         {
             // Spaceship 1 attacks Spaceship 2
             if (Vector3.Distance(spaceship1.transform.position, spaceship2.transform.position) <= 100f)
             {
                 foreach (var weapon in spaceship1Controller.activeWeapons)
                 {
                     weapon.Shoot(spaceship1, spaceship2);
                 }
             }

             // Spaceship 2 attacks Spaceship 1
             if (Vector3.Distance(spaceship2.transform.position, spaceship1.transform.position) <= 100f)
             {
                 foreach (var weapon in spaceship2Controller.activeWeapons)
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

    private void EndGame(Object winner)
    {
        isGameOver = true;
        resultText.color = Color.white;
        resultText.fontSize = 50;
        
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
    

    private void OnExitButtonClicked()
    {
        Application.Quit();
    }

}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Menu Navigation")]
    
    [SerializeField] SaveSlotsMenu saveSlotsmenu;

    [Header("Menu Buttons")]

    [SerializeField] private Button newGameButton;

    [SerializeField] private Button continueGameButton;

    [SerializeField] private Button loadGameButton;

    private void Start()
    {
        DisableButtonsDependingOnData();
    }

    void DisableButtonsDependingOnData()
    {
        if (!DataStorageManager.instance.HasGameData())
        {
            continueGameButton.interactable = false;
            loadGameButton.interactable = false;
        }
    }

    public void OnNewGameClicked()
    {
        saveSlotsmenu.ActivateMenu(false);
        this.DeactivateMenu();
    }

    public void OnLoadGameClicked()
    {
        saveSlotsmenu.ActivateMenu(true);
        this.DeactivateMenu();
    }

    public void OnContinueGameClicked()
    {
        DisableMenuButtons();

        DataStorageManager.instance.SaveGame();

        SceneManager.LoadSceneAsync(DataStorageManager.instance.firstLevelName);
    }

    void DisableMenuButtons()
    {
        newGameButton.interactable = false;
        continueGameButton.interactable = false;
    }

    public void ActivateMenu()
    {
        this.gameObject.SetActive(true);
        DisableButtonsDependingOnData();
    }

    public void DeactivateMenu()
    {
        this.gameObject.SetActive(false);
    }
}

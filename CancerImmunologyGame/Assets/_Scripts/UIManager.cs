using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : Singleton<UIManager>
{

    [SerializeField]
    public GameObject WinScreen = null;

    public Image ImmunotherapyIcon = null;
    public Color ImmunotherapyCanActivateColour = Color.cyan;
    public Color ImmunotherapyCannotActivateColour = Color.white;
    public Button ImmunotherapyButton = null;

    /////////////////////////////////////
    public GameObject BlackScreenPanel;
    public GameObject MainMenuPanel;



    public void QuitGame()
    {
        Application.Quit();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}

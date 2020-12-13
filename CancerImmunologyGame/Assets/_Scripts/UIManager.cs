using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : Singleton<UIManager>
{
	private const int FIRST_PLAY_SCENE_ID = 2;

	[Header("UI Panels")]
	[SerializeField] private GameObject MainMenuPanel = null;
	[SerializeField] private GameObject WinScreenPanel = null;


	public void WinScreen()
	{
		WinScreenPanel.SetActive(true);
	}

	public void OpenMainMenu()
	{
		WinScreenPanel.SetActive(false);
		MainMenuPanel.SetActive(true);
	}

	// Main Menu Functionality

	public void Play()
	{
		Debug.Log("Play");
		MainMenuPanel.SetActive(false);
		SceneManager.LoadScene(FIRST_PLAY_SCENE_ID);
	}

	//public void Credits()
	//{
	//}

	//public void Settings()
	//{
	//}

	public void ClosePanels()
	{
		WinScreenPanel.SetActive(false);
		MainMenuPanel.SetActive(false);
	}

	public void QuitGame()
	{
		Application.Quit();
	}

	//public void ResetMainMenu()
	//{
	//	SettingsPanel.SetActive(false);
	//	CreditsPanel.SetActive(false);
	//}

    public void RestartGame()
    {
        SceneManager.LoadScene(FIRST_PLAY_SCENE_ID);
    }

}

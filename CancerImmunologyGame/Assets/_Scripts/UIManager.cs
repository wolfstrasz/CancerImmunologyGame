using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : Singleton<UIManager>
{
	private const int FIRST_PLAY_SCENE_ID = 2;
	private bool available = true;

	[Header("UI Panels")]
	[SerializeField] private GameObject MainMenuPanel = null;
	[SerializeField] private GameObject WinScreenPanel = null;
	[SerializeField] private GameObject GameMenuPanel = null;
	[SerializeField] private GameObject SurveyPanel = null;
	[SerializeField] private string SurveyURL = "";

	public void WinScreen()
	{
		WinScreenPanel.SetActive(true);
	}

	public void OpenMainMenu()
	{
		WinScreenPanel.SetActive(false);
		MainMenuPanel.SetActive(true);
		GameMenuPanel.SetActive(false);
	}

	public void ClosePanels()
	{
		WinScreenPanel.SetActive(false);
		MainMenuPanel.SetActive(false);
		GameMenuPanel.SetActive(false);
		SurveyPanel.SetActive(false);
	}

	// Main Menu Functionality buttons
	public void Play()
	{
		if (!available) return;
		available = false;
		Debug.Log("Play");
		StartCoroutine(WaitToStart());
	}

	private IEnumerator WaitToStart()
	{
		yield return new WaitForSecondsRealtime(0.2f);
		available = true;
		MainMenuPanel.SetActive(false);
		WinScreenPanel.SetActive(false);
		GameMenuPanel.SetActive(false);

		SceneManager.LoadScene(FIRST_PLAY_SCENE_ID);
	}

	public void OpenSettings()
	{
		if (!available) return;
		// open settings
	}

	public void OpenCredits()
	{
		if (!available) return;
		// open credits
	}

	public void QuitGame()
	{
		if (!available) return;
		available = false;
		Debug.Log("Play");
		StartCoroutine(WaitToQuit());
	}

	private IEnumerator WaitToQuit()
	{
		yield return new WaitForSecondsRealtime(0.2f);
		Application.Quit();
	}
	//public void ResetMainMenu()
	//{
	//	SettingsPanel.SetActive(false);
	//	CreditsPanel.SetActive(false);
	//}


	public void OpenURL()
	{
		Application.OpenURL(SurveyURL);
	}


	public void RestartGame()
    {
		if (!available) return;
		available = false;
		Debug.Log("Play");
		StartCoroutine(WaitToStart());
    }



	public void OnOpenGameMenu()
	{
		Core.GameManagement.GameManager.Instance.RequestGamePause();
	}

	public void OnCloseGameMenu()
	{
		Core.GameManagement.GameManager.Instance.RequestGameUnpause();
	}

	public void UseAutoAim()
	{
		GlobalGameData.autoAim = true;
	}


	public void NoAutoAim()
	{
		GlobalGameData.autoAim = false;
	}
}

using UnityEngine;
using ImmunotherapyGame.Core;
using ImmunotherapyGame.Core.SystemInterfaces;
using ImmunotherapyGame.GameManagement;

namespace ImmunotherapyGame.UI
{
    public class MainMenu : Singleton<MainMenu>
    {
		[Header("Main Menu")]
		[SerializeField]
		private GameObject continueBtn = null;

		[SerializeField]
		private InterfaceControlPanel mainMenuPanel = null;
		[SerializeField]
		private InterfaceControlPanel newGamePromptPanel = null;
		[SerializeField]
		private InterfaceControlPanel creditsPanel = null;

		public bool Opened { get; set; }

		//private void Awake()
		//{
		//	continueBtn.SetActive(PlayerPrefs.HasKey("GameInProgress"));
		//	mainMenuPanel.Open();
		//}

		private void Start()
		{
			Opened = false;
		}

		public void RequestOpen()
		{
			continueBtn.SetActive(PlayerPrefs.HasKey("GameInProgress"));
			mainMenuPanel.Open();
			Opened = true;
		}

		// Main menu buttons functionality
		public void TryToStartNewGame()
		{
			if (continueBtn.activeInHierarchy)
			{
				newGamePromptPanel.Open();
			}
			else
			{
				StartNewGame();
			}
		}

		public void StartNewGame()
		{
			GameManager.Instance.OnStartNewGame();
			PlayerPrefs.SetInt("GameInProgress", 1);
			newGamePromptPanel.Close();
			LevelSelectScreen.Instance.Open();
		}


		public void ContinueGame()
		{
			LevelSelectScreen.Instance.Open();
		}

		public void OpenSettings()
		{
			SettingsManager.Instance.Open();
		}

		public void OpenCredits()
		{
			creditsPanel.Open();
		}

		public void Quit()
		{
			// Quit
			Application.Quit();
		}
	}
}

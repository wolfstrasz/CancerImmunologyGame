using UnityEngine;
using ImmunotherapyGame.Core;
using ImmunotherapyGame.Core.SystemInterfaces;

namespace ImmunotherapyGame.UI
{
    public class MainMenu : MonoBehaviour
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

		private void Awake()
		{
			continueBtn.SetActive(PlayerPrefs.HasKey("GameInProgress"));
			mainMenuPanel.Open();
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
			// Reset data
			foreach (IDataManager manager in GlobalGameData.dataManagers)
			{
				manager.ResetData();
			}

			foreach (IDataManager manager in GlobalGameData.dataManagers)
			{
				manager.SaveData();
			}

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
			// Save data
			foreach(IDataManager manager in GlobalGameData.dataManagers)
			{
				manager.SaveData();
			}

			// Quit
			Application.Quit();
		}
	}
}

using UnityEngine;
using ImmunotherapyGame.Core;
using ImmunotherapyGame.Core.SystemInterfaces;

namespace ImmunotherapyGame.UI
{
    public class MainMenu : MonoBehaviour
    {
		[Header("Main Menu")]
		[SerializeField]
		private GameObject continueButtonObj = null;

		[SerializeField]
		private InterfaceControlPanel mainMenuPanel = null;
		[SerializeField]
		private InterfaceControlPanel newGamePrompt = null;
		[SerializeField]
		private InterfaceControlPanel creditsPanel = null;

		private void Awake()
		{
			continueButtonObj.SetActive(PlayerPrefs.HasKey("GameInProgress"));
			mainMenuPanel.Open();
		}

		// Main menu buttons functionality
		public void TryToStartNewGame()
		{
			if (continueButtonObj.activeInHierarchy)
			{

				newGamePrompt.Open();
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

			newGamePrompt.Close();
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

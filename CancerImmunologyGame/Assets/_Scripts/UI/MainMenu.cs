using UnityEngine;
using ImmunotherapyGame.Core;
using ImmunotherapyGame.Core.SystemInterfaces;

namespace ImmunotherapyGame.UI
{
    public class MainMenu : MonoBehaviour
    {
		[SerializeField]
		private GameObject newGamePrompt = null;
		[SerializeField]
		private GameObject continueButtonObj = null;


		private void Awake()
		{
			continueButtonObj.SetActive(PlayerPrefs.HasKey("GameInProgress"));
		}

		public void TryToStartNewGame()
		{
			if (continueButtonObj.activeInHierarchy)
			{
				newGamePrompt.SetActive(true);
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

			// Open Level Selection
			LevelSelectScreen.Instance.Open();
		}


		public void ContinueGame()
		{
			// Open Level Selection
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

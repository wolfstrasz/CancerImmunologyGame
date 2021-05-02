using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ImmunotherapyGame.Core;

namespace ImmunotherapyGame.UI
{
    public class MainMenu : Singleton<MainMenu>
    {

		[SerializeField]
		private GameObject newGamePrompt = null;
		[SerializeField]
		private GameObject continueButtonObj = null;


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
			// Start new game
		}


		public void ContinueGame()
		{

		}

		public void OpenSettings()
		{
			SettingsManager.Instance.Open();
		}

		public void Quit()
		{
			// Save data
			// Quit
			Application.Quit();
		}
	}
}

using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

using ImmunotherapyGame.Core;
using ImmunotherapyGame.Core.SystemInterfaces;
using ImmunotherapyGame.LevelManagement;
using ImmunotherapyGame.GameManagement;

namespace ImmunotherapyGame
{
	namespace Loader {

		public class Loader : Singleton<Loader>
		{
			void Awake()
			{
				GlobalGameData.dataManagers = new List<IDataManager>(2);

				// Load Settings
				SettingsManager.Instance.Initialise();

				// Load Game Data
				Debug.Log("Loader: Game Manager");
				GameManager.Instance.Initialise();
				GameManager.Instance.LoadData();
				GlobalGameData.dataManagers.Add(GameManager.Instance);

				// Load Level Data
				Debug.Log("Loader: Level Manager");
				LevelManager.Instance.LoadData();
				GlobalGameData.dataManagers.Add(LevelManager.Instance);


				// call intro finish
				Debug.Log("Fading Logos");
				Intro.Instance.FadeLogos();

				Debug.Log("Loader finished");
			}

		

			internal void OnIntroFinished()
			{
				SceneManager.LoadScene("MainMenu");
			}
		}

	}
}
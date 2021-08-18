using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

using ImmunotherapyGame.Core;
using ImmunotherapyGame.Core.SystemInterfaces;
using ImmunotherapyGame.LevelManagement;
using ImmunotherapyGame.GameManagement;
using ImmunotherapyGame.CellpediaSystem;
using ImmunotherapyGame.UI;
using ImmunotherapyGame.UI.TopOverlay;
using ImmunotherapyGame.ImmunotherapyResearchSystem;
using ImmunotherapyGame.Audio;
using ImmunotherapyGame.LevelTasks;
using ImmunotherapyGame.Tutorials;
using ImmunotherapyGame.Abilities;

namespace ImmunotherapyGame.Loader
{
	public class Loader : MonoBehaviour
	{
		[SerializeField]
		private Intro intro = null;

		private bool InitialisationHasFinished { get; set; }

		void Awake()
		{
			StartCoroutine(InitialiseManagers());
		}

		private IEnumerator InitialiseManagers()
		{
			AudioManager.Instance.Initialise();
			InterfaceManager.Instance.Initialise();
			BackgroundMusic.Instance.Initialise();
			AbilityEffectManager.Instance.Initialise();

			List<IDataManager> allDataManagers = new List<IDataManager>(4);

			// Load Settings
			Debug.Log("Loader: Settings Manager");
			SettingsManager.Instance.Initialise();

			// Load Game Data
			Debug.Log("Loader: Game Manager");
			GameManager.Instance.Initialise();

			// Load Level Data
			Debug.Log("Loader: Level Manager");
			LevelManager.Instance.LoadData();
			allDataManagers.Add(LevelManager.Instance);

			// Load Tutorial Manager
			Debug.Log("Loader: Tutorial Manager");
			TutorialManager.Instance.LoadData();
			TutorialManager.Instance.Initialise();
			allDataManagers.Add(TutorialManager.Instance);

			// Load Cellpedia Data
			Debug.Log("Loader: Cellpedia");
			Cellpedia.Instance.LoadData();
			Cellpedia.Instance.Initialise();
			allDataManagers.Add(Cellpedia.Instance);

			// Load Research Advancement
			Debug.Log("Loader: Research Advancement");
			ImmunotherapyResearch.Instance.LoadData();
			ImmunotherapyResearch.Instance.Initialise();
			allDataManagers.Add(ImmunotherapyResearch.Instance);

			// Load UIs
			LevelTaskSystem.Instance.Initialise();
			TopOverlayUI.Instance.Initialise();
			PauseMenu.Instance.Initialise();

			// Assign managers to Game manager
			GameManager.DataManagers = allDataManagers;

			// Update
			InitialisationHasFinished = true;
			intro.LoadingFinished = true;
			Debug.Log("Loader: Loading finished");

			// call intro finish
			yield return null;
		}

		private void Update()
		{
			if (intro.HasFinished && InitialisationHasFinished)
			{
				Debug.Log("Loader: Opening Main Menu scene");
				Debug.Log("------------------------------------------------------------------");
				SceneManager.LoadScene("MainMenu");
			}
		}

		private void OnDisable()
		{
			StopAllCoroutines();
		}
	}

}

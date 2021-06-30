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
using ImmunotherapyGame.ResearchAdvancement;
using ImmunotherapyGame.Audio;


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

			GlobalGameData.dataManagers = new List<IDataManager>(4);

			// Load Settings
			Debug.Log("Loader: Settings Manager");
			SettingsManager.Instance.Initialise();

			// Load Game Data
			Debug.Log("Loader: Game Manager");
			GameManager.Instance.LoadData();
			GameManager.Instance.Initialise();
			GlobalGameData.dataManagers.Add(GameManager.Instance);

			// Load Level Data
			Debug.Log("Loader: Level Manager");
			LevelManager.Instance.LoadData();
			GlobalGameData.dataManagers.Add(LevelManager.Instance);

			// Load Cellpedia Data
			Debug.Log("Loader: Cellpedia");
			Cellpedia.Instance.LoadData();
			Cellpedia.Instance.Initialise();
			GlobalGameData.dataManagers.Add(Cellpedia.Instance);

			// Load Research Advancement
			Debug.Log("Loader: Research Advancement");
			ResearchAdvancementSystem.Instance.LoadData();
			ResearchAdvancementSystem.Instance.Initialise();
			GlobalGameData.dataManagers.Add(ResearchAdvancementSystem.Instance);

			// Load UIs
			TopOverlayUI.Instance.Initialise();
			PauseMenu.Instance.Initialise();

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

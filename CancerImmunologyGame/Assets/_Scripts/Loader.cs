using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

using ImmunotherapyGame.Core;
using ImmunotherapyGame.Core.SystemInterfaces;
using ImmunotherapyGame.LevelManagement;
using ImmunotherapyGame.GameManagement;
using ImmunotherapyGame.CellpediaSystem;


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
			GlobalGameData.dataManagers = new List<IDataManager>(3);

			// Load Settings
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

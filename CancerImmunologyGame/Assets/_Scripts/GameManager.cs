using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using Player;
using Tutorials;
using Bloodflow;

namespace Core
{
	public class GameManager : Singleton<GameManager>
	{
		// Game state definition through function
		private delegate void StateFunction();
		StateFunction StateUpdate;
		StateFunction StateFixedUpdate;
		private bool isLevelInitialised = false;
		
		void Update()
		{
			StateUpdate();
		}

		void FixedUpdate()
		{
			StateFixedUpdate();
		}

		public void Initialise()
		{
			Debug.Log("Initialise Game Manager");
			Time.timeScale = 1.0f;
			SceneManager.activeSceneChanged += OnActiveSceneChanged;

			SceneManager.sceneLoaded += OnSceneLoaded;
			StateUpdate = LevelMainMenu;
			StateFixedUpdate = NoLevelFixedRunning;

			GlobalGameData.gameplaySpeed = 1.0f;
			GlobalGameData.gameSpeed = 1.0f;
			GlobalGameData.isGameplayPaused = false;
			GlobalGameData.isInitialised = true;
			CellpediaUI.Cellpedia.Instance.Initialise();
		}


		public void OnActiveSceneChanged(Scene currentScene, Scene nextScene)
		{
			StateUpdate = LevelLoading;
			StateFixedUpdate = NoLevelFixedRunning;
		}

		private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
		{
			if (scene.buildIndex == 0) return;

			if (scene.buildIndex == 1)
			{
				StateUpdate = LevelMainMenu;
				UIManager.Instance.OpenMainMenu();
				return;
			}

			StartCoroutine(InitialiseLevel());
		}

		private IEnumerator InitialiseLevel()
		{
			SmoothCamera.Instance.Reset();
			UIManager.Instance.ClosePanels();
			GlobalGameData.ResetObjectPool();

			PlayerController.Instance.Initialise();
			TutorialManager.Instance.Initialise();
			BloodflowController.Instance.Initialise();

			isLevelInitialised = true;
			yield return null;
		}

		private void LevelMainMenu()
		{
			//Debug.Log("LevelMainMenu");
		}

		private void LevelLoading()
		{
			//Debug.Log("LevelLoading");
			if (isLevelInitialised)
			{
				Debug.Log("Level Loaded");
				StateUpdate = OnLevelEnterRunning;
			}
		}

		private void OnLevelEnterRunning()
		{
			GlobalGameData.isGameplayPaused = false;
			StateUpdate = LevelRunning;
			StateFixedUpdate = LevelFixedRunning;
		}

		private void LevelRunning()
		{
			PlayerController.Instance.OnUpdate();
			foreach (KillerCell kc in GlobalGameData.KillerCells)
			{
				kc.OnUpdate();
			}

			foreach (Cancer cancer in GlobalGameData.Cancers)
			{
				cancer.OnUpdate();
			}

			if (GlobalGameData.Cancers.Count == 0)
			{
				OnLevelFinished();
				return;
			}


#if !REMOVE_PLAYER_DEBUG
			if (Input.GetKeyDown(KeyCode.R))
			{
				PlayerUI.Instance.ActivateImmunotherapyPanel();
			}
			if (Input.GetKeyDown(KeyCode.Z))
			{
				CellpediaUI.Cellpedia.Instance.UnlockCellDescription(CellpediaCells.DENDRITIC);
			}
			if (Input.GetKeyDown(KeyCode.X))
			{
				CellpediaUI.Cellpedia.Instance.UnlockCellDescription(CellpediaCells.REGULATORY);
			}
			if (Input.GetKeyDown(KeyCode.C))
			{
				CellpediaUI.Cellpedia.Instance.UnlockCellDescription(CellpediaCells.CANCER);
			}
			if (Input.GetKeyDown(KeyCode.V))
			{
				CellpediaUI.Cellpedia.Instance.UnlockCellDescription(CellpediaCells.THELPER);
			}
#endif

		}

		private void LevelFixedRunning()
		{
			PlayerController.Instance.OnFixedUpdate();
			BloodflowController.Instance.OnFixedUpdate();
			foreach (KillerCell kc in GlobalGameData.KillerCells)
			{
				kc.OnFixedUpdate();
			}
		}

		private void NoLevelFixedRunning()
		{

		}

		private void OnEnterLevelPause()
		{
			GlobalGameData.isGameplayPaused = true;
			StateUpdate = LevelPaused;
			StateFixedUpdate = NoLevelFixedRunning;
		}

		private void LevelPaused()
		{

		}

		private void OnLevelFinished()
		{
			GlobalGameData.isGameplayPaused = true;
			StateUpdate = LevelPaused;
			StateFixedUpdate = NoLevelFixedRunning;
			UIManager.Instance.WinScreen();
		}


	}
}

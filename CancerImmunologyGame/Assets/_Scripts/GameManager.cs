using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using Player;
using Tutorials;

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
			Time.timeScale = 1.0f;
			SceneManager.activeSceneChanged += OnActiveSceneChanged;

			SceneManager.sceneLoaded += OnSceneLoaded;
			StateUpdate = LevelMainMenu;
			StateFixedUpdate = NoLevelFixedRunning;

			GlobalGameData.gameplaySpeed = 1.0f;
			GlobalGameData.gameSpeed = 1.0f;
			GlobalGameData.isGameplayPaused = false;
			GlobalGameData.isInitialised = true;
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
			UIManager.Instance.ClosePanels();
			GlobalGameData.RestObjectPool();
			GlobalGameData.Cancers.Clear();
			GlobalGameData.Cancers.AddRange(FindObjectsOfType<Cancer>());

			PlayerController.Instance.Initialise();
			TutorialManager.Instance.Initialise();

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

			if (GlobalGameData.Cancers.Count == 0)
			{
				OnLevelFinished();
				return;
			}
			for (int i =0; i< GlobalGameData.Cancers.Count; ++i)
			{
				GlobalGameData.Cancers[i].OnUpdate();
			}
		}

		private void LevelFixedRunning()
		{
			PlayerController.Instance.OnFixedUpdate();
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

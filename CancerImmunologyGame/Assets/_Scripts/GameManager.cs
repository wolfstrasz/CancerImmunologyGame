using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using Player;

namespace Core
{
	public class GameManager : Singleton<GameManager>
	{
		// Game state definition through function
		private delegate void StateFunction();
		StateFunction StateUpdate;
		StateFunction StateFixedUpdate;

		private bool isLevelInitialised = false;
		public void Initialise()
		{
			Time.timeScale = 1.0f;
			SceneManager.activeSceneChanged += OnActiveSceneChanged;

			SceneManager.sceneLoaded += OnSceneLoaded;
			StateUpdate = LevelMainMenu;
			StateFixedUpdate = NoLevelFixedRunning;

			GlobalGameData.gameplaySpeed = 1.0f;
			GlobalGameData.gameSpeed = 1.0f;
			GlobalGameData.isPaused = false;
			GlobalGameData.isInitialised = true;
		}


		public void OnActiveSceneChanged(Scene currentScene, Scene nextScene)
		{
			StateUpdate = LevelLoading;
			StateFixedUpdate = NoLevelFixedRunning;
		}

		private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
		{
			if (scene.buildIndex == 1)
			{
				StateUpdate = LevelMainMenu;
				UIManager.Instance.OpenMainMenu();
				return;
			}

			StartCoroutine(InitialiseLevel());
			//GlobalData.levelData = LevelManager.Instance.GetLevelData(scene.buildIndex);
			//GlobalData.SetPathTileGallery(scene.buildIndex);

			//GUIManager.Instance.LevelSceneInstance();
			//BoardManager.Instance.GenerateLevel();
			//PoolManager.Instance.Initialise();
			//LevelControl.Instance.Initialise();
			//GUIWaveInfoDisplayer.Instance.Reset();
			//GUIWaveInfoDisplayer.Instance.Hide();
			//GUITowerInfoDisplayer.Instance.Hide();

			//loadingSceneFinished = true;
		}

		private IEnumerator InitialiseLevel()
		{
			PlayerController.Instance.Initialise();
			yield return null;
		}

		private void LevelMainMenu()
		{

		}

		private void LevelLoading()
		{
			if (isLevelInitialised)
			{
				StateUpdate = LevelRunning;
				StateFixedUpdate = LevelFixedRunning;
			}
		}

		private void LevelFixedRunning()
		{
			PlayerController.Instance.OnFixedUpdate();
		}

		private void NoLevelFixedRunning()
		{

		}

		private void LevelRunning()
		{

		}


		public void LevelPaused()
		{

		}


	}
}

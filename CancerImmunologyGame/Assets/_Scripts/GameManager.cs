using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

namespace Core
{
	public class GameManager : Singleton<GameManager>
	{
		// Game state definition through function
		private delegate void StateFunction();
		StateFunction StateUpdate;

		public void Initialise()
		{
			Time.timeScale = 1.0f;
			//SceneManager.activeSceneChanged += OnActiveSceneChanged;

			SceneManager.sceneLoaded += OnSceneLoaded;
			StateUpdate = LevelMainMenu;

			GlobalGameData.gameplaySpeed = 1.0f;
			GlobalGameData.gameSpeed = 1.0f;
			GlobalGameData.isPaused = false;
			GlobalGameData.isInitialised = true;
		}


		//public void OnActiveSceneChanged(Scene currentScene, Scene nextScene)
		//{
		//		StateController.State = new LevelLoadingState(StateController);
		//}

		private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
		{
			if (scene.buildIndex == 1)
			{
				StateUpdate = LevelMainMenu;
				UIManager.Instance.OpenMainMenu();
				return;
			}


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



		public void LevelMainMenu()
		{

		}

		public void LevelLoading()
		{

		}

		public void LevelRunning()
		{

		}


		public void LevelPaused()
		{

		}


	}
}

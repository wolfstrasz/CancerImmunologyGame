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

		private float gameplaySpeed = 1.0f;
		private float gameSpeed = 1.0f;
		private bool isPaused = false;

		void Start()
		{
			gameplaySpeed = 1.0f;
			gameSpeed = 1.0f;
			isPaused = false;
			Time.timeScale = 1.0f;
			//SceneManager.activeSceneChanged += OnActiveSceneChanged;
			SceneManager.sceneLoaded += OnSceneLoaded;
			StateUpdate = LevelMainMenu;
		}


		//public void OnActiveSceneChanged(Scene currentScene, Scene nextScene)
		//{
		//		StateController.State = new LevelLoadingState(StateController);
		//}

		private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
		{
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using Player;
using Tutorials;
using Bloodflow;
using Cancers;

namespace Core
{
	namespace GameManagement
	{
		public class GameManager : Singleton<GameManager>
		{
			private GameStateController stateController = new GameStateController();

			// Game state definition through function
			private delegate void StateFunction();
			StateFunction StateUpdate;
			StateFunction StateFixedUpdate;
			private bool isLevelInitialised = false;

			void Update()
			{
				stateController.OnUpdate();
			}

			void FixedUpdate()
			{
				stateController.OnFixedUpdate();
			}

			public void Initialise()
			{

				Time.timeScale = 1.0f;
				SceneManager.activeSceneChanged += OnActiveSceneChanged;

				SceneManager.sceneLoaded += OnSceneLoaded;
				//StateUpdate = LevelMainMenu;
				//StateFixedUpdate = NoLevelFixedRunning;

				GlobalGameData.gameplaySpeed = 1.0f;
				GlobalGameData.gameSpeed = 1.0f;
				GlobalGameData.isGameplayPaused = false;
				GlobalGameData.isInitialised = true;
			}


			internal bool sceneLoaded = false;
			public void OnActiveSceneChanged(Scene currentScene, Scene nextScene)
			{
				if (nextScene.buildIndex == 0) return;
				if (nextScene.buildIndex == 1)
				{
					stateController.SetState(new MainMenuState(stateController));
					UIManager.Instance.OpenMainMenu();
					return;
				}

				sceneLoaded = false;
				stateController.SetState(new LoadState(stateController));
			}

			private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
			{
				sceneLoaded = true;
			}
		}
	}
}

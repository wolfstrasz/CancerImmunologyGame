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

			[SerializeField]
			internal bool sceneLoaded = false;
			[SerializeField]
			private GameStateController stateController = new GameStateController();

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

				GlobalGameData.gameplaySpeed = 1.0f;
				GlobalGameData.gameSpeed = 1.0f;
				GlobalGameData.isInitialised = true;
			}

			public void InitialiseDebugScene()
			{

				Time.timeScale = 1.0f;

				GlobalGameData.gameplaySpeed = 1.0f;
				GlobalGameData.gameSpeed = 1.0f;
				GlobalGameData.isInitialised = true;

				sceneLoaded = true;
				stateController.SetState(new LoadState(stateController));

			}
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

			public void RequestGameplayPause()
			{
				stateController.AddState(new GameplayPauseState(stateController));
			}

			public void RequestGameplayUnpause(string callerName)
			{
				stateController.RemoveCurrentState(callerName);
			}

			public void RequestGamePause()
			{
				stateController.AddState(new PauseState(stateController));
			}

			public void RequestGameUnpause(string callerName)
			{
				stateController.RemoveCurrentState(callerName);
			}
		}
	}
}

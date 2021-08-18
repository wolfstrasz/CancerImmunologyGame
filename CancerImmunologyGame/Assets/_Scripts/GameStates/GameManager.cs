using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections.Generic;

using ImmunotherapyGame.Core;
using ImmunotherapyGame.Core.SystemInterfaces;

namespace ImmunotherapyGame
{
	namespace GameManagement
	{
		public class GameManager : Singleton<GameManager>
		{
			[SerializeField] internal bool sceneLoaded = false;
			[SerializeField] internal GameStateController stateController = new GameStateController();
			[SerializeField] private bool isTestScene = false;
			public static List<IDataManager> DataManagers;

			void Start() { Initialise(isTestScene); }

			void Update() { stateController.OnUpdate(); }

			void FixedUpdate() { stateController.OnFixedUpdate(); }

			private void OnEnable()
			{
				SceneManager.activeSceneChanged += OnActiveSceneChanged;
				SceneManager.sceneLoaded += OnSceneLoaded;
			}

			private void OnDisable()
			{
				SceneManager.activeSceneChanged -= OnActiveSceneChanged;
				SceneManager.sceneLoaded -= OnSceneLoaded;
			}

			public void Initialise(bool testScene = false)
			{
				Debug.Log("Game Manager: Initialise");
				//if (testScene)
				//{
				//	Debug.Log("Game Manager: Initialise Test Scene");
				//	sceneLoaded = true;
				//	Debug.Log("Game Manager -> Test Scene: Set Play State");
				//	stateController.SetState(new PlayState(stateController));
				//}
			}

			public void OnActiveSceneChanged(Scene currentScene, Scene nextScene)
			{
				if (nextScene.buildIndex >= 2)
				{
					sceneLoaded = false;
					Debug.Log("GameManager -> OnActiveSceneChanged: Set Load State");
					stateController.SetState(new LoadState(stateController));
				} 
				else if (nextScene.buildIndex == 1)
				{
					Debug.Log("GameManager -> OnActiveSceneChanged: Set MainMenu State");
					stateController.SetState(new MainMenuState(stateController));
				}
				else 
				{
					Debug.Log("GameManager -> OnActiveSceneChanged: Set Empty State");
					stateController.SetState(new EmptyState(stateController));
				}
			}

			private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
			{
				Debug.Log("Scene Loaded ------------------");
				sceneLoaded = true;
			}


			public bool RequestGameStatePause(GameStatePauseRequestType requestType, GameObject callerObject)
			{
				if (requestType == GameStatePauseRequestType.GAMEPLAY)
				{
					return stateController.AddPauseState(callerObject, new GameplayPauseState(stateController));
				}
				else if (requestType == GameStatePauseRequestType.FULL)
				{
					return stateController.AddPauseState(callerObject, new PauseState(stateController));
				}

				return true;
			}

			public bool RequestGameStateUnpause (GameObject callerObject)
			{
				return stateController.RemovePauseState(callerObject);
			}
			

			public void OnStartNewGame()
			{
				// Reset data
				foreach (IDataManager manager in DataManagers)
				{
					manager.ResetData();
				}

				foreach (IDataManager manager in DataManagers)
				{
					manager.SaveData();
				}
			}

			public void SaveData()
			{
				// Save all data from level
				for (int i = 0; i < DataManagers.Count; ++i)
				{
					DataManagers[i].SaveData();
				}
			}
		}

		public enum GameStatePauseRequestType { NONE, GAMEPLAY, FULL}
	}
}
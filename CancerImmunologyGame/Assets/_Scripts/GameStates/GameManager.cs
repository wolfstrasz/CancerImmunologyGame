using UnityEngine.SceneManagement;
using UnityEngine;
using ImmunotherapyGame.Core;
using ImmunotherapyGame.Core.SystemInterfaces;

namespace ImmunotherapyGame
{
	namespace GameManagement
	{
		public class GameManager : Singleton<GameManager>, IDataManager
		{
			[SerializeField]
			private CurrentGameData data = null;

			[SerializeField]
			internal bool sceneLoaded = false;
			[SerializeField]
			internal GameStateController stateController = new GameStateController();
			[SerializeField]
			private bool isTestScene = false;

			void Start() { Initialise(isTestScene); }

			void Update() { stateController.OnUpdate(); }

			void FixedUpdate() { stateController.OnFixedUpdate(); }

			public void Initialise(bool testScene = false)
			{
				Debug.Log("Game Manager: Initialise");

				GlobalGameData.gameplaySpeed = 1.0f;
				GlobalGameData.gameSpeed = 1.0f;
				SceneManager.activeSceneChanged += OnActiveSceneChanged;
				SceneManager.sceneLoaded += OnSceneLoaded;

				if (testScene)
				{
					Debug.Log("Game Manager: Initialise Test Scene");
					sceneLoaded = true;
					stateController.SetState(new PlayState(stateController));
				}
			}

			public void OnActiveSceneChanged(Scene currentScene, Scene nextScene)
			{
				if (nextScene.buildIndex >= 2)
				{
					sceneLoaded = false;
					stateController.SetState(new LoadState(stateController));
				} else
				{
					stateController.SetState(new EmptyState(stateController));

				}
			}

			private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
			{
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



			public void LoadData()
			{
				//LoadCurrentGameData();
			}

			public void SaveData()
			{
				//SaveCurrentGameData();
			}

			public void ResetData()
			{
				//ResetCurrentGameData();
			}


			
		}

		public enum GameStatePauseRequestType { NONE, GAMEPLAY, FULL}
	}
}
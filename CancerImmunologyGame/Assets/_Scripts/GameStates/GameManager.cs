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
			private GameStateController stateController = new GameStateController();
			[SerializeField]
			private bool isTestScene = false;


			void Start()
			{
				if (isTestScene)
					InitialiseTestScene();
			}

			void Update() { stateController.OnUpdate(); }

			void FixedUpdate() { stateController.OnFixedUpdate(); }

			public void Initialise()
			{
				SceneManager.activeSceneChanged += OnActiveSceneChanged;
				SceneManager.sceneLoaded += OnSceneLoaded;

				GlobalGameData.gameplaySpeed = 1.0f;
				GlobalGameData.gameSpeed = 1.0f;
				GlobalGameData.isInitialised = true;
			}

			public void InitialiseTestScene()
			{

				GlobalGameData.gameplaySpeed = 1.0f;
				GlobalGameData.gameSpeed = 1.0f;
				GlobalGameData.isInitialised = true;

				sceneLoaded = true;
				stateController.SetState(new PlayTestState(stateController));
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



			private void LoadCurrentGameData()
			{

			}


			private void SaveCurrentGameData()
			{

			}

			private void ResetCurrentGameData()
			{

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




			public void LoadData()
			{
				LoadCurrentGameData();
			}

			public void SaveData()
			{
				SaveCurrentGameData();
			}

			public void ResetData()
			{
				ResetCurrentGameData();
			}
		}
	}
}
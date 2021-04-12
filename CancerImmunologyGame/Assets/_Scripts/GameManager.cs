using UnityEngine.SceneManagement;
using UnityEngine;


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
			[SerializeField]
			private bool isTestScene = false;
			[SerializeField]
			private float gameSpeed = 1.2f;
			void Start()
			{
				if (isTestScene) 
					InitialiseTestScene();
			}

			void Update() { stateController.OnUpdate(); }

			void FixedUpdate() { stateController.OnFixedUpdate(); }

			public void Initialise()
			{
				Time.timeScale = gameSpeed;
				SceneManager.activeSceneChanged += OnActiveSceneChanged;
				SceneManager.sceneLoaded += OnSceneLoaded;

				GlobalGameData.gameplaySpeed = gameSpeed;
				GlobalGameData.gameSpeed = gameSpeed;
				GlobalGameData.isInitialised = true;
			}

			public void InitialiseTestScene()
			{
				Time.timeScale = gameSpeed;

				GlobalGameData.gameplaySpeed = gameSpeed;
				GlobalGameData.gameSpeed = gameSpeed;
				GlobalGameData.isInitialised = true;

				sceneLoaded = true;
				stateController.SetState(new PlayTestState(stateController));
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

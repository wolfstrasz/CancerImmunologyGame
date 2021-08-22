using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using TMPro;
using ImmunotherapyGame.Core;
using ImmunotherapyGame.UI;
using ImmunotherapyGame.UI.TopOverlay;

using ImmunotherapyGame.GameManagement;
using ImmunotherapyGame.Core.SystemInterfaces;
using ImmunotherapyGame.SaveSystem;

using UnityEngine.InputSystem;

namespace ImmunotherapyGame.Tutorials
{
	public class TutorialManager : Singleton<TutorialManager>, IDataManager
	{

		[Header("Data")] 
		[SerializeField] private TutorialLogsData data;
		[SerializeField] private TopOverlayButtonData inGameUIButtonData;
		[SerializeField] [ReadOnly] private bool isFeatureUnlocked;
		[SerializeField] [ReadOnly] SerializableTutorialLogsData savedData = null;

		[Header("UI Elements Linking")]
		[SerializeField] private InterfaceControlPanel textDisplayPanel = null;
		[SerializeField] private InterfaceControlPanel logDisplayPanel = null;
		[SerializeField] private InterfaceControlPanel tutorialLogsPanel = null;

		[SerializeField] private Image logImage = null;
		[SerializeField] private TMP_Text logTitle = null;
 		[SerializeField] private TMP_Text tutorialTxt = null;
		[SerializeField] private GameObject skipTextButton = null;

		[SerializeField] private List<TutorialStage> LevelTutorialStages = new List<TutorialStage>();

		// Attributes
		internal delegate void OnSkipDelegate();
		internal OnSkipDelegate onSkipDelegate;

		// Input handling
		private PlayerControls playerControls;

		// Protected methods
		protected override void Awake()
		{
			base.Awake();
			playerControls = new PlayerControls();

			// Update skip text
#if UNITY_WEBGL
			string displayStr = "Hold [Spacebar] | [LT] to skip. ";

#else
			List<string> bindings = Utils.GetAllKeybindsStrings(playerControls.Systems.SkipText);

			string displayStr = "Hold [";

			if (bindings.Count != 0)
			{
				displayStr += bindings[0];
				for (int i = 1; i < bindings.Count; ++i)
				{
					displayStr += "] | [" + bindings[i];
				}
			}
			displayStr += "] to skip. ";

#endif
			skipTextButton.GetComponent<TMP_Text>().text = displayStr;
			playerControls.Systems.SkipText.Enable();

		}

		public void ClearData()
		{
			LevelTutorialStages.Clear();
		}
		public void ClearData(Scene currentScene, Scene nextScene)
		{
			LevelTutorialStages.Clear();
		}

		public void UnlockFeature()
		{
			isFeatureUnlocked = true;
			data.isSystemUnlocked = true;
			inGameUIButtonData.PingUnlockStatus(isFeatureUnlocked);
		}

		public void Initialise()
		{
			isFeatureUnlocked = data.isSystemUnlocked;
			inGameUIButtonData.PingUnlockStatus(isFeatureUnlocked);
		}

		// UI  Buttons callback
		public void OpenLogsView()
		{
			tutorialLogsPanel.Open();
		}

		public void CloseLogsView()
		{
			tutorialLogsPanel.Close();
		}

		public void OpenLogsView(InputAction.CallbackContext context)
		{
			Debug.Log("Button Call for Cellpedia");
			if (!isFeatureUnlocked) return; // TODO: remove hidden state

			if (tutorialLogsPanel.IsOpened)
			{
				Debug.Log("Try to close Cellpedia");

				tutorialLogsPanel.Close();
			}
			else
			{
				Debug.Log("Try to open Cellpedia");

				tutorialLogsPanel.Open();
			}
		}

		protected void OnEnable()
		{
			playerControls.Enable();
			playerControls.Systems.SkipText.started += OnSkipPressed;

			// Tutorial display bindings
			textDisplayPanel.onCloseInterface += OnCloseTextDisplayPanel;
			logDisplayPanel.onCloseInterface += OnCloseLogDisplayPanel;

			// Logs panel binding
			inGameUIButtonData.onOpenMenu += OpenLogsView;
			playerControls.Systems.LogsMenu.started += OpenLogsView;

			SceneManager.activeSceneChanged += ClearData;
		}

		protected void OnDisable()
		{
			playerControls.Disable();
			playerControls.Systems.SkipText.started -= OnSkipPressed;

			// Tutorial display bindings
			textDisplayPanel.onCloseInterface -= OnCloseTextDisplayPanel;
			logDisplayPanel.onCloseInterface -= OnCloseLogDisplayPanel;

			// Logs panel binding
			inGameUIButtonData.onOpenMenu -= OpenLogsView;
			playerControls.Systems.LogsMenu.started -= OpenLogsView;

			SceneManager.activeSceneChanged -= ClearData;


		}


		// Public methods
		public void LoadLevelTutorials()
		{
			LevelTutorialStages = new List<TutorialStage>();
			LevelTutorialStages.AddRange(FindObjectsOfType<TutorialStage>(false));

			foreach (TutorialStage ts in LevelTutorialStages)
			{
				ts.InitialiseStage();
			}
		}

		public void OnUpdate()
		{
			
			for (int i = 0; i < LevelTutorialStages.Count; ++i)
			{
				if (!LevelTutorialStages[i].IsFinished)
				{
					LevelTutorialStages[i].OnUpdate();
				}
			}
		}

		// Panels opening
		internal void DisplayTextPanel(string text)
		{
			tutorialTxt.text = text;
			textDisplayPanel.Open();
		}

		internal void DisplayLogPanel(TutorialLog log)
		{
			logImage.sprite = log.imageSprite;
			logTitle.text = log.title;
			logDisplayPanel.Open();
		}


		/// <summary>
		/// ////////////////////////////////////////////
		/// 
		/// </summary>
		internal void DisplaySkipButton()
		{
			skipTextButton.SetActive(true);
		}

		public void OnSkipPressed(InputAction.CallbackContext context)
		{
			if (skipTextButton.activeInHierarchy)
			{
				if (textDisplayPanel.IsOpened)
				{
					textDisplayPanel.Close();
				}
			} 
			else if (logDisplayPanel.IsOpened)
			{
				logDisplayPanel.Close();
			}
		}

		// Interface Object callbacks
		internal void OnCloseTextDisplayPanel()
		{
			if (onSkipDelegate != null)
			{
				skipTextButton.SetActive(false);
				onSkipDelegate();
			}
		}

		internal void OnCloseLogDisplayPanel()
		{
			if (onSkipDelegate != null)
			{
				onSkipDelegate();
			}
		}


		// Event requests gameplay pause
		internal void RequestPause()
		{
			GameManager.Instance.RequestGameStatePause(GameStatePauseRequestType.GAMEPLAY, gameObject);
		}

		internal bool RequestUnpause()
		{
			return GameManager.Instance.RequestGameStateUnpause(gameObject);
		}


		// Data management
		public void LoadData()
		{
			// Try to load levels
			savedData = SaveManager.Instance.LoadData<SerializableTutorialLogsData>();

			if (savedData == null)
			{
				Debug.Log("No previous saved data found. Creating new level data save.");
				ResetData();
				SaveData();
			}
			else
			{
				savedData.CopyTo(data);
			}

			isFeatureUnlocked = data.isSystemUnlocked;
			inGameUIButtonData.PingUnlockStatus(isFeatureUnlocked);
		}

		public void SaveData()
		{
			savedData = new SerializableTutorialLogsData(data);
			SaveManager.Instance.SaveData<SerializableTutorialLogsData>(savedData);
		}

		public void ResetData()
		{
			data.ResetData();
			inGameUIButtonData.PingUnlockStatus(false);
		}
	}


}
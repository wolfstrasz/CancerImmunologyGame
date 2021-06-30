using System.Collections.Generic;
using UnityEngine;
using TMPro;
using ImmunotherapyGame.Core;
using ImmunotherapyGame.UI;
using ImmunotherapyGame.GameManagement;

using UnityEngine.InputSystem;

namespace ImmunotherapyGame.Tutorials
{
	public class TutorialManager : Singleton<TutorialManager>
	{

		// Input handling
		private PlayerControls playerControls;
		private InputAction SkipAction = null;

		[Header("UI Elements Linking")]
		[SerializeField]
		private InterfaceControlPanel textDisplayPanel = null;
		[SerializeField]
		private TMP_Text tutorialTxt = null;
		[SerializeField]
		private GameObject skipTextButton = null;

		[SerializeField]
		private List<TutorialStage> StoryTutorials = new List<TutorialStage>();

		internal delegate void OnSkipDelegate();
		internal OnSkipDelegate onSkipDelegate;
		internal bool IsGameplayPaused => textDisplayPanel.IsOpened;

		// Protected methods
		protected override void Awake()
		{
			base.Awake();

			playerControls = new PlayerControls();
			SkipAction = playerControls.Systems.Skip;

			// Update skip text
			List<string> bindings = Utils.GetAllKeybindsStrings(SkipAction);
			if (bindings.Count == 0) return;
			string displayStr = "Hold [" + bindings[0];

			for (int i = 1; i < bindings.Count; ++i)
			{
				displayStr += "] | [" + bindings[i];
			}
			displayStr += "] to skip. ";
			skipTextButton.GetComponent<TMP_Text>().text = displayStr;
			SkipAction.Enable();

		}

		protected void OnEnable()
		{
			playerControls.Enable();
			SkipAction.started += OnSkipPressed;
			textDisplayPanel.onCloseInterface += OnClosePauseInterfacePanel;
		}

		protected void OnDisable()
		{
			SkipAction.started -= OnSkipPressed;
			textDisplayPanel.onCloseInterface -= OnClosePauseInterfacePanel;
		}


		// Public methods
		public void Initialise()
		{
			StoryTutorials = new List<TutorialStage>();
			StoryTutorials.AddRange(FindObjectsOfType<TutorialStage>(false));

			foreach (TutorialStage ts in StoryTutorials)
			{
				ts.InitialiseStage();
			}
		}

		public void OnUpdate()
		{
			foreach (TutorialStage ts in StoryTutorials)
			{
				if (!ts.IsFinished)
				{
					ts.OnUpdate();
				}
			}
		}

	
		internal void DisplayText(string text)
		{
			tutorialTxt.text = text;
			textDisplayPanel.Open();
		}

		internal void DisplaySkipButton()
		{
			skipTextButton.SetActive(true);
		}

		public void OnSkipPressed(InputAction.CallbackContext context)
		{
			if (skipTextButton.activeInHierarchy)
			{
				textDisplayPanel.Close();
			}
		}

		// Inteface Object callbacks
		public void OnClosePauseInterfacePanel()
		{
			if (onSkipDelegate != null)
			{
				skipTextButton.SetActive(false);
				onSkipDelegate();
			}
		}

		// Event requests gameplay pause
		internal void RequestPause()
		{
			GameManager.Instance.RequestGameStatePause(GameStatePauseRequestType.GAMEPLAY, gameObject);
		}

		internal void RequestUnpause()
		{
			GameManager.Instance.RequestGameStateUnpause(gameObject);
		}




	}


}
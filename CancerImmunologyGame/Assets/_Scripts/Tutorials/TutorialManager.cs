using System.Collections.Generic;
using UnityEngine;
using TMPro;
using ImmunotherapyGame.Core;
using ImmunotherapyGame.UI;

using UnityEngine.InputSystem;

namespace ImmunotherapyGame.Tutorials
{
	public class TutorialManager : Singleton<TutorialManager>
	{

		[SerializeField]
		private InterfaceControlPanel interfacePanel = null;
		// Input handling
		private PlayerControls playerControls;
		private InputAction SkipAction = null;

		[Header("UI Elements Linking")]
		[SerializeField]
		private GameObject tutorialPanel = null;
		[SerializeField]
		private TMP_Text tutorialTxt = null;
		[SerializeField]
		private GameObject skipTextButton = null;

		[SerializeField]
		private List<TutorialStage> StoryTutorials = new List<TutorialStage>();

		internal delegate void OnSkipDelegate();
		internal OnSkipDelegate onSkipDelegate;
		internal bool IsGameplayPaused => interfacePanel.IsOpened;

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
			interfacePanel.onCloseInterface += OnClosePauseInterfacePanel;
		}

		protected void OnDisable()
		{
			SkipAction.started -= OnSkipPressed;
			interfacePanel.onCloseInterface -= OnClosePauseInterfacePanel;
		}


		// Public methods
		public void LoadLevelTutorials()
		{
			StoryTutorials = new List<TutorialStage>();
			StoryTutorials.AddRange(FindObjectsOfType<TutorialStage>());

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

	
		// Internal methods for text displaying
		internal void HideText()
		{
			skipTextButton.SetActive(false);
			tutorialPanel.SetActive(false);
		}

		internal void DisplayText(string text)
		{
			tutorialTxt.text = text;
			tutorialPanel.SetActive(true);
		}

		internal void DisplaySkipButton()
		{
			skipTextButton.SetActive(true);
		}


		// Event requests gameplay pause
		internal void OpenPauseInterfacePanel()
		{
			interfacePanel.Open();
		}


		internal void ClosePauseIntefacePanel()
		{
			interfacePanel.Close();
		}

		public void OnSkipPressed(InputAction.CallbackContext context)
		{
			if (skipTextButton.activeInHierarchy)
				interfacePanel.Close();
		}

		// Inteface Object callbacks
		public void OnClosePauseInterfacePanel()
		{
			if (onSkipDelegate != null)
			{
				onSkipDelegate();
			}
		}

	}


}
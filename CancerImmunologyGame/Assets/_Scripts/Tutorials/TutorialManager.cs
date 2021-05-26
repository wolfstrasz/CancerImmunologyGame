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
		private InterfaceControlPanel interfaceObject = null;
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
		internal bool IsGameplayUnpaused = true;

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

			//interfaceObject.interfaceObjectOwner = gameObject;
			//interfaceObject.interfaceInitialMenuNode = null;
		}

		protected void OnEnable()
		{
			playerControls.Enable();
			SkipAction.started += OnSkipPressed;
			//interfaceObject.openInterface += OpenInterfaceObject;
			//interfaceObject.closeInterface += CloseInterfaceObject;
		}

		protected void OnDisable()
		{
			SkipAction.started -= OnSkipPressed;
			//interfaceObject.openInterface -= OpenInterfaceObject;
			//interfaceObject.closeInterface -= CloseInterfaceObject;
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

		// Input action callbacks
		public void OnSkipPressed(InputAction.CallbackContext context)
		{
			//InterfaceManager.Instance.RequestClose(interfaceObject);
		}


		// Inteface Object callbacks
		public void CloseInterfaceObject()
		{
			IsGameplayUnpaused = true;
			if (DisplaySkipButton && onSkipDelegate != null)
			{
				onSkipDelegate();
			}
		}

		public void OpenInterfaceObject()
		{
			IsGameplayUnpaused = false;
		}


		// Internal methods
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

		internal bool DisplaySkipButton
		{
			get { return skipTextButton.activeInHierarchy; }
			set { skipTextButton.SetActive(value); }
		}

		internal void RequestGameplayPause()
		{
			//InterfaceManager.Instance.RequestOpen(interfaceObject);
		}

		internal void RequestGameplayUnpause()
		{
			//InterfaceManager.Instance.RequestClose(interfaceObject);
		}
	}


}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

using ImmunotherapyGame.Core;

namespace ImmunotherapyGame.UI.InGameUI
{
    public class InGameUI : Singleton<InGameUI>
    {
		// input handling
		private PlayerControls playerControls = null;

		[SerializeField]
		private List<ButtonBindings> btnBindings = null;

		[Header("Pause Submenu")]
		[SerializeField]
		private GameObject pauseMenu = null;
		[SerializeField]
		private InGameUIButtonData pauseMenuButtonData = null;

		[Header("Survey Submenu")]
		[SerializeField]
		private string surveyURL = "https://forms.office.com/Pages/ResponsePage.aspx?id=MEu3vWiVVki9vwZ1l3j8vDgrGvdUcKhJmLa6FrN3JvhUNVA2OUZJUkQ2VFMzUlQ1WldWUUJLSkVJUC4u";
		[SerializeField]
		private GameObject surveyMenu = null;
		[SerializeField]
		private InGameUIButtonData surveyMenuButtonData = null;

		protected override void Awake()
		{
			base.Awake();

			playerControls = new PlayerControls();

			foreach (var btnBinding in btnBindings)
			{
				btnBinding.button.ApplyData(btnBinding.buttonData);
				btnBinding.inputAction = playerControls.asset.FindAction(btnBinding.actionName);
			}
		}

		private void OnEnable()
		{
			playerControls.Enable();

			pauseMenuButtonData.onOpenMenu += OpenPauseMenu;
			surveyMenuButtonData.onOpenMenu += OpenSurveyMenu;
			pauseMenuButtonData.onCloseMenu += ClosePauseMenu;
			surveyMenuButtonData.onCloseMenu += CloseSurveyMenu;

			foreach (var btnBinding in btnBindings) 
			{
				btnBinding.buttonData.onChangedStatus += btnBinding.button.Animate;
				btnBinding.inputAction.started += btnBinding.button.OpenMenu;
			}

		}

		private void OnDisable()
		{
			pauseMenuButtonData.onOpenMenu -= OpenPauseMenu;
			surveyMenuButtonData.onOpenMenu -= OpenSurveyMenu;
			pauseMenuButtonData.onCloseMenu -= ClosePauseMenu;
			surveyMenuButtonData.onCloseMenu -= CloseSurveyMenu;

			foreach (var btnBinding in btnBindings)
			{
				btnBinding.buttonData.onChangedStatus -= btnBinding.button.Animate;
				btnBinding.inputAction.started -= btnBinding.button.OpenMenu;
			}
		}


		
		// Survey Menu
		public void OpenSurveyMenu()
		{
			// Request Pause
			surveyMenu.gameObject.SetActive(true);
		}

		public void GoToSurvey()
		{
			// Open URL Link
			Application.OpenURL(surveyURL);
		}

		public void CloseSurveyMenu()
		{
			// Free Pause
			surveyMenu.gameObject.SetActive(false);
		}

		// Pause Menu
		public void OpenPauseMenu()
		{
			// Request Pause
			pauseMenu.gameObject.SetActive(true);
		}

		public void ClosePauseMenu()
		{
			// Free Pause
			pauseMenu.gameObject.SetActive(false);
		}

		public void OpenSettingsMenu()
		{
			SettingsManager.Instance.Open();
		}

		public void RestartLevel()
		{
			// Restart the level
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
			ClosePauseMenu();
		}

		public void GoBackToMainMenu()
		{
			ClosePauseMenu();
			SceneManager.LoadScene(1); // 1 = Main Menu
		}

		[System.Serializable]
		internal class ButtonBindings
		{
			public InGameUIButtonData buttonData;
			public InGameUIButton button;
			public string actionName;
			[HideInInspector]
			public InputAction inputAction;
		}
	}
}

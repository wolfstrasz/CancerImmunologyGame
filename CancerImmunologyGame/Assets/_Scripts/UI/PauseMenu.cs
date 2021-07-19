using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

using ImmunotherapyGame.Core;
using ImmunotherapyGame.UI.TopOverlay;
using ImmunotherapyGame.LevelManagement;

namespace ImmunotherapyGame.UI
{
    public class PauseMenu : Singleton<PauseMenu>
    {
		[Header("Menu")]
		[SerializeField]
		private InterfaceControlPanel pauseMenuPanel = null;
        [SerializeField]
        private TopOverlayButtonData pauseMenuBtnData = null;

		[Header ("Survey")]
		[SerializeField]
		private InterfaceControlPanel surveyMenuPanel = null;
		[SerializeField]
		private TopOverlayButtonData surveyMenuBtnData = null;
		[SerializeField]
		private string surveyURL = "https://forms.office.com/Pages/ResponsePage.aspx?id=MEu3vWiVVki9vwZ1l3j8vDgrGvdUcKhJmLa6FrN3JvhUNVA2OUZJUkQ2VFMzUlQ1WldWUUJLSkVJUC4u";

		// Input handling
		private PlayerControls playerControls;

		public void Initialise() { }

		private void OnEnable()
		{
			playerControls = new PlayerControls();
			playerControls.Enable();
			pauseMenuBtnData.onOpenMenu += OpenMenuView;
			surveyMenuBtnData.onOpenMenu += OpenSurveyView;
			playerControls.Systems.PauseMenu.started += OpenMenuView;
			playerControls.Systems.SurveyMenu.started += OpenSurveyView;
		}

		private void OnDisable()
		{
			playerControls.Disable();
			pauseMenuBtnData.onOpenMenu -= OpenMenuView;
			surveyMenuBtnData.onOpenMenu -= OpenSurveyView;
			playerControls.Systems.PauseMenu.started -= OpenMenuView;
			playerControls.Systems.SurveyMenu.started -= OpenSurveyView;
		}

		// Panel Openeing callbacks
		public void OpenMenuView()
		{
			pauseMenuPanel.Open();
		}

		public void OpenMenuView(InputAction.CallbackContext context)
		{
			if (pauseMenuPanel.IsOpened)
			{
				pauseMenuPanel.Close();
			}

			else
			{
				pauseMenuPanel.Open();
			}
		}

		public void OpenSurveyView()
		{
			surveyMenuPanel.Open();
		}

		public void OpenSurveyView(InputAction.CallbackContext context)
		{
			if (surveyMenuPanel.IsOpened)
			{
				surveyMenuPanel.Close();
			}

			else
			{
				surveyMenuPanel.Open();
			}
		}

		// Buttons callbacks
		public void RestartLevel()
		{
			LevelManager.Instance.RestartLevel();
		}

		public void OpenSettings()
		{
			SettingsManager.Instance.Open();
		}

		public void BackToMainMenu()
		{
			LevelManager.Instance.LoadMainMenu();
		}
			
		public void OpenSurveyOnWeb()
		{
			Application.OpenURL(surveyURL);
		}
	}
}

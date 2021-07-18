using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

using ImmunotherapyGame.Core;
using ImmunotherapyGame.UI.TopOverlay;

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
		private PlayerControls playerControls = null;

		public void Initialise()
		{
			SceneManager.activeSceneChanged += OnActiveSceneChanged;
			gameObject.SetActive(SceneManager.GetActiveScene().buildIndex >= 2);
		}

		public void OnActiveSceneChanged(Scene currentScene, Scene nextScene)
		{
			gameObject.SetActive(nextScene.buildIndex >= 2);
		}

		protected override void Awake()
		{
			base.Awake();
			playerControls = new PlayerControls();
			playerControls.Enable();
		}

		private void OnEnable()
		{
			pauseMenuBtnData.onOpenMenu += OpenMenuView;
			surveyMenuBtnData.onOpenMenu += OpenSurveyView;
			playerControls.Systems.PauseMenu.started += OpenMenuView;
			playerControls.Systems.SurveyMenu.started += OpenSurveyView;
		}

		private void OnDisable()
		{
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
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		}

		public void OpenSettings()
		{
			SettingsManager.Instance.Open();
		}

		public void BackToMainMenu()
		{
			SceneManager.LoadScene(1); // 1 = Main Menu Scene
		}
			
		public void OpenSurveyOnWeb()
		{
			Application.OpenURL(surveyURL);
		}
	}
}

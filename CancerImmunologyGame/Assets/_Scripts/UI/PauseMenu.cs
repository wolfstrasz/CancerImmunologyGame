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
		private InterfaceControlPanel menuPanel = null;
        [SerializeField]
        private TopOverlayButtonData menuData = null;

		[Header ("Survey")]
		[SerializeField]
		private TopOverlayButtonData surveyData = null;
		[SerializeField]
		private InterfaceControlPanel surveyPanel = null;
		[SerializeField]
		private string surveyURL = "https://forms.office.com/Pages/ResponsePage.aspx?id=MEu3vWiVVki9vwZ1l3j8vDgrGvdUcKhJmLa6FrN3JvhUNVA2OUZJUkQ2VFMzUlQ1WldWUUJLSkVJUC4u";

		// Input handling
		private InputAction PauseMenuAction = null;
		private InputAction SurveyMenuAction = null;
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
			PauseMenuAction = playerControls.Systems.PauseMenu;
			SurveyMenuAction = playerControls.Systems.Survey;
		}

		private void OnEnable()
		{
			menuData.onOpenMenu += OpenMenuView;
			surveyData.onOpenMenu += OpenSurveyView;
			PauseMenuAction.started += OpenMenuView;
			SurveyMenuAction.started += OpenSurveyView;
		}

		private void OnDisable()
		{
			menuData.onOpenMenu -= OpenMenuView;
			surveyData.onOpenMenu -= OpenSurveyView;
			PauseMenuAction.started -= OpenMenuView;
			SurveyMenuAction.started -= OpenSurveyView;

		}

		// Panel Openeing callbacks
		public void OpenMenuView()
		{
			menuPanel.Open();
		}

		public void OpenMenuView(InputAction.CallbackContext context)
		{
			if (menuPanel.IsOpened)
			{
				menuPanel.Close();
			}

			else
			{
				menuPanel.Open();
			}
		}


		public void OpenSurveyView()
		{
			surveyPanel.Open();
		}

		public void OpenSurveyView(InputAction.CallbackContext context)
		{
			if (surveyPanel.IsOpened)
			{
				surveyPanel.Close();
			}

			else
			{
				surveyPanel.Open();
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

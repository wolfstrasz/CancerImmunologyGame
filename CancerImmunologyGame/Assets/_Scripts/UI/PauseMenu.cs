using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using ImmunotherapyGame.Core;
using ImmunotherapyGame.UI.TopOverlay;

namespace ImmunotherapyGame.UI
{
    public class PauseMenu : Singleton<PauseMenu>
    {
		[SerializeField]
		private InterfaceControlPanel menuObject = null;

		[Header("Menu")]
        [SerializeField]
        private TopOverlayButtonData menuData = null;
		[SerializeField]
		private GameObject menuPanel = null;

		[Header ("Survey")]
		[SerializeField]
		private TopOverlayButtonData surveyData = null;
		[SerializeField]
		private GameObject surveyPanel = null;
		[SerializeField]
		private string surveyURL = "https://forms.office.com/Pages/ResponsePage.aspx?id=MEu3vWiVVki9vwZ1l3j8vDgrGvdUcKhJmLa6FrN3JvhUNVA2OUZJUkQ2VFMzUlQ1WldWUUJLSkVJUC4u";

		private void OnEnable()
		{
			menuData.onOpenMenu += OpenMenuView;
			surveyData.onOpenMenu += OpenSurveyView;
		}

		private void OnDisable()
		{
			menuData.onOpenMenu -= OpenMenuView;
			surveyData.onOpenMenu -= OpenSurveyView;
		}

		// PauseMenu
		public void OpenMenuView()
		{
			menuPanel.SetActive(true);
		}

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

		public void CloseMenuView()
		{
			menuPanel.SetActive(false);
		}

		// Survey
		public void OpenSurveyView()
		{
			surveyPanel.SetActive(true);
		}

		public void OpenSurveyOnWeb()
		{
			Application.OpenURL(surveyURL);
		}

		public void CloseSurveyView()
		{
			surveyPanel.SetActive(false);
		}


		public void Initialise()
		{
			SceneManager.activeSceneChanged += OnActiveSceneChanged;
			gameObject.SetActive(SceneManager.GetActiveScene().buildIndex >= 2);
		}

		public void OnActiveSceneChanged(Scene currentScene, Scene nextScene)
		{
			gameObject.SetActive(nextScene.buildIndex >= 2);
		}

	}
}

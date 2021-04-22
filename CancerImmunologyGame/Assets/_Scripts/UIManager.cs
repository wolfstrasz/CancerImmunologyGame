using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using ImmunotherapyGame.GameManagement;

namespace ImmunotherapyGame
{
	public class UIManager : Singleton<UIManager>
	{
		private const int FIRST_PLAY_SCENE_ID = 2;
		private bool available = true;

		[Header("UI Panels")]
		[SerializeField] private GameObject MainMenuPanel = null;
		[SerializeField] private GameObject WinScreenPanel = null;
		[SerializeField] private GameObject GameMenuPanel = null;
		[SerializeField] private GameObject SurveyPanel = null;
		[SerializeField] private GameObject WinScene1Panel = null;

		[SerializeField] private string SurveyURL = "";

		public void WinScreen()
		{
			WinScreenPanel.SetActive(true);
		}

		public void OpenWinScenePanel1()
		{
			WinScene1Panel.SetActive(true);
		}

		public void OpenWinScenePanel2()
		{
			WinScreenPanel.SetActive(true);
		}

		public void OpenMainMenu()
		{
			WinScreenPanel.SetActive(false);
			MainMenuPanel.SetActive(true);
			GameMenuPanel.SetActive(false);
			WinScene1Panel.SetActive(false);
		}

		public void ClosePanels()
		{
			WinScreenPanel.SetActive(false);
			MainMenuPanel.SetActive(false);
			GameMenuPanel.SetActive(false);
			SurveyPanel.SetActive(false);
			WinScene1Panel.SetActive(false);

		}

		// Main Menu Functionality buttons
		public void Play()
		{
			if (!available) return;
			available = false;
			Debug.Log("Play");
			StartCoroutine(WaitToStart(FIRST_PLAY_SCENE_ID));
		}

		private IEnumerator WaitToStart(int sceneId)
		{
			yield return new WaitForSecondsRealtime(0.2f);
			available = true;
			MainMenuPanel.SetActive(false);
			WinScreenPanel.SetActive(false);
			GameMenuPanel.SetActive(false);

			SceneManager.LoadScene(sceneId);
		}


		public void OpenSettings()
		{
			if (!available) return;
			// open settings
		}

		public void OpenCredits()
		{
			if (!available) return;
			// open credits
		}

		public void QuitGame()
		{
			if (!available) return;
			available = false;
			Debug.Log("Play");
			StartCoroutine(WaitToQuit());
		}

		private IEnumerator WaitToQuit()
		{
			yield return new WaitForSecondsRealtime(0.2f);
			Application.Quit();
		}


		//public void ResetMainMenu()
		//{
		//	SettingsPanel.SetActive(false);
		//	CreditsPanel.SetActive(false);
		//}


		public void OpenURL()
		{
			Application.OpenURL(SurveyURL);
		}

		public void ContinueToScene2()
		{
			if (!available) return;
			available = false;
			Debug.Log("Play");
			StartCoroutine(WaitToStart(FIRST_PLAY_SCENE_ID + 1));
		}

		public void RestartGame()
		{
			if (!available) return;
			available = false;
			Debug.Log("Play");
			StartCoroutine(WaitToStart(SceneManager.GetActiveScene().buildIndex));
		}



		public void OnOpenGameMenu()
		{
			GameManager.Instance.RequestGamePause();
		}

		public void OnCloseGameMenu()
		{
			GameManager.Instance.RequestGameUnpause(gameObject.name);
		}

		public void UseAutoAim()
		{
			GlobalGameData.autoAim = true;
		}


		public void NoAutoAim()
		{
			GlobalGameData.autoAim = false;
		}
	}
}
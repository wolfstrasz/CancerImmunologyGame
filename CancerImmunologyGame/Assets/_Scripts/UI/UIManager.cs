using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

using ImmunotherapyGame.Core;
using ImmunotherapyGame.GameManagement;

namespace ImmunotherapyGame
{
	public class UIManager : Singleton<UIManager>
	{
		[Header("UI Panels")]
		[SerializeField] private GameObject MainMenuPanel = null;
		[SerializeField] private GameObject WinScreenPanel = null;
		[SerializeField] private GameObject GameMenuPanel = null;
		[SerializeField] private GameObject SurveyPanel = null;
		[SerializeField] private GameObject WinScene1Panel = null;

		[SerializeField] private string SurveyURL = "";

	}
}
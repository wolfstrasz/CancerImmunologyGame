using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Tutorials
{
	public class TutorialManager : Singleton<TutorialManager>
	{
		[Header("UI Elements Linking")]
		[SerializeField]
		private GameObject tutorialPanel = null;
		[SerializeField]
		private TMP_Text tutorialTxt = null;
		[SerializeField]
		private GameObject skipTextButton = null;

		[SerializeField]
		private List<GameObject> StoryTutorials = new List<GameObject>();



		public void Initialise()
		{
			//foreach (GameObject ts in StoryTutorials)
			//{
			//	Instantiate(ts, Vector3.zero, Quaternion.identity).GetComponent<TutorialStage>().StartStage();
			//}
		}

		

		public void HideText()
		{
			tutorialPanel.SetActive(false);
			skipTextButton.SetActive(false);
			tutorialTxt.gameObject.SetActive(false);
		}

		public void DisplayText(string text)
		{
			tutorialPanel.SetActive(true);
			tutorialTxt.text = text;
			tutorialTxt.gameObject.SetActive(true);
		}

		public void DisplaySkipButton()
		{
			skipTextButton.SetActive(true);
		}


		public void OnStageFinished(TutorialStage stage)
		{
			Debug.Log("Tutorial stage: " + stage.name + " is finished!");
		}



		public enum SpecialTutorials { REGULATORY_CELL, HEALTH, THERAPY, EXHAUST }

	}
}


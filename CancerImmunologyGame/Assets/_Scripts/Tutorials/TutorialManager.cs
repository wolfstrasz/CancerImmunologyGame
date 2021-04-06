using System.Collections.Generic;
using UnityEngine;
using TMPro;
using ImmunotherapyGame.Tutorials;


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
	private List<TutorialStage> StoryTutorials = new List<TutorialStage>();

	public void Initialise()
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

	internal void HideText()
	{
		tutorialPanel.SetActive(false);
		skipTextButton.SetActive(false);
		tutorialTxt.gameObject.SetActive(false);
	}

	internal void DisplayText(string text)
	{
		tutorialPanel.SetActive(true);
		tutorialTxt.text = text;
		tutorialTxt.gameObject.SetActive(true);
	}

	internal void DisplaySkipButton()
	{
		skipTextButton.SetActive(true);
	}

}



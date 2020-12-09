using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class TutorialManager : Singleton<TutorialManager>
{
	[Header("Elements Linking")]
	[SerializeField]
	private GameObject tutorialPanel = null;
	[SerializeField]
	private TMP_Text tutorialTxt = null;
	[SerializeField]
	private GameObject skipTextButton = null;

	[SerializeField]
	private List<TutorialStage> StoryTutorials = new List<TutorialStage>();
	private int tutorial_index = 0;


	public void NextTutorialStage()
	{
		if (tutorial_index < StoryTutorials.Count)
		{
			StoryTutorials[tutorial_index].StartStage();
			++tutorial_index;
		}

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

	void Update()
	{

		if (Input.GetKeyDown(KeyCode.P))
		{
			NextTutorialStage();
		}
	}

	public void OnStageFinished()
	{
		
	}




	private void Awake()
	{
		// Create dictionary
	}

	public enum SpecialTutorials { REGULATORY_CELL, HEALTH, THERAPY, EXHAUST}

}


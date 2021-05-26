using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ImmunotherapyGame.Tutorials
{
	public class TutorialText : TutorialEvent
	{

		[Header("Text Displayed")]
		[SerializeField][TextArea(5,20)]
		private string text = "";

		[Header("Skipping functionality")]
		[SerializeField]
		private bool shouldHideText = false;
		[SerializeField]
		private bool canSkipTxt = false;
		[SerializeField]
		private float waitBeforeSkip = 0.0f;

		[Header("Timing")]
		[SerializeField]
		private bool isTimed = false;
		[SerializeField]
		private float timeBeforeFinish = 0.0f;

		private bool isSkipped = false;
		private bool timedOut = false;

		protected override void OnEndEvent() 
		{
			TutorialManager.Instance.onSkipDelegate -= Skip;
			TutorialManager.Instance.HideText();
		}

		protected override void OnStartEvent()
		{
			if (!shouldHideText)
			{
				TutorialManager.Instance.DisplayText(text);
			}

			if (canSkipTxt)
			{
				TutorialManager.Instance.onSkipDelegate += Skip;
				StartCoroutine(WaitBeforeSkipButton());
			}
		}

		protected override bool OnUpdateEvent()
		{
			if (isTimed)
			{
				timeBeforeFinish -= Time.deltaTime;
				timedOut = timeBeforeFinish <= 0f;
			}

			return (timedOut || isSkipped);
		}

		IEnumerator WaitBeforeSkipButton()
		{
			yield return new WaitForSecondsRealtime(waitBeforeSkip);
			TutorialManager.Instance.DisplaySkipButton();
		}

		internal void Skip()
		{
			// Skips text
			isSkipped = true;
			// Panel is already closed so we should prevent a second close panel call
			shouldPauseGameplay = false;
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ImmunotherapyGame.Tutorials
{
	public class TutorialText : TutorialEvent
	{

		[Header("Text Displayed")]
		[SerializeField][TextArea(5,20)] private string text = "";

		[Header("Skipping functionality")]
		[SerializeField] private bool shouldHideText = false;
		[SerializeField] private bool canSkipTxt = false;
		[SerializeField] private float waitBeforeSkip = 0.0f;

		[Header("Timing")]
		[SerializeField] private bool isTimed = false;
		[SerializeField] private float timeBeforeFinish = 0.0f;

		[SerializeField] [ReadOnly] private bool isSkipped = false;
		[SerializeField] [ReadOnly] private bool timedOut = false;

		protected override void OnEndEvent() 
		{
			TutorialManager.Instance.onSkipDelegate -= Skip;
		}

		protected override void OnStartEvent()
		{
			if (!shouldHideText)
			{
				TutorialManager.Instance.DisplayTextPanel(text);
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
		}
	}
}

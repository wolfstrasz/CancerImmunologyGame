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
		private bool allowSkip = false;

		[Header("Timing")]
		[SerializeField]
		private bool timed = false;
		[SerializeField]
		private float timeBeforeFinish = 0.0f;
		private bool allowFinish = false;

		protected override void OnEndEvent()
		{
			TutorialManager.Instance.HideText();
		}

		protected override void OnStartEvent()
		{
			if (!shouldHideText)
			{
				TutorialManager.Instance.DisplayText(text);
			}

			if (canSkipTxt)
				StartCoroutine(WaitBeforeSkipButton());
		}

		protected override bool OnUpdateEvent()
		{
			if (timed)
			{
				Debug.LogWarning("HELLO TIMED");

				timeBeforeFinish -= Time.deltaTime;
				if (timeBeforeFinish <= 0f)
				{
					allowFinish = true;
				}
			}

			if (allowSkip)
			{
				if (Input.GetKeyDown(KeyCode.Space))
					return true;
				return false;
			}

			if (allowFinish)
				return true;
			return false;
		}

		IEnumerator WaitBeforeSkipButton()
		{
			yield return new WaitForSecondsRealtime(waitBeforeSkip);
			allowSkip = true;
			TutorialManager.Instance.DisplaySkipButton();
		}
	}
}

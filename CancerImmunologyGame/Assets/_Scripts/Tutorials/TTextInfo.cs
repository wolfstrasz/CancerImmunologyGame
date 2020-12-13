using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Tutorials
{
	public class TTextInfo : TutorialEvent
	{

		[Header("Text Displayed")]
		[SerializeField]
		private string text = "";

		[Header("Skipping functionality")]
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
			TutorialManager.Instance.DisplayText(text);

			if (canSkipTxt)
				StartCoroutine(WaitBeforeSkipButton());
			if (timed)
				StartCoroutine(WaitBeforeFinish());
		}

		protected override bool OnUpdate()
		{
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

		IEnumerator WaitBeforeFinish()
		{
			yield return new WaitForSecondsRealtime(timeBeforeFinish);
			allowFinish = true;
		}
	}
}

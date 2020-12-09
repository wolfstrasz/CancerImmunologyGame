using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Preload
{
	public class Intro : Singleton<Intro>
	{
		[Header("Logos")]
		[SerializeField]
		private IntroFadeCallback logoAnimator = null;

		[Header("Intro texts")]
		[SerializeField]
		private List<GameObject> texts = new List<GameObject>();

		[Header("Skip functionality")]
		[SerializeField]
		private GameObject continueSpaceBar = null;
		[SerializeField]
		private Slider skipHoldSlider = null;
		[SerializeField]
		private float timeBeforeSkipAppears = 2.0f;
		[SerializeField]
		private float timeHoldToSkip = 2.0f;

		private int text_index = 0;
		private bool allowSkip = false;

		[Header("Debug Attributes")]
		[SerializeField]
		private bool skipLogos = false;
		[SerializeField]
		private bool skipIntroTexts = false;

		void Awake()
		{
			skipHoldSlider.maxValue = timeHoldToSkip;
		}

		void Update()
		{
			if (allowSkip)
			{
				if (Input.GetKey(KeyCode.Space))
				{
					Debug.Log(skipHoldSlider.value + " : " + skipHoldSlider.maxValue);
					skipHoldSlider.value = skipHoldSlider.value + Time.deltaTime;
					if (skipHoldSlider.value == skipHoldSlider.maxValue)
					{
						IntroFinished();
					}
				} else
				{
					skipHoldSlider.value = 0;
				}
			}
		}


		internal void ShowNextText()
		{
			if (text_index < texts.Count)
			{
				texts[text_index].SetActive(true);
				++text_index;
			}
		}

		private void IntroFinished()
		{
			Preloader.Instance.OnIntroFinished();
		}

		/// <summary>
		/// Call to make logos fade away in the intro screen
		/// </summary>
		internal void FadeLogos()
		{
			if (skipLogos)
			{
				logoAnimator.gameObject.SetActive(false);
				LogoFadeFinished();
				return;
			}
			Debug.Log("Fade Logos");
			logoAnimator.StartFade();
		}


		// Animation callbacks
		internal void LogoFadeFinished()
		{
			if (skipIntroTexts)
			{
				IntroFinished();
				return;
			}
			ShowNextText();
			StartCoroutine(WaitToShowSkipButton());
		}


		IEnumerator WaitToShowSkipButton()
		{
			yield return new WaitForSeconds(timeBeforeSkipAppears);
			continueSpaceBar.SetActive(true);
			skipHoldSlider.gameObject.SetActive(true);

			allowSkip = true;
			yield return null;
		}
	}
}
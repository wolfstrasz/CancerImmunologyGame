using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;
using ImmunotherapyGame.Core;

namespace ImmunotherapyGame.Loader
{

	public class Intro : MonoBehaviour
	{
		// Input handling
		private PlayerControls playerControls = null;
		private InputAction SkipAction = null;

		[Header("Force skipping")]
		[SerializeField]
		private bool forceSkip = false;

		[Header("Logos")]
		[SerializeField]
		private Animator logoAnimator = null;
		[SerializeField]
		private float timeBeforeLogoFade = 3.0f;

		[Header("Intro texts")]
		[SerializeField]
		private List<GameObject> texts = new List<GameObject>();
		private int text_index = 0;

		[Header("Skip text functionality")]
		[SerializeField]
		private TMP_Text continueTxt = null;
		[SerializeField]
		private Slider skipHoldSlider = null;
		[SerializeField]
		private float timeBeforeSkipAppears = 2.0f;
		[SerializeField]
		private float timeHoldToSkip = 2.0f;
		[ReadOnly]
		private bool allowSkip = false;

		

		// Properties for intro text handling
		internal bool HasFinished { get; set; }
		private bool IsSkipping { get; set; }

		// Properties for intro logos handling
		internal bool LoadingFinished { get; set; }
		private bool LogosCanFade { get; set; }

		private void Awake()
		{
			playerControls = new PlayerControls();

			skipHoldSlider.maxValue = timeHoldToSkip;

			// Bind skip action
			SkipAction = playerControls.Systems.Skip;

			// Update skip text
			List<string> bindings = Utils.GetAllKeybindsStrings(SkipAction);
			if (bindings.Count == 0) return;
			string displayStr = "Hold [" + bindings[0];

			for (int i = 1; i < bindings.Count; ++i)
			{
				displayStr += "] | [" + bindings[i];
			}
			displayStr += "] to skip. ";

			continueTxt.text = displayStr;

			HasFinished = forceSkip;
			if (!forceSkip)
			{
				StartCoroutine(WaitForLogosInitialTime());
			}
		}

		private void Update()
		{
			if (LogosCanFade && LoadingFinished)
			{
				Debug.Log("Intro: Starting Logo fade");
				logoAnimator.SetTrigger("Fade");
				LogosCanFade = false;
				return;
			}

			if (IsSkipping)
			{
				skipHoldSlider.value += Time.deltaTime;
			} else
			{
				skipHoldSlider.value = 0;
			}

			if (skipHoldSlider.value == skipHoldSlider.maxValue)
			{
				HasFinished = true;
			}
		}

		private void OnEnable()
		{
			SkipAction.started += OnSkipPressed;
			SkipAction.canceled += OnSkipReleased;
			playerControls.Enable();

		}

		private void OnDisable()
		{
			SkipAction.started -= OnSkipPressed;
			SkipAction.canceled -= OnSkipPressed;
			StopAllCoroutines();

		}

		// Intro private methods
		internal void ShowNextText()
		{
			if (text_index < texts.Count)
			{
				texts[text_index].SetActive(true);
				++text_index;
			}
		}

		private IEnumerator WaitToShowSkipButton()
		{
			yield return new WaitForSeconds(timeBeforeSkipAppears);

			continueTxt.gameObject.SetActive(true);
			skipHoldSlider.gameObject.SetActive(true);
			allowSkip = true;
			yield return null;
		}

		private IEnumerator WaitForLogosInitialTime()
		{
			yield return new WaitForSecondsRealtime(timeBeforeLogoFade);
			LogosCanFade = true;
		}

		// Logo animation fade callback
		public void LogoFadeFinished()
		{
			Debug.Log("Intro: Logo animation fade finished");
			ShowNextText();
			StartCoroutine(WaitToShowSkipButton());
		}


		// Input listeners functionality
		void OnSkipPressed(InputAction.CallbackContext context)
		{
			Debug.Log("Skip called");
			IsSkipping = allowSkip;
		}

		void OnSkipReleased(InputAction.CallbackContext context)
		{

			Debug.Log("Skip released");
			IsSkipping = false;
		}
	}
	
}
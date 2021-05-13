using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;
using ImmunotherapyGame.Core;

namespace ImmunotherapyGame.Loader
{

	public class Intro : Singleton<Intro>
	{
		[Header("Player Input")]
		[SerializeField]
		PlayerInput playerInput = null;
		[SerializeField]
		InputAction SkipAction = null;

		[Header("Logos")]
		[SerializeField]
		private IntroFadeCallback logoAnimator = null;

		[Header("Intro texts")]
		[SerializeField]
		private List<GameObject> texts = new List<GameObject>();
		private int text_index = 0;

		[Header("Skip functionality")]
		[SerializeField]
		private GameObject continueSpaceBar = null;
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

		[Header("Force skipping")]
		[SerializeField]
		private bool forceSkip = false;
		[SerializeField]
		private bool skipLogos = false;
		[SerializeField]
		private bool skipIntroTexts = false;

		private void OnEnable()
		{
			SkipAction.started += OnSkipPressed;
			SkipAction.canceled += OnSkipReleased;
		}

		private void OnDisable()
		{
			SkipAction.started -= OnSkipPressed;
			SkipAction.canceled -= OnSkipPressed;
		}

		void Awake()
		{
			skipHoldSlider.maxValue = timeHoldToSkip;
			if (forceSkip)
				IntroFinished();

			Debug.Log("Searching for action");
			Debug.Log(playerInput.currentActionMap.ToString());
			Debug.Log(playerInput.currentActionMap.FindAction("Skip").ToString());
			SkipAction = playerInput.currentActionMap.FindAction("Skip", true);

			List<string> bindings = InputHandlerUtils.GetAllKeybindsStrings(SkipAction);

			if (bindings.Count == 0) return;
			string displayStr = "Hold [" + bindings[0];

			for (int i = 1; i < bindings.Count; ++i)
			{
				displayStr += "] | [" + bindings[i];
			}
			displayStr += "] to skip. ";

			continueTxt.text = displayStr;
		}


		void Update()
		{
			if (Skipping)
			{
				skipHoldSlider.value += Time.deltaTime;
			} else
			{
				skipHoldSlider.value = 0;
			}

			if (skipHoldSlider.value == skipHoldSlider.maxValue)
			{
				IntroFinished();
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
			Loader.Instance.OnIntroFinished();
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


		private bool Skipping { get; set; }


		void OnSkipPressed(InputAction.CallbackContext context)
		{
			Debug.Log("Skip called");
			if (allowSkip)
				Skipping = true;
		}

		void OnSkipReleased(InputAction.CallbackContext context)
		{

			Debug.Log("Skip released");
			Skipping = false;
		}
	}
	
}
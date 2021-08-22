using UnityEngine;
using System.Collections.Generic;

using ImmunotherapyGame.Core;

namespace ImmunotherapyGame.Audio
{
	public class BackgroundMusic : Singleton<BackgroundMusic>
	{
		[Header("Music and Sources")]
		[SerializeField] private List<MusicType> musicTypes = new List<MusicType>();
		[SerializeField] private AudioSource firstSource;
		[SerializeField] private AudioSource secondSource;

		[SerializeField] [ReadOnly] private AudioSource currentSource = null;
		[SerializeField] [ReadOnly] private AudioSource sourceToDecrease = null;
		[SerializeField] [ReadOnly] private AudioSource sourceToIncrease = null;

		[SerializeField] [ReadOnly] private MusicType currentType;
		[SerializeField] [ReadOnly] private MusicType correctType;

		[Header("Timing")]
		[SerializeField] private float transitionTime = 3.0f;
		[SerializeField] private float timeInTransition = 3.0f;
		[SerializeField] private float timeGapSafetyCheck = 10.0f;
		[SerializeField] [ReadOnly] private bool initialised = false;

		public void Initialise()
		{
			// Reset sources
			firstSource.volume = 0f;
			firstSource.Stop();
			secondSource.volume = 0f;
			secondSource.Stop();
			currentSource = firstSource;

			// Sort music types by priority and assing current and correct types to the highest priority one
			musicTypes.Sort((musicType1, musicType2) => musicType1.priority.CompareTo(musicType2.priority));
			currentType = musicTypes[0];
			correctType = musicTypes[0];

			timeInTransition = transitionTime + timeGapSafetyCheck;
			initialised = true;
			gameObject.SetActive(false);
		}

		// Update is called once per frame
		void Update()
		{
			if (timeInTransition < transitionTime)
			{
				timeInTransition += Time.deltaTime;
				sourceToIncrease.volume = timeInTransition / transitionTime;
				sourceToDecrease.volume = 1.0f - timeInTransition / transitionTime;

				if (timeInTransition >= transitionTime)
				{
					sourceToDecrease.Stop();
					timeInTransition += timeGapSafetyCheck;
				}
			}

			if (currentType != correctType && timeInTransition >= transitionTime + timeGapSafetyCheck)
			{
				// Music Blending
				timeInTransition = 0.0f;

				// Current source has to start decreasing
				sourceToDecrease = currentSource;

				// Choose other source to be the new and increasing source
				currentSource = currentSource == firstSource ? secondSource : firstSource;
				currentSource.clip = correctType.musicClip;

				sourceToIncrease = currentSource;
				sourceToIncrease.volume = 0.0f;
				sourceToIncrease.time = correctType.clipStartTime;
				sourceToIncrease.Play();

				currentType = correctType;
			}
		}

		internal void Subscribe(MusicType type)
		{
			type.subscribers++;
			CorrectMusic();
		}

		internal void Unsubscribe(MusicType type)
		{
			type.subscribers--;
			CorrectMusic();
		}

		private void CorrectMusic()
		{
			// Go through music types and get the one with highest subscribers
			// if two have the same one then select one with higher priority
			int maxSubscribers = -1;
			for (int i = 0; i < musicTypes.Count; ++i)
			{
				if (musicTypes[i].subscribers > maxSubscribers)
				{
					maxSubscribers = musicTypes[i].subscribers;
					correctType = musicTypes[i];
				}
			}
		}

		public void StopBackgroundMusic()
		{
			gameObject.SetActive(false);
		}

		public void EnableBackgroundMusic()
		{
			gameObject.SetActive(true);
		}

		private void OnEnable()
		{
			if (initialised)
			{
				currentSource.clip = currentType.musicClip;
				currentSource.volume = 1f;
				currentSource.Play();
			}
		}

		private void OnDisable()
		{
			currentSource.Stop();
		}

		public void PlayMusic()
		{
			gameObject.SetActive(true);
		}

	}
}
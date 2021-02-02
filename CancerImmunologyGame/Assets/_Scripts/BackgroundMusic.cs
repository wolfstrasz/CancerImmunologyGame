using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : Singleton<BackgroundMusic>
{
	[Header ("Music")]
	[SerializeField]
	private AudioSource normalMusic = null;
	[SerializeField]
	private AudioSource travelMusic = null;
	[SerializeField]
	private AudioSource battleMusic= null;

	[Header ("Timing")]
	[SerializeField]
	private float transitionTime = 3.0f;
	[SerializeField]
	private float timeInTransition = 3.0f;
	[SerializeField]
	private float timeGapSafetyCheck = 10.0f;

	[Header("DEBUG (READ ONLY)")]
	[SerializeField]
	AudioSource sourceToDecrease = null;
	[SerializeField]
	AudioSource sourceToIncrease = null;
	
	[SerializeField]
	private BackgroundMusicType currentType = BackgroundMusicType.NORMAL;
	[SerializeField]
	private BackgroundMusicType correctType = BackgroundMusicType.NORMAL;

	public void Initialise()
	{

		travelMusic.Stop();
		battleMusic.Stop();
		normalMusic.Play();
		normalMusic.volume = 1.0f;
		currentType = BackgroundMusicType.NORMAL;
		correctType = BackgroundMusicType.NORMAL;
		timeInTransition = transitionTime + timeGapSafetyCheck;

	}

	// Update is called once per frame
	void Update()
    {

		if (Input.GetKeyDown(KeyCode.B))
		{
			correctType = BackgroundMusicType.BATTLE;

		}

		if (Input.GetKeyDown(KeyCode.N))
		{
			correctType = BackgroundMusicType.TRAVEL;

		}

		if (Input.GetKeyDown(KeyCode.M))
		{
			correctType = BackgroundMusicType.NORMAL;

		}

		if (Input.GetKeyDown(KeyCode.L))
		{
			Initialise();
		}

		if (timeInTransition < transitionTime)
		{
			Debug.Log("Should Transition: " + timeInTransition +  " : " + Time.deltaTime);
			timeInTransition += Time.deltaTime;
			sourceToIncrease.volume = timeInTransition / transitionTime;
			sourceToDecrease.volume = 1.0f - timeInTransition / transitionTime;

			if (timeInTransition >= transitionTime)
			{
				sourceToDecrease.Stop();
				timeInTransition += timeGapSafetyCheck;
			}
		}

		if ( currentType != correctType && timeInTransition >= transitionTime + timeGapSafetyCheck)
		{
			timeInTransition = 0.0f;
			StopCurrentType();
			StartCorrectType();
			currentType = correctType;
		}

	}


	private void StopCurrentType()
	{
		if (currentType == BackgroundMusicType.NORMAL)
		{
			sourceToDecrease = normalMusic;
		}
		if (currentType == BackgroundMusicType.BATTLE)
		{
			sourceToDecrease = battleMusic;
		}
		if (currentType == BackgroundMusicType.TRAVEL)
		{
			sourceToDecrease = travelMusic;
		}

	}

	private void StartCorrectType()
	{
		if (correctType == BackgroundMusicType.NORMAL)
		{
			sourceToIncrease = normalMusic;
		}
		else if (correctType == BackgroundMusicType.BATTLE)
		{
			sourceToIncrease = battleMusic;
		}
		else if (correctType == BackgroundMusicType.TRAVEL)
		{
			sourceToIncrease = travelMusic;
		}

		sourceToIncrease.volume = 0.0f;
		sourceToIncrease.Play();
	}


	public void PlayBattleMusic()
	{
		correctType = BackgroundMusicType.BATTLE;
	}

	public void PlayNormalMusic()
	{
		correctType = BackgroundMusicType.NORMAL;
	}

	public void PlayTravelMusic()
	{
		correctType = BackgroundMusicType.TRAVEL;
	}

	public void PlayMusic(BackgroundMusicType type)
	{
		correctType = type;
	}

	public enum BackgroundMusicType { NORMAL, TRAVEL, BATTLE }
}

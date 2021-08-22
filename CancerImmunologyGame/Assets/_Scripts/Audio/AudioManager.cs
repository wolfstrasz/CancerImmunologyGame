using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Audio;
using ImmunotherapyGame.Core;

namespace ImmunotherapyGame.Audio
{
	public class AudioManager : Singleton<AudioManager>
	{
		private Dictionary<UIAudioClipKey, AudioClip> uiAudioLibrary = null;

		[Header("General Settings")]
		[SerializeField] private AudioMixer mixer = null;
		[SerializeField] private float volumeBoost = 10f;

		[Header("UI Audio")]
		[SerializeField] private AudioSource uiSource = null;
		[SerializeField] private List<UIAudioClipBinding> uiAudioClipBindings = null;
		float lowestDecibelsBeforeMute = -80f;

		public void Initialise()
		{
			if (uiAudioClipBindings != null)
			{
				uiAudioLibrary = new Dictionary<UIAudioClipKey, AudioClip>(uiAudioClipBindings.Count);

				foreach (UIAudioClipBinding binding in uiAudioClipBindings)
				{
					var key = binding.key;
					if (uiAudioLibrary.ContainsKey(key))
					{
						Debug.LogWarning("UI Audio Library already contains binding for key: " + key + " mapped to: " + uiAudioLibrary[key]);
					}
					else
					{
						uiAudioLibrary.Add(key, binding.clip);
					}
				}
			}
		}

		public void PlayUISoundClip(UIAudioClipKey key, GameObject caller)
		{
			if (uiAudioLibrary == null) return;

			if (uiAudioLibrary.ContainsKey(key))
			{
				uiSource.Stop();
				uiSource.clip = uiAudioLibrary[key];
				uiSource.Play();
			}
			else if (key != UIAudioClipKey.NONE)
			{
				Debug.LogError("No [" + key + "] was found in UI Audio Library. Check if " + caller.name + " has a correct UI audio key.");
			}
		}

		public void SetVolume(AudioChannel channel, float volume)
		{
			Debug.Log("Setting new volume :" + volume);

			float adjustedVolume = lowestDecibelsBeforeMute + (-lowestDecibelsBeforeMute / 5 * volume / 20);
			Debug.Log("Setting new volume :" + adjustedVolume);

			if (volume == 0)
			{
				adjustedVolume = -100;
			}
			Debug.Log("Setting new volume :" + adjustedVolume);

			switch (channel)
			{
				case AudioChannel.Master:
					mixer.SetFloat("MasterVolume", adjustedVolume + volumeBoost);
					break;
				case AudioChannel.Music:
					mixer.SetFloat("MusicVolume", adjustedVolume + volumeBoost);
					break;
				case AudioChannel.SFX:
					mixer.SetFloat("SFXVolume", adjustedVolume + volumeBoost);
					break;
				case AudioChannel.UI:
					mixer.SetFloat("UIVolume", adjustedVolume + volumeBoost);
					break;
				default:
					break;
			}
		}
	}


	
	[System.Serializable]
	public class UIAudioClipBinding
	{
		public AudioClip clip;
		public UIAudioClipKey key;
	}

	[System.Serializable]
	public enum UIAudioClipKey { NONE, BUTTON, ACHIEVEMENT }
	[System.Serializable]
	public enum AudioChannel { Master, Music, SFX, UI}
}

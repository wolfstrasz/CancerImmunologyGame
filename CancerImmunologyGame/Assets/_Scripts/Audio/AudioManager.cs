using System;
using System.Collections.Generic;

using UnityEngine;
using ImmunotherapyGame.Core;

namespace ImmunotherapyGame.Audio
{
	public class AudioManager : Singleton<AudioManager>
	{
		private Dictionary<UIAudioClipKey, AudioClip> uiAudioLibrary = null;

		[Header("UI Audio")]
		[SerializeField]
		private AudioSource uiSource = null;
		[SerializeField]
		private List<UIAudioClipBinding> uiAudioClipBindings = null;

		private void Start()
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
			else
			{
				Debug.LogError("No [" + key + "] was found in UI Audio Library. Check if " + caller.name + " has a correct UI audio key.");
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
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ImmunotherapyGame.Audio
{
	[CreateAssetMenu(menuName = ("MyAssets/Music Type"))]
	public class MusicType : ScriptableObject, ISerializationCallbackReceiver
	{
		public AudioClip musicClip;
		public int priority;
		public float clipStartTime = 0.0f;
		[ReadOnly] public int subscribers;
		public void OnAfterDeserialize() { subscribers = 0; }
		public void OnBeforeSerialize() { }

	}

}

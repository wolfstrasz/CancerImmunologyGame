using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.GameManagement;

namespace ImmunotherapyGame.Tutorials
{
	public abstract class TutorialEvent : MonoBehaviour
	{
		[SerializeField]
		internal int order = 0;

		[SerializeField]
		protected bool shouldPauseGameplay;

		public void StartEvent()
		{
			if (shouldPauseGameplay)
			{
				GameManager.Instance.RequestGameplayPause();
			}

			OnStartEvent();
		}

		public void EndEvent()
		{
			if (shouldPauseGameplay)
			{
				GameManager.Instance.RequestGameplayUnpause(gameObject.name);
			}

			OnEndEvent();
		}

		protected abstract void OnStartEvent();

		protected abstract bool OnUpdateEvent();

		protected abstract void OnEndEvent();

		internal bool OnUpdate()
		{
			return OnUpdateEvent();
		}
	}
}

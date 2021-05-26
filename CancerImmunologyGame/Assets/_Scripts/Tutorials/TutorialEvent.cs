using UnityEngine;

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
				TutorialManager.Instance.RequestGameplayPause();
			}

			OnStartEvent();
		}

		public void EndEvent()
		{
			if (shouldPauseGameplay)
			{
				TutorialManager.Instance.RequestGameplayUnpause();
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

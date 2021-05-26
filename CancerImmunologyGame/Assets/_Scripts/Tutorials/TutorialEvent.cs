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
				Debug.Log(gameObject.name + " requests game pause");
				TutorialManager.Instance.RequestPause();
			}

			OnStartEvent();
		}

		public void EndEvent()
		{
			if (shouldPauseGameplay)
			{
				Debug.Log(gameObject.name + " requests game UN pause");

				TutorialManager.Instance.RequestUnpause();
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

using UnityEngine;

namespace ImmunotherapyGame.Tutorials
{
	public abstract class TutorialEvent : MonoBehaviour
	{
		[SerializeField] protected bool shouldPauseGameplay;

		public void StartEvent()
		{
			if (shouldPauseGameplay)
			{
				//Debug.Log(gameObject.name + " requests game pause");
				TutorialManager.Instance.RequestPause();
			}

			OnStartEvent();
		}

		public bool EndEvent()
		{
			bool successfulUnpause = true;
			if (shouldPauseGameplay)
			{
				//Debug.Log(gameObject.name + " requests game UN pause");

				successfulUnpause = TutorialManager.Instance.RequestUnpause();
			}

			if (successfulUnpause)
			{
				OnEndEvent();
				return true;
			}
			return false;
		}

		protected virtual void OnStartEvent() { }

		protected virtual bool OnUpdateEvent() { return true; }

		protected virtual void OnEndEvent() { }

		internal bool OnUpdate()
		{
			return OnUpdateEvent();
		}
	}
}

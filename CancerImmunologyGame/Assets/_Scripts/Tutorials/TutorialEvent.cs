using UnityEngine;

namespace ImmunotherapyGame.Tutorials
{
	public abstract class TutorialEvent : MonoBehaviour
	{
		[SerializeField] protected bool shouldPauseGameplay;

		/// <summary>
		/// Called by the Tutorial Stage when the event is activated.
		/// </summary>
		internal void StartEvent()
		{
			if (shouldPauseGameplay)
			{
				//Debug.Log(gameObject.name + " requests game pause");
				TutorialManager.Instance.RequestPause();
			}

			OnStartEvent();
		}

		/// <summary>
		/// Called by the Tutorial Stage when the event has fully finished.
		/// </summary>
		/// <returns></returns>
		internal bool EndEvent()
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

		/// <summary>
		/// Called by the Tutorial Stage every Update loop.
		/// </summary>
		/// <returns></returns>
		internal bool OnUpdate()
		{
			return OnUpdateEvent();
		}

		/// <summary>
		/// Called at the end of StartEvent
		/// </summary>
		protected virtual void OnStartEvent() { }

		/// <summary>
		/// Returns true if all event requirements were satisfied.
		/// </summary>
		/// <returns></returns>
		protected virtual bool OnUpdateEvent() { return true; }

		/// <summary>
		/// Called at the end of EndEvent
		/// </summary>
		protected virtual void OnEndEvent() { }

	
	}
}

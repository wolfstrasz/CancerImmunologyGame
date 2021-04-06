using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ImmunotherapyGame.Tutorials
{
	public class TutorialStage : MonoBehaviour
	{
		[SerializeField]
		private List<TutorialEvent> events = null;
		private TutorialEvent currentEvent = null;
		private int event_index = 0;
		private bool isFinished = false;


		internal bool IsFinished { get => isFinished; }

		internal void OnUpdate()
		{
			if (isFinished) return;

			// If event is finished
			if (currentEvent.OnUpdate())
			{
				// Stop previous event
				currentEvent.EndEvent();
				currentEvent.gameObject.SetActive(false);
				Debug.Log(gameObject.name + " event finished: " + currentEvent.name);

				// Start next event
				currentEvent = events[event_index];
				currentEvent.gameObject.SetActive(true);    // Debugging
				currentEvent.StartEvent();

				// Update stage
				++event_index;
				isFinished = event_index >= events.Count;
			}
		}

		internal void InitialiseStage()
		{
			// Collect events and sort them by priority
			List<TutorialEvent> eventsFound = new List<TutorialEvent>(GetComponentsInChildren<TutorialEvent>());
			eventsFound.OrderBy(e => e.order);
			events = eventsFound;

			// Run the first event
			if (events.Count > 0)
			{
				currentEvent = events[event_index];
				currentEvent.gameObject.SetActive(true);	// Debugging
				++event_index;
				currentEvent.StartEvent();
			}

		}

	}
}

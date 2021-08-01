using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ImmunotherapyGame.Tutorials
{
	public class TutorialWaitForCameraInView : TutorialEvent
	{
		[SerializeField] private List<GameObject> objectsToWait = new List<GameObject>();
		
		protected override void OnEndEvent() { }

		protected override void OnStartEvent() { }

		protected override bool OnUpdateEvent()
		{
			foreach (GameObject go in objectsToWait)
			{
				if (GameCamera2D.Instance.IsInCameraViewBounds(go.transform.position, true))
				{
					return true;
				}
			}

			return false;
		}

	}
}
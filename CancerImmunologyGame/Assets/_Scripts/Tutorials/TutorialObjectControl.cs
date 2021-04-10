using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ImmunotherapyGame.Tutorials
{
	public class TutorialObjectControl : TutorialEvent
	{
		[SerializeField]
		private List<GameObject> objectsToControl = new List<GameObject>();
		[SerializeField]
		private ControlMode mode = ControlMode.DEACTIVATE;

		protected override void OnEndEvent()
		{
		}

		protected override void OnStartEvent()
		{
			foreach (GameObject obj in objectsToControl)
			{
				if (obj == null)
				{
					Debug.LogWarning("Object reference is null. Please add it through editor");
				} 
				else if (mode == ControlMode.ACTIVATE)
				{

					obj.SetActive(true);
				}
				else if (mode == ControlMode.DEACTIVATE)
				{

					obj.SetActive(false);
				}
			}
		}

		protected override bool OnUpdateEvent()
		{
			if (mode == ControlMode.MONITOR_ACTIVATED)
			{
				bool allActivated = false;
				for (int i = 0; i < objectsToControl.Count; ++i)
				{
					allActivated = allActivated || objectsToControl[i].activeInHierarchy;
				}

				return allActivated;
			} 
			else if (mode == ControlMode.MONITOR_DEACTIVATED)
			{
				bool anyActivated = true;
				for (int i = 0; i < objectsToControl.Count; ++i)
				{
					anyActivated = anyActivated && objectsToControl[i].activeInHierarchy;
				}

				return !anyActivated;
			}

			return true;
		}

		private enum ControlMode { ACTIVATE, DEACTIVATE, MONITOR_ACTIVATED, MONITOR_DEACTIVATED}
	}
}

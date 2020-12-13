using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tutorials
{
	public class TCameraFocus : TutorialEvent
	{
		[SerializeField]
		private FocusObjectType closestObjectTypeToFocusOn = FocusObjectType.NONE;

		protected override void OnEndEvent()
		{
			return;
		}

		protected override void OnStartEvent()
		{
			if (closestObjectTypeToFocusOn == FocusObjectType.NONE) return;
			if (closestObjectTypeToFocusOn == FocusObjectType.PLAYER)
			{
				SmoothCamera.Instance.SetFocusToPlayer();
			}
			if (closestObjectTypeToFocusOn == FocusObjectType.DENDRITIC_CELL)
			{
				GameObject playerObj = FindObjectOfType<DendriticCell>().gameObject;
				Debug.Log(playerObj);
				SmoothCamera.Instance.SetNewFocus(playerObj);
			}

			//if (closestObjectTypeToFocusOn == FocusObjectType.HELPER_CELL)
			//{
			//	GameObject playerObj = FindObjectOfType<HelperTCell>().gameObject;
			//	SmoothCamera.Instance.SetNewFocus(playerObj);
			//}
			//if (closestObjectTypeToFocusOn == FocusObjectType.CANCER)
			//{
			//	GameObject playerObj = FindObjectOfType<Cancer>().gameObject;
			//	SmoothCamera.Instance.SetNewFocus(playerObj);
			//}

		}

		protected override bool OnUpdate()
		{
			if (SmoothCamera.Instance.isCameraFocused)
			{
				return true;
			}
			return false;
		}

		private enum FocusObjectType { NONE, PLAYER, DENDRITIC_CELL, HELPER_CELL, CANCER }
	}
}
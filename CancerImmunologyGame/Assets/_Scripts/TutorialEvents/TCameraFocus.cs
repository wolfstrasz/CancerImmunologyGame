using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TCameraFocus : TutorialEvent
{
	[SerializeField]
	private FocusObjectType closestObjectTypeToFocusOn;

	protected override void OnEnd()
	{
		return;
	}

	protected override void OnStart()
	{
		if (closestObjectTypeToFocusOn == FocusObjectType.PLAYER)
		{
			GameObject playerObj = FindObjectOfType<PlayerController>().gameObject;
			SmoothCamera.Instance.SetNewFocus(playerObj);
		}
		if (closestObjectTypeToFocusOn == FocusObjectType.DENDRITIC_CELL)
		{
			GameObject playerObj = FindObjectOfType<DendriticCell>().gameObject;
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

	private enum FocusObjectType { PLAYER, DENDRITIC_CELL, HELPER_CELL, CANCER }
}

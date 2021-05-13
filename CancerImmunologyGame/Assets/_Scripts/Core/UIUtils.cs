using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

using ImmunotherapyGame.UI;

namespace ImmunotherapyGame.Core
{
	public static class UIUtils
	{

		private static Stack<UINavigationFirstSelected> callerStack = new Stack<UINavigationFirstSelected>();

		public static void RequestUINavigationControl(UINavigationFirstSelected caller)
		{
			Debug.Log("Control Requested: " + caller.gameObject);
			EventSystem.current.SetSelectedGameObject(caller.gameObject);
			callerStack.Push(caller);
		}

		public static void FreeUINavigationControl(UINavigationFirstSelected caller)
		{
			Debug.Log("Control Freed: " + caller.gameObject );

			if (callerStack.Count == 0)
			{
				Debug.LogError("Cannot free control. Caller stack is empty but caller " + caller.gameObject + ") has requested to free control!");
			}

			if (callerStack.Peek() == caller)
			{
				Debug.Log("Removing previous caller");
				callerStack.Pop();
			}

			if (callerStack.Count > 0)
			{
				Debug.Log(" -> Control transfered to " + callerStack.Peek().gameObject);
				EventSystem.current.SetSelectedGameObject(callerStack.Peek().gameObject);
			}
		}
	}



}

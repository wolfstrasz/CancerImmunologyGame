using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

namespace ImmunotherapyGame.Core
{
	public static class InputHandlerUtils
	{

		public static List<string> GetAllKeybindsStrings(InputAction action)
		{
			List<string> strs = new List<string>();


			if (InputSystem.GetDevice<Keyboard>() != null)
			{
				var keyboardIndex = action.GetBindingIndex(group: "Keyboard + Mouse");
				var keyboardString = action.GetBindingDisplayString(keyboardIndex);
				strs.Add(keyboardString);
			}

			if (InputSystem.GetDevice<Gamepad>() != null)
			{
				var gamepadIndex = action.GetBindingIndex(group: "Gamepad");
				var gamepadString = action.GetBindingDisplayString(gamepadIndex);
				strs.Add(gamepadString);
			}

			return strs;
		}

		private static Stack<UINavigationFirstSelected> callerStack = new Stack<UINavigationFirstSelected>();

		public static void RequestUIControl(UINavigationFirstSelected caller)
		{
			Debug.Log("Control Requested: " + caller.gameObject);
			EventSystem.current.SetSelectedGameObject(caller.gameObject);
			callerStack.Push(caller);
		}

		public static void FreeUIControl(UINavigationFirstSelected caller)
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

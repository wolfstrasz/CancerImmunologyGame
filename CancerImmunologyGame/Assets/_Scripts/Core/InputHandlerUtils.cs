using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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

    }

}

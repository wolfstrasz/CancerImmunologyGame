using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ImmunotherapyGame.Core
{

	public static class Utils
	{

		public static List<string> GetAllKeybindsStrings(InputAction action)
		{
			List<string> strs = new List<string>();


			if (InputSystem.GetDevice<Keyboard>() != null)
			{
				var keyboardIndex = action.GetBindingIndex(group: "Keys+Mouse");
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
		/// <summary>
		/// Finds the game object of given type closest to the given location. Uses SqrMagnitude for distance!
		/// </summary>
		/// <typeparam name="GameObjectType"></typeparam>
		/// <param name="point"></param>
		public static GameObject FindClosestGameObjectOfType<GameObjectType>(Vector3 location) where GameObjectType : MonoBehaviour
		{
			GameObjectType[] foundGameObjects = GameObject.FindObjectsOfType<GameObjectType>();
			if (foundGameObjects.Length == 0)
			{
				Debug.LogWarning("A search for a game object returned null! Method cannot find inactive MonoBehaviours.");
				return null;
			}

			if (foundGameObjects.Length == 1)
			{
				return foundGameObjects[0].gameObject;
			}

			float closestDistance = Vector3.SqrMagnitude(foundGameObjects[0].transform.position - location);
			GameObjectType closestObject = foundGameObjects[0];

			foreach (GameObjectType foundObject in foundGameObjects)
			{
				float distance = Vector3.SqrMagnitude(foundObject.transform.position - location);
				if (distance < closestDistance)
				{
					closestDistance = distance;
					closestObject = foundObject;
				}
			}

			return closestObject.gameObject;
		}

		public static List<InterfaceType> FindInterfaces<InterfaceType>(List<GameObject> objectsToSearch)
		{
			List<InterfaceType> interfaces = new List<InterfaceType>();
			foreach (var searchObject in objectsToSearch)
			{
				InterfaceType[] childrenInterfaces = searchObject.GetComponentsInChildren<InterfaceType>();
				foreach (var childInterface in childrenInterfaces)
				{
					interfaces.Add(childInterface);
				}
			}
			return interfaces;
		}


		public static string RemoveNamespacesFromAssemblyType(string name)
		{
			string typename = "";
			for (int i = name.Length - 1; i > 0; --i)
			{
				if (name[i] == '.')
				{
					for (int j = i + 1; j < name.Length; ++j)
					{
						typename += name[j];
					}
					return typename;
				}
			}
			return name;
		}

		public static void LookAt2D (this Transform trans, Vector3 position)
		{
			Quaternion rotation = Quaternion.LookRotation (position, trans.TransformDirection(Vector3.up));
			trans.rotation = new Quaternion(0, 0, rotation.z, rotation.w);
		}

	}
    

    public class ReadOnlyAttribute : PropertyAttribute { }
}


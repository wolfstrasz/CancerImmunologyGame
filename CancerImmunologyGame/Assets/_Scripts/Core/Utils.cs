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

		public static void LookAt2D (this Transform transformToRotate, Vector3 pointToLookAt)
		{
			transformToRotate.right = pointToLookAt - transformToRotate.position;
			//Quaternion rotation = Quaternion.LookRotation (pointToLookAt, transformToRotate.TransformDirection(Vector3.up));
			//transformToRotate.rotation = new Quaternion(0, 0, rotation.z, rotation.w);
		}

		public static GameObject GetClosestObject (List<GameObject> objectsToCheck, GameObject mainObject)
		{
			Vector3 mainObjectPosition = mainObject.transform.position;
			float minDist = 100000f;
			GameObject closestObject = null;
			for (int i = 0; i < objectsToCheck.Count; ++i)
			{
				GameObject target = objectsToCheck[i];

				float distanceSqrt = (target.transform.position - mainObjectPosition).sqrMagnitude;
				if (distanceSqrt < minDist)
				{
					minDist = distanceSqrt;
					closestObject = target;
				}
			}

			return closestObject;
		}

		public static void GetClosestObject<Type>(List<Type> objectsToCheck, Type mainObject, ref Type objectToReturn) where Type : MonoBehaviour
		{
			Vector3 mainObjectPosition = mainObject.transform.position;
			float minDist = 100000f;
			Type closestObject = null;
			for (int i = 0; i < objectsToCheck.Count; ++i)
			{
				Type target = objectsToCheck[i];

				if (!target.gameObject.activeInHierarchy)
				{
					continue;
				}

				float distanceSqrt = (target.transform.position - mainObjectPosition).sqrMagnitude;
				if (distanceSqrt < minDist)
				{
					minDist = distanceSqrt;
					closestObject = target;
				}
			}

			objectToReturn = closestObject;
		}

		public static GameObject GetRandomGameObjectInRange(List<GameObject> possibleObjects, float range, GameObject fromObject)
		{
			List<GameObject> targetsInRange = new List<GameObject>();
			float rangeSqrt = range * range;

			for (int i = 0; i < possibleObjects.Count; ++i)
			{
				GameObject target = possibleObjects[i];

				if (!target.gameObject.activeInHierarchy)
				{
					continue;
				}

				float distanceSqrt = (target.transform.position - fromObject.transform.position).sqrMagnitude;
				if (distanceSqrt < 1.0f || distanceSqrt < rangeSqrt)
				{
					targetsInRange.Add(target);
				}
			}

			return GetRandomGameObject(targetsInRange);
		}

		public static GameObject GetRandomGameObject(List<GameObject> possibleObjects)
		{
			int id = possibleObjects.Count;
			if (id >= 1)
			{
				id = Random.Range(0, id);

				return possibleObjects[id];
			}
			else
			{
				return null;
			}
		}
	}
    
}


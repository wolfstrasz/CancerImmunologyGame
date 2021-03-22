using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{

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
}

public enum CellpediaCells { NONE, TKILLER, THELPER, DENDRITIC, REGULATORY, CANCER}
public enum PlayerUIPanels { PLAYER_INFO_ENERGYBAR, PLAYER_INFO_HEALTHBAR, IMMUNOTHERAPY }

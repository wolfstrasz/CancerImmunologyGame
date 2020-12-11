using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
using System;

public static class GlobalGameData
{
	public static bool isPaused = false;
	public static bool areControlsEnabled = true;
	public static bool isInitialised = false;
    public static bool isInPowerUpMode = false;
	public static float gameplaySpeed = 1.0f;
	public static float gameSpeed = 1.0f;


	public static GameObject player = null;

	private static List<Vector3> RespawnLocations = new List<Vector3>();

	public static Vector3 GetClosestSpawnLocation(Vector3 position)
	{
		if (RespawnLocations.Count == 1)
		{
			return RespawnLocations[0];
		}

		Vector3 closestRespawnLocation = Vector3.zero;
		float minDistance = 100000.0f;

		foreach(var location in RespawnLocations)
		{
			float distance = Vector3.Distance(position, location);
			if (distance <= minDistance)
			{
				minDistance = distance;
				closestRespawnLocation = location;
			}
		}

		return closestRespawnLocation;
	}
}

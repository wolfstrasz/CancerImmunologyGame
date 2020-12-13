using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
using System;

public static class GlobalGameData
{
	public static bool isGameplayPaused = false;
	public static bool areControlsEnabled = true;
	public static bool isInitialised = false;
    public static bool isInPowerUpMode = false;
	public static float gameplaySpeed = 1.0f;
	public static float gameSpeed = 1.0f;

	// Shared object pool
	public static GameObject player = null;

	private static List<PlayerRespawnArea> RespawnAreas = new List<PlayerRespawnArea>();

	public static Vector3 GetClosestSpawnLocation(Vector3 position)
	{
		if (RespawnAreas.Count == 1)
		{
			return RespawnAreas[0].Location;
		}

		Vector3 closestRespawnLocation = Vector3.zero;
		float minDistance = 100000.0f;

		foreach(var area in RespawnAreas)
		{
			float distance = Vector3.Distance(position, area.Location);
			if (distance <= minDistance)
			{
				minDistance = distance;
				closestRespawnLocation = area.Location;
			}
		}

		return closestRespawnLocation;
	}
	public static void AddSpawnLocation(PlayerRespawnArea area)
	{
		RespawnAreas.Add(area);
	}

	public static void RestObjectPool()
	{
		player = null;
		RespawnAreas.Clear();
	}
}

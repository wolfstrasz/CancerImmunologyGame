using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
using System;
using Cancers;

public static class GlobalGameData
{
	public static bool autoAim = false;
	public static bool isGameplayPaused = false;
	public static bool isInitialised = false;
    public static bool isInPowerUpMode = false;
	public static float gameplaySpeed = 1.0f;
	public static float gameSpeed = 1.0f;

	// Shared object pool
	public static GameObject player = null;
	private static List<PlayerRespawnArea> RespawnAreas = new List<PlayerRespawnArea>();
	public static List<Cancer> Cancers = new List<Cancer>();
	public static List<KillerCell> KillerCells = new List<KillerCell>();

	public static Vector3 GetClosestSpawnLocation(Vector3 position)
	{
		if (RespawnAreas.Count == 1)
		{
			return RespawnAreas[0].Location;
		}

		Vector3 closestRespawnLocation = RespawnAreas[0].transform.position;
		float minDistance = Vector3.Distance(position, RespawnAreas[0].Location);

		foreach (var area in RespawnAreas)
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

	public static bool isPowerUpOn = false;

	public static void ResetObjectPool()
	{
		player = null;
		RespawnAreas.Clear();
		Cancers.Clear();
		Cancers.AddRange(GameObject.FindObjectsOfType<Cancer>());
		KillerCells.Clear();
		KillerCells.AddRange(GameObject.FindObjectsOfType<KillerCell>());
	}
}

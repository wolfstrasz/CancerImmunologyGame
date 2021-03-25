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
	public static List<Cancer> Cancers = new List<Cancer>();
	public static List<KillerCell> KillerCells = new List<KillerCell>();
	public static List<PlayerRespawnArea> RespawnAreas = new List<PlayerRespawnArea>();

	public static bool isPowerUpOn = false;

	public static void ResetObjectPool()
	{
		RespawnAreas.Clear();
		RespawnAreas.AddRange(GameObject.FindObjectsOfType<PlayerRespawnArea>());
		Cancers.Clear();
		Cancers.AddRange(GameObject.FindObjectsOfType<Cancer>());
		KillerCells.Clear();
		KillerCells.AddRange(GameObject.FindObjectsOfType<KillerCell>());
	}
}

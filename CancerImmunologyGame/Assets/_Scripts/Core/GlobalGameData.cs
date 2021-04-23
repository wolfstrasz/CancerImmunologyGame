using System.Collections.Generic;
using UnityEngine;
using ImmunotherapyGame.Player;
using ImmunotherapyGame.Cancers;
using ImmunotherapyGame.AI;

namespace ImmunotherapyGame
{
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
		public static List<HelperTCell> HelperTCells = new List<HelperTCell>();
		public static List<RegulatoryCell> RegulatoryCells = new List<RegulatoryCell>();
		public static List<PlayerRespawnArea> RespawnAreas = new List<PlayerRespawnArea>();
		public static List<AIController> AIKillerCells = new List<AIController>();
		public static bool isPowerUpOn = false;

		public static void ResetObjectPool()
		{
			RespawnAreas.Clear();
			RespawnAreas.AddRange(GameObject.FindObjectsOfType<PlayerRespawnArea>(true));
			Cancers.Clear();
			Cancers.AddRange(GameObject.FindObjectsOfType<Cancer>(true));
			KillerCells.Clear();
			KillerCells.AddRange(GameObject.FindObjectsOfType<KillerCell>(true));
			HelperTCells.Clear();
			HelperTCells.AddRange(GameObject.FindObjectsOfType<HelperTCell>(true));
			RegulatoryCells.Clear();
			RegulatoryCells.AddRange(GameObject.FindObjectsOfType<RegulatoryCell>(true));
			AIKillerCells.Clear();
			AIKillerCells.AddRange(GameObject.FindObjectsOfType<AIController>(true));
		}
	}
}
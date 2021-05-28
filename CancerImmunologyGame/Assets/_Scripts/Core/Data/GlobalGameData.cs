﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using ImmunotherapyGame.Core.SystemInterfaces;

namespace ImmunotherapyGame
{
	public static class GlobalGameData
	{
		public static bool isGameplayPaused = false;
		public static bool isInitialised = false;
		public static float gameplaySpeed = 1.0f;
		public static float gameSpeed = 1.0f;

		public static bool isPowerUpOn = false;
		public static bool isInPowerUpMode = false;


		public static List<IDataManager> dataManagers;
	}
}
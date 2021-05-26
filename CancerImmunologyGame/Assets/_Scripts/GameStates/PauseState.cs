﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ImmunotherapyGame.UI.TopOverlay;

namespace ImmunotherapyGame
{
	namespace GameManagement
	{
		public class PauseState : GameState
		{

			private float prevTimeScale = 1.0f;
			public PauseState(GameStateController owner) : base(owner) { }

			internal override void OnFixedUpdate()
			{
			}

			internal override void OnStateEnter()
			{
				prevTimeScale = Time.timeScale;
				Time.timeScale = 0.0f;
				TopOverlayUI.Instance.GamePaused = true;

			}

			internal override void OnStateExit()
			{
				Time.timeScale = prevTimeScale;
			}

			internal override void OnStateReEnter()
			{
				TopOverlayUI.Instance.GamePaused = true;

			}

			internal override void OnUpdate()
			{
			}
		}
	}
}
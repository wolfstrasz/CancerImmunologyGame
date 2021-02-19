using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tutorials;

namespace Core
{
	namespace GameManagement
	{
		public class GameplayPauseState : GameState
		{
			public GameplayPauseState(GameStateController owner) : base(owner) { }


			internal override void OnFixedUpdate()
			{
			}

			internal override void OnStateEnter()
			{
				GlobalGameData.isGameplayPaused = true;
			}

			internal override void OnStateExit()
			{
				GlobalGameData.isGameplayPaused = false;
			}

			internal override void OnUpdate()
			{
				TutorialManager.Instance.OnUpdate();
			}
		}
	}
}
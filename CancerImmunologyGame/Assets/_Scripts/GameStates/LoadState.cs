using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ImmunotherapyGame
{

	namespace GameManagement
	{
		public class LoadState : GameState
		{
			// Used for loading screen
			public LoadState(GameStateController owner) : base(owner) { }

			internal override void OnFixedUpdate()
			{
			}

			internal override void OnStateEnter()
			{
			}

			internal override void OnStateExit()
			{
			}

			internal override void OnUpdate()
			{
				if (GameManager.Instance.sceneLoaded)
				{
					GameManager.Instance.sceneLoaded = false;
					owner.SetState(new PlayState(owner));
				}
			}

		}
	}
}

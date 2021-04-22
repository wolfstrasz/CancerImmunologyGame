using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ImmunotherapyGame
{
	namespace GameManagement
	{
		public abstract class GameState
		{

			protected GameStateController owner;

			internal protected GameState(GameStateController owner)
			{
				this.owner = owner;
			}

			internal abstract void OnStateExit();
			internal abstract void OnStateEnter();
			internal abstract void OnUpdate();
			internal abstract void OnFixedUpdate();
		}
	}
}
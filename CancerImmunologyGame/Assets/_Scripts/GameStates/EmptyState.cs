using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ImmunotherapyGame.GameManagement
{
	public class EmptyState : GameState
	{
		public EmptyState(GameStateController owner) : base(owner) { }
		internal override void OnFixedUpdate()
		{
		}

		internal override void OnStateEnter()
		{
			Debug.Log("Entering: Empty State");

		}

		internal override void OnStateExit()
		{
		}

		internal override void OnStateReEnter()
		{
			Debug.Log("Re-Entering: Empty State");

		}

		internal override void OnUpdate()
		{
		}
	}
}

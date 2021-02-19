using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
	namespace GameManagement
	{
		public class GameStateController
		{
			protected Stack<GameState> stateHistory = new Stack<GameState>();
			protected GameState activeState = null;

			internal GameStateController()
			{
				stateHistory = new Stack<GameState>();
				AddState(new MainMenuState(this));
			}

			internal void OnUpdate()
			{
				activeState.OnUpdate();
			}

			internal void OnFixedUpdate()
			{
				activeState.OnFixedUpdate();
			}

			internal void RemoveAllStates()
			{
				while (stateHistory.Count > 0)
				{
					stateHistory.Peek().OnStateExit();
					stateHistory.Pop();
				}
				activeState.OnStateExit();
				activeState = null;
			}

			internal void AddState(GameState state)  // Push new on stack
			{
				if (activeState != null)
					stateHistory.Push(activeState);
				activeState = state;
				activeState.OnStateEnter();
			}

			internal void SetState(GameState state) // Replaces the state (pop + push)
			{
				if (activeState != null)
					activeState.OnStateExit();
				if (stateHistory.Count != 0)
					stateHistory.Pop();
				activeState = state;
				activeState.OnStateEnter();
			}

			internal void RemoveState() // Removes the state and continues with the previous state
			{

				if (stateHistory.Count <= 0)
				{
					// On reaching this error means that your State machine will have had no STATE to exist in
					Debug.LogError("StackedFSM.ExitState(): State machine in has no states in history to replace current state.");
					return;
				}

				activeState.OnStateExit();
				activeState = stateHistory.Pop();
			}


		}
	}
}
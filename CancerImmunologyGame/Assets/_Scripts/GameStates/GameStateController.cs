using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ImmunotherapyGame
{
	namespace GameManagement
	{
		public class GameStateController
		{
			protected Stack<GameObject> pauseStateCallers = new Stack<GameObject>();
			protected Stack<GameState> stateHistory = new Stack<GameState>();
			protected GameState activeState = null;

			internal GameStateController()
			{
				stateHistory = new Stack<GameState>();
				SetState(new EmptyState(this));
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

				if (activeState != null)
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
				// Clear all previous states
				RemoveAllStates();
				pauseStateCallers.Clear();

				// Assign new state
				activeState = state;
				activeState.OnStateEnter();
			}

			internal bool AddPauseState(GameObject caller, GameState state)
			{
				AddState(state);
				pauseStateCallers.Push(caller);
				return true;
			}

			internal bool RemovePauseState (GameObject caller)
			{
				if (caller == pauseStateCallers.Peek())
				{
					if (pauseStateCallers.Count <= 0)
					{
						Debug.LogWarning("StackedFSM.RemovePauseState: State machine has an empty record of pause callers. Cannot free pause state requested from: " + caller.name);
						pauseStateCallers.Pop();
						return false;
					}
					return RemoveCurrentState(caller.name);
				
				}
				Debug.LogWarning("StackedFSM.RemovePauseStat: Caller requesting to free pause is not the current owner");
				return false;
			}

			internal bool RemoveCurrentState(string callerName) // Removes the state and continues with the previous state
			{
				Debug.Log("StackedFSM.RemoveCurrentState() requested by: " + callerName);
				if (stateHistory.Count <= 0)
				{
					// On reaching this error means that your State machine will have had no STATE to exist in
					Debug.LogError("StackedFSM.RemoveCurrentState(): State machine in has no states in history to replace current state. Caller Name: " + callerName);
					return false;
				}

				activeState.OnStateExit();
				activeState = stateHistory.Pop();
				return true;
			}


		}
	}
}
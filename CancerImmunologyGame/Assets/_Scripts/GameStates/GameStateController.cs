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
				if (activeState == null)
				{
					Debug.Log("Game State Controller contructor: create empty state");
					SetState(new EmptyState(this));
				}
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
				Debug.Log("GSC: Add new state " + state.ToString());

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
				Debug.Log("GSC: Set new state " + state.ToString());
				activeState = state;
				activeState.OnStateEnter();
			}

			internal bool AddPauseState(GameObject caller, GameState state)
			{
				Debug.Log("GSC: Add new pause state request from: " + caller.name);

				AddState(state);
				pauseStateCallers.Push(caller);
				return true;
			}

			internal bool RemovePauseState (GameObject caller)
			{
				Debug.Log("GSC: Remove pause state request from: " + caller.name);

				if (caller == pauseStateCallers.Peek())
				{
					if (pauseStateCallers.Count <= 0)
					{
						Debug.LogWarning("GSC: State machine has an empty record of pause callers. Cannot free pause state requested from: " + caller.name);
						return false;
					}
					return RemoveCurrentState(caller.name);
				
				}
				Debug.LogWarning("GCS: Caller requesting to free pause is not the current owner. Called by " + caller.name + " but owner is: " + pauseStateCallers.Peek().name);
				return false;
			}

			internal bool RemoveCurrentState(string callerName) // Removes the state and continues with the previous state
			{
				Debug.Log("GSC: remove current state accepted for: " + callerName);
				if (stateHistory.Count <= 0)
				{
					// On reaching this error means that your State machine will have had no STATE to exist in
					Debug.LogError("StackedFSM.RemoveCurrentState(): State machine in has no states in history to replace current state. Caller Name: " + callerName);
					return false;
				}

				activeState.OnStateExit();
				activeState = stateHistory.Pop();
				Debug.Log("GSC: returning to state " + activeState.ToString());
				activeState.OnStateReEnter();
				pauseStateCallers.Pop();
				return true;
			}


		}
	}
}
using System.Collections.Generic;
using UnityEngine;
using BehaviourTreeBase;
using ImmunotherapyGame.Cancers;
using ImmunotherapyGame.Core;

namespace ImmunotherapyGame.AI
{
	public class AIFindACellTarget : BTActionNode
	{

		AIControlledCellData controlledCellData;
		AICombatData cancerData;
		AITargetingData targetData;

		public AIFindACellTarget(string name, BehaviourTree owner, AIControlledCellData controlledCellData, AICombatData cancerData, AITargetingData targetData ) : base(name, owner, "AIFindACellTarget")
		{
			this.controlledCellData = controlledCellData;
			this.cancerData = cancerData;
			this.targetData = targetData;
		}


		protected override NodeStates OnEvaluateAction()
		{

			GameObject currentTarget = targetData.currentTarget;
			if (currentTarget != null && currentTarget.activeInHierarchy && currentTarget.GetComponent<Cell>() != null)
			{
				nodeState = NodeStates.SUCCESS;
				return nodeState;
			}

			Cancer targetCancer = null;
			targetCancer = cancerData.currentCancerTarget;

			if (targetCancer == null && !cancerData.currentCancerTarget.IsAlive)

			{
				nodeState = NodeStates.FAILURE;
				return nodeState;
			}

			Cell closestCellToTarget = null;
			Utils.GetClosestObject<Cell>(targetCancer.AllCells, controlledCellData.controlledCell, ref closestCellToTarget);

			Debug.Log("Evaluating AIFindEVILCELLS3");
			GameObject objectFound = closestCellToTarget.gameObject;

			if (objectFound == null)
			{
				nodeState = NodeStates.FAILURE;
				targetData.currentTarget = null;
				targetData.acceptableDistanceFromCurrentTarget = 0f;
				return nodeState;
			}
			else
			{
				targetData.currentTarget = objectFound;
				targetData.acceptableDistanceFromCurrentTarget = cancerData.acceptableDistance;
				nodeState = NodeStates.SUCCESS;
				return nodeState;
			}
		}

		protected override void OnResetTreeNode()
		{

		}
	}
}
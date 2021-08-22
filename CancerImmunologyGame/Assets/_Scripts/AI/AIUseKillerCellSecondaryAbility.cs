using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTreeBase;
using ImmunotherapyGame.Core;

namespace ImmunotherapyGame.AI
{
    public class AIUseKillerCellSecondaryAbility : BTActionNode
    {
		AIControlledCellData controlledCellData;

		public AIUseKillerCellSecondaryAbility(string name, BehaviourTree owner, AIControlledCellData controlledCellData) : base(name, owner, "AIUseKillerCellSecondaryAbility")
		{
			this.controlledCellData = controlledCellData;
		}

		protected override NodeStates OnEvaluateAction()
		{
			KillerCell controlledKillerCell = controlledCellData.controlledCell.GetComponent<KillerCell>();

			if (controlledKillerCell != null)
			{
				if (controlledKillerCell.SecondaryAbilityCaster == null)
				{
					nodeState = NodeStates.FAILURE;
					return nodeState;
				} 
				else if (controlledKillerCell.CanUseSecondaryAttack)
				{

					// Get a target to shoot at
					List<GameObject> availableTargets = controlledKillerCell.SecondaryAbilityCaster.TargetsInRange;
					GameObject target = Utils.GetClosestObject(availableTargets, controlledCellData.controlledObject);
					controlledKillerCell.UseSecondaryAttack(target);
					nodeState = NodeStates.SUCCESS;
					return nodeState;
				}

			}

			nodeState = NodeStates.FAILURE;
			return nodeState;
		}

		protected override void OnResetTreeNode()
		{
		}

	}
}

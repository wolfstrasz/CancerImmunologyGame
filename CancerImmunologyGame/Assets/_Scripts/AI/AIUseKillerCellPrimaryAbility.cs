using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTreeBase;
using ImmunotherapyGame.Core;

namespace ImmunotherapyGame.AI
{
    public class AIUseKillerCellPrimaryAbility : BTActionNode
    {
		AIControlledCellData controlledCellData;

		public AIUseKillerCellPrimaryAbility(string name, BehaviourTree owner, AIControlledCellData controlledCellData) : base(name, owner, "AIUseKillerCellPrimaryAbility")
		{
			this.controlledCellData = controlledCellData;
		}

		protected override NodeStates OnEvaluateAction()
		{
			KillerCell controlledKillerCell = controlledCellData.controlledCell.GetComponent<KillerCell>();

			if (controlledKillerCell != null)
			{
				if (controlledKillerCell.PrimaryAbilityCaster == null)
				{
					nodeState = NodeStates.FAILURE;
					return nodeState;
				}
				else if (controlledKillerCell.CanUsePrimaryAttack)
				{

					// Get a target to shoot at
					List<GameObject> availableTargets = controlledKillerCell.PrimaryAbilityCaster.TargetsInRange;
					GameObject target = Utils.GetClosestObject(availableTargets, controlledCellData.controlledObject);
					Debug.Log("Found Target :" + target);
					controlledKillerCell.UsePrimaryAttack(target);

					// using NOT due to inverter killer cell sprite
					controlledKillerCell.FlipSpriteLocalTransform = !(target.transform.position.x <= controlledKillerCell.transform.position.x);
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

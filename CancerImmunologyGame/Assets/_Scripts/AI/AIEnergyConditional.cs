using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTreeBase;

namespace ImmunotherapyGame.AI
{
	public class AIEnergyConditional : BTActionNode
	{
		AIControlledCellData controlledCellData;
		ValueConditionalOperator op;
		float percentageValue;

		public AIEnergyConditional(string name, BehaviourTree owner, AIControlledCellData controlledCellData, ValueConditionalOperator op, float percentageValue) : base(name, owner, "AIEnergyConditional")
		{
			this.name = name;
			this.owner = owner;
			this.controlledCellData = controlledCellData;
			this.percentageValue = percentageValue;
			this.op = op;

		}

		protected override NodeStates OnEvaluateAction()
		{
			bool result = false;
			float energyPercentage = controlledCellData.controlledCell.CurrentEnergyPercentage;

			switch (op)
			{
				case ValueConditionalOperator.LESS_THAN:
					result = (energyPercentage < percentageValue);
					break;
				case ValueConditionalOperator.LESS_THAN_EQUAL:
					result = (energyPercentage <= percentageValue);
					break;
				case ValueConditionalOperator.EQUAL:
					result = (energyPercentage == percentageValue);
					break;
				case ValueConditionalOperator.MORE_THAN:
					result = (energyPercentage > percentageValue);
					break;
				case ValueConditionalOperator.MORE_THAN_EQUAL:
					result = (energyPercentage >= percentageValue);
					break;
				default: result = false; break;
			}

			nodeState = result ? NodeStates.SUCCESS : NodeStates.FAILURE;
			return nodeState;
		}

		protected override void OnResetTreeNode()
		{
		}
	}
}

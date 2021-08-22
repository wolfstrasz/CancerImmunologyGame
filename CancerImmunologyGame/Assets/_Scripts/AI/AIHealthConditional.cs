using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTreeBase;

namespace ImmunotherapyGame.AI
{
	public class AIHealthConditional : BTActionNode
	{
		AIControlledCellData controlledCellData;
		ValueConditionalOperator op;
		float percentageValue;

		public AIHealthConditional(string name, BehaviourTree owner, AIControlledCellData controlledCellData, ValueConditionalOperator op, float percentageValue) : base(name, owner, "AIHealthConditional")
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
			float healthPercentage = controlledCellData.controlledCell.CurrentHealthPercentage;
			switch (op)
			{
				case ValueConditionalOperator.LESS_THAN:
					result = (healthPercentage < percentageValue);
					break;
				case ValueConditionalOperator.LESS_THAN_EQUAL:
					result = (healthPercentage <= percentageValue);
					break;
				case ValueConditionalOperator.EQUAL:
					result = (healthPercentage == percentageValue);
					break;
				case ValueConditionalOperator.MORE_THAN:
					result = (healthPercentage > percentageValue);
					break;
				case ValueConditionalOperator.MORE_THAN_EQUAL:
					result = (healthPercentage >= percentageValue);
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

	public enum ValueConditionalOperator { LESS_THAN, LESS_THAN_EQUAL, EQUAL, MORE_THAN, MORE_THAN_EQUAL }
}
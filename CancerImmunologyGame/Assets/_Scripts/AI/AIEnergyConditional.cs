using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTreeBase;

public class AIEnergyConditional : BTActionNode
{
	IAICellController controller;
	ValueConditionalOperator op;
	float value;

	public AIEnergyConditional(string name, BehaviourTree owner, IAICellController controller, ValueConditionalOperator op, float value) : base (name, owner, "EnergyConditional")
	{
		this.name = name;
		this.owner = owner;
		this.controller = controller;
		this.value = value;
		this.op = op;

	}

	protected override NodeStates OnEvaluateAction()
	{
		bool result = false;

		switch (op)
		{
			case ValueConditionalOperator.LESS_THAN:
				result = (controller.ControlledCell.Energy < value);
				break;
			case ValueConditionalOperator.LESS_THAN_EQUAL:
				result = (controller.ControlledCell.Energy <= value);
				break;
			case ValueConditionalOperator.EQUAL:
				result = (controller.ControlledCell.Energy == value);
				break;
			case ValueConditionalOperator.MORE_THAN:
				result = (controller.ControlledCell.Energy > value);
				break;
			case ValueConditionalOperator.MORE_THAN_EQUAL:
				result = (controller.ControlledCell.Energy >= value);
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
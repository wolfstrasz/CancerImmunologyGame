using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTreeBase;


public class AIHealthConditional : BTActionNode
{
	IAICellController controller;
	ValueConditionalOperator op;
	float value;

	public AIHealthConditional(string name, BehaviourTree owner, IAICellController controller, ValueConditionalOperator op, float value ) : base (name, owner, "HealthConditional")
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
				result = (controller.ControlledCell.Health < value);
				break;
			case ValueConditionalOperator.LESS_THAN_EQUAL:
				result = (controller.ControlledCell.Health <= value);
				break;
			case ValueConditionalOperator.EQUAL:
				result = (controller.ControlledCell.Health == value);
				break;
			case ValueConditionalOperator.MORE_THAN:
				result = (controller.ControlledCell.Health > value);
				break;
			case ValueConditionalOperator.MORE_THAN_EQUAL:
				result = (controller.ControlledCell.Health >= value);
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

	public enum ValueConditionalOperator { LESS_THAN, LESS_THAN_EQUAL, EQUAL, MORE_THAN, MORE_THAN_EQUAL}

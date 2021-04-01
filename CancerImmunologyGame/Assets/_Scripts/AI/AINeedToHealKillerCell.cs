using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTreeBase;

public class AINeedToHealKillerCell : BTActionNode
{
	IAIKillerCellController controller;

	public AINeedToHealKillerCell(string name, BehaviourTree owner, IAIKillerCellController controller) : base(name, owner, "")
	{
		this.controller = controller;
	}

	protected override NodeStates OnEvaluateAction()
	{
		if (controller.GetControlledCell().Health * 2 < KillerCell.maxHealth)
		{
			return NodeStates.SUCCESS;
		}
		return NodeStates.FAILURE;
	}

	protected override void OnResetTreeNode()
	{
	}

}

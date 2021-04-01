using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTreeBase;

public class AINeedToHealKillerCell : BTActionNode
{
	IAICellController controller;

	public AINeedToHealKillerCell(string name, BehaviourTree owner, IAICellController controller) : base(name, owner, "NeedToHeal")
	{
		this.controller = controller;
	}

	protected override NodeStates OnEvaluateAction()
	{
		if (controller.ControlledCell.Health * 3 < KillerCell.maxHealth)
		{
			return NodeStates.SUCCESS;
		}
		else if (controller.ControlledCell.Energy * 3 < KillerCell.maxEnergy)
		{
			return NodeStates.SUCCESS;
		}
		return NodeStates.FAILURE;
	}

	protected override void OnResetTreeNode()
	{
	}

}

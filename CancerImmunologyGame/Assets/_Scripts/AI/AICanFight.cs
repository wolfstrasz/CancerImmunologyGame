using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTreeBase;


public class AICanFight : BTActionNode
{

	IAICellController controller;

	public AICanFight(string name, BehaviourTree owner, IAICellController controller) : base(name, owner, "CanFight")
	{
		this.controller = controller;
	}

	protected override NodeStates OnEvaluateAction()
	{
		
		if (controller.ControlledCell.Health == controller.ControlledCell.maxHealth && controller.ControlledCell.Energy == controller.ControlledCell.maxEnergy)
		{
			nodeState = NodeStates.SUCCESS;
			return nodeState;
		}

		nodeState = NodeStates.FAILURE;
		return nodeState;


	}

	protected override void OnResetTreeNode()
	{
	}

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTreeBase;

public class AINeedToHeal : BTActionNode
{
	KillerCell monitoredCell = null;

	public AINeedToHeal(string name, BehaviourTree owner, KillerCell monitoredCell) : base(name, owner, "")
	{
		this.monitoredCell = monitoredCell;
	}

	protected override NodeState OnEvaluateAction()
	{
		if (monitoredCell.Health * 2 < KillerCell.maxHealth)
		{
			return NodeState.SUCCESS;
		}
		return NodeState.FAILURE;
	}

	protected override void OnResetTreeNode()
	{
	}

}

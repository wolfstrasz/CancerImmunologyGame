using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTreeBase;
using Cells;

public class AIHasHealingTarget : BTActionNode
{
	private IAIHelperCellInteractor handler;
	public AIHasHealingTarget(string name, BehaviourTree owner, IAIHelperCellInteractor handler) : base (name, owner, "WaitingToHeal")
	{
		this.handler = handler;
	}

	protected override NodeStates OnEvaluateAction()
	{
		if (handler.Target == null || handler.BookedHelperTCell == null)
		{
			nodeState = NodeStates.FAILURE;
			return nodeState;
		}
	
		nodeState = NodeStates.SUCCESS;
		return nodeState;
	}

	protected override void OnResetTreeNode()
	{
	}


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTreeBase;
using Cancers;


public class AIAttackCancerCell : BTActionNode
{
	private IAICancerCellInteractor interactor = null;

	public AIAttackCancerCell(string name, BehaviourTree owner, IAICancerCellInteractor interactor) : base(name, owner, "Attack Cancer Cell")
	{
		this.interactor = interactor;
	}


	protected override NodeStates OnEvaluateAction()
	{

		if (interactor.TargetedCancerCell != null)
		{
			interactor.ControlledCell.Attack(interactor.TargetedCancerCell.transform.position);

			nodeState = NodeStates.RUNNING;
			return nodeState;
		}
		else
		{
			nodeState = NodeStates.FAILURE;
			return nodeState;
		}
	}

	protected override void OnResetTreeNode()
	{
	}

}

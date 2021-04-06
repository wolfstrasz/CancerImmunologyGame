using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTreeBase;

public class AICanAttackCancerCell : BTActionNode
{
	private IAICancerCellInteractor interactor;

	public AICanAttackCancerCell(string name, BehaviourTree owner, IAICancerCellInteractor interactor) : base(name, owner, "CanAttackCancerCell")
	{
		this.interactor = interactor;
	}


	protected override NodeStates OnEvaluateAction()
	{
		if (interactor.TargetedCancerCell == null || interactor.Target == null)
		{
			nodeState = NodeStates.FAILURE;
			return nodeState;
		}

		if (Vector3.SqrMagnitude(interactor.ControlledCell.transform.position - interactor.TargetedCancerCell.transform.position) < interactor.ControlledCell.Range * interactor.ControlledCell.Range)
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTreeBase;
using Cells;


public class AIFindEvilCellTarget : BTActionNode
{

	private IAICancerCellInteractor interactor;

	public AIFindEvilCellTarget(string name, BehaviourTree owner, IAICancerCellInteractor interactor) : base(name, owner, "AIFindClosestTarget")
	{
		this.interactor = interactor;
	}


	protected override NodeStates OnEvaluateAction()
	{

		//if (controller.TargetedCancerCell != null)
		//{
		//	nodeState = NodeStates.SUCCESS;
		//	return nodeState;
		//}

		GameObject objectFound = Utils.FindClosestGameObjectOfType<EvilCell>(interactor.ControlledCell.transform.position);

		if (objectFound == null)
		{
			nodeState = NodeStates.FAILURE;
			return nodeState;
		}
		else
		{
			interactor.Target = objectFound;
			interactor.AcceptableDistanceFromTarget = interactor.ControlledCell.Range;
			interactor.TargetedEvilCell = objectFound.GetComponent<EvilCell>();
			nodeState = NodeStates.SUCCESS;
			return nodeState;
		}
	}

	protected override void OnResetTreeNode()
	{

	}
}

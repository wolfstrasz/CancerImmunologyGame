using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTreeBase;


public class AIFindClosestTargetOfType<TypeOfTarget> : BTActionNode where TypeOfTarget : MonoBehaviour
{

	private IAIKillerCellController controller;

	public AIFindClosestTargetOfType(string name, BehaviourTree owner, IAIKillerCellController controller) : base(name, owner, "AIFindClosestTarget")
	{
		this.controller = controller;
	}

	protected override NodeStates OnEvaluateAction()
	{
		GameObject objectFound = Utils.FindClosestGameObjectOfType<TypeOfTarget>(controller.GetControlledCellTransform().position);

		if (objectFound == null)
		{
			nodeState = NodeStates.FAILURE;
			return nodeState;
		} else
		{
			controller.SetTarget(objectFound);
			nodeState = NodeStates.SUCCESS;
			return nodeState;
		}
	}

	protected override void OnResetTreeNode()
	{

	}
}

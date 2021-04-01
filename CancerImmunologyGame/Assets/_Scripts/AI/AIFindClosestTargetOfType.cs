using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTreeBase;


public class AIFindClosestTargetOfType<TypeOfTarget> : BTActionNode where TypeOfTarget : MonoBehaviour
{

	private IAITargetHandler controller;

	public AIFindClosestTargetOfType(string name, BehaviourTree owner, IAITargetHandler controller) : base(name, owner, "AIFindClosestTarget")
	{
		this.controller = controller;
	}

	protected override NodeStates OnEvaluateAction()
	{
		GameObject objectFound = Utils.FindClosestGameObjectOfType<TypeOfTarget>(controller.ControlledCell.transform.position);

		if (objectFound == null)
		{
			nodeState = NodeStates.FAILURE;
			return nodeState;
		} else
		{
			controller.Target = objectFound;
			controller.AcceptableDistanceFromTarget = 1f;
			nodeState = NodeStates.SUCCESS;
			return nodeState;
		}
	}

	protected override void OnResetTreeNode()
	{

	}
}

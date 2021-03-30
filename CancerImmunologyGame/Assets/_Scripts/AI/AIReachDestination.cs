using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTreeBase;
using Pathfinding;
public class AIReachDestination : BTActionNode
{

	private AIDestinationSetter setter;
	private AIPath mover;
	private Transform destination;
	private float distanceToAccept = 1.0f;
	private bool activated = false;


	public AIReachDestination(string name, BehaviourTree owner, AIDestinationSetter setter, AIPath mover, Transform destination, float distanceToAccept = 1.0f) : base (name, owner, "AIReachDestination")
	{
		this.setter = setter;
		this.mover = mover;
		this.destination = destination;
		this.distanceToAccept = distanceToAccept;
	}

	protected override NodeState OnEvaluateAction()
	{
		if (activated)
		{
			if (mover.reachedEndOfPath)
			{
				return NodeState.SUCCESS;
			}
			return nodeState;
		}

		setter.target = destination;
		mover.canMove = true;
		return NodeState.RUNNING;
	}

	protected override void OnResetTreeNode()
	{
		activated = false;
		mover.canMove = false;
	}
}

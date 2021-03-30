using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTreeBase;

public class AIWait : BTActionNode
{
	private float timeToWait = 0f;
	private float timeWaited = 0f;
	public AIWait (string name, BehaviourTree owner, float time, bool force = false) : base (name, owner, "AIWait")
	{
		timeToWait = time;
		timeWaited = 0f;
		allowTreeToReevaluate = !force;
	}

	protected override NodeState OnEvaluateAction()
	{
		timeWaited += Time.deltaTime;
		if (timeWaited < timeToWait)
		{
			return NodeState.RUNNING;
		}
		timeWaited = 0f; // Reset
		return NodeState.SUCCESS;
	}

	protected override void OnResetTreeNode()
	{
		timeWaited = 0f;
	}
}

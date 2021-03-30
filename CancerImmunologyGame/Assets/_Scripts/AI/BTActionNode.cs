using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTreeBase
{

	public abstract class BTActionNode : BTNode
	{
		public BehaviourTree owner = null;

		protected abstract NodeState OnEvaluateAction();
		protected abstract void OnResetTreeNode();

		internal override NodeState Evaluate()
		{
			if (owner != null)
			{
				owner.currentProcessingNode = this;
			}

			if (nodeState != NodeState.RUNNING)
			{
				return NodeState;
			}

			nodeState = OnEvaluateAction();
			Debug.Log(name + "\t has been evaluated to " + nodeState);
			return nodeState;
		}
		 
		internal override void ResetTreeNode()
		{
			nodeState = NodeState.RUNNING;
			OnResetTreeNode();
		}

		public BTActionNode (string name, BehaviourTree owner, string actionName)
		{
			this.name = name + "(" + actionName + ")";
			this.owner = owner;
		}

	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTreeBase
{

	public abstract class BTActionNode : BTNode
	{
		public BehaviourTree owner = null;

		protected abstract NodeStates OnEvaluateAction();
		protected abstract void OnResetTreeNode();

		internal override NodeStates Evaluate()
		{
			if (owner != null)
			{
				owner.currentProcessingNode = this;
			}

			if (nodeState != NodeStates.RUNNING)
			{
				return NodeState;
			}

			nodeState = OnEvaluateAction();
			//Debug.Log(name + "\t has been evaluated to " + nodeState);
			if (nodeState != NodeStates.RUNNING)
			{
				Debug.Log(nodeState + "\t from node " + name);
			}
			return nodeState;
		}
		 
		internal override void ResetTreeNode()
		{
			nodeState = NodeStates.RUNNING;
			OnResetTreeNode();
		}

		public BTActionNode (string name, BehaviourTree owner, string actionName)
		{
			this.name = name + "(" + actionName + ")";
			this.owner = owner;
		}

		protected void ActionNodeLogWarning (string message)
		{
			Debug.LogWarning(name + " : " + message);
		}
	}
}
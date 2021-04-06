using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTreeBase
{
	[System.Serializable]
	public abstract class BTNode
	{
		internal protected bool allowTreeToReevaluate = true;
		// Delegate that returns the state of the node.
		internal delegate NodeStates NodeReturn();

		// The current state of the node 
		[SerializeReference]
		protected NodeStates nodeState = NodeStates.RUNNING;
		[SerializeReference]
		protected string name = "unknownNode";

		public NodeStates NodeState => nodeState;

		// The constructor for the node 
		public BTNode() { }

		//Implementing classes use this method to evaluate the desired set of conditions
		internal abstract NodeStates Evaluate();

		internal abstract void ResetTreeNode();
	}

	[System.Serializable]
	public enum NodeStates { SUCCESS, FAILURE, RUNNING }
}
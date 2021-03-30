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
		internal delegate NodeState NodeReturn();

		// The current state of the node 
		[SerializeReference]
		protected NodeState nodeState = NodeState.RUNNING;
		[SerializeReference]
		protected string name = "unknownNode";

		public NodeState NodeState => nodeState;

		// The constructor for the node 
		public BTNode() { }

		//Implementing classes use this method to evaluate the desired set of conditions
		internal abstract NodeState Evaluate();

		internal abstract void ResetTreeNode();


	
	}

	[System.Serializable]
	public enum NodeState { SUCCESS, FAILURE, RUNNING }
}
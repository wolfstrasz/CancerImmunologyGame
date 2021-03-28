using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTreeBase
{
	public abstract class BTNode
	{
		// Delegate that returns the state of the node.
		internal delegate NodeState NodeReturn();

		// The current state of the node 
		protected NodeState nodeState;
		protected string name;

		public NodeState NodeState => NodeState;

		// The constructor for the node 
		public BTNode() { }

		//Implementing classes use this method to evaluate the desired set of conditions
		public abstract NodeState Evaluate();
	}

	public enum NodeState { SUCCESS, FAILURE, RUNNING }
}
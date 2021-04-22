using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace BehaviourTreeBase
{
	public class BTInverter : BTDecorator
	{

		public BTInverter(string name, BTNode node) : base (name, node)
		{
			Assert.IsTrue(node != null, "BTInverter (" + name + ") must have a child node!");
		}

		internal override NodeStates Evaluate()
		{
			switch (node.Evaluate())
			{
				case NodeStates.FAILURE:
					nodeState = NodeStates.SUCCESS;
					return NodeState;
				case NodeStates.SUCCESS:
					nodeState = NodeStates.FAILURE;
					return nodeState;
				case NodeStates.RUNNING:
					nodeState = NodeStates.RUNNING;
					return nodeState;
				default:
					nodeState = NodeStates.FAILURE;
					return nodeState;
			}
		}
	}

}
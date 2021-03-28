using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace BehaviorTreeBase
{
	public class BTInverter : BTNode
	{

		protected BTNode node = null;

		public BTInverter(string name, BTNode node)
		{
			this.node = node;
			this.name = name;
			Assert.IsTrue(node != null, "BTInverter (" + name + ") must have a child node!");
		}

		public override NodeState Evaluate()
		{
			switch (node.Evaluate())
			{
				case NodeState.FAILURE:
					nodeState = NodeState.SUCCESS;
					return NodeState;
				case NodeState.SUCCESS:
					nodeState = NodeState.FAILURE;
					return nodeState;
				case NodeState.RUNNING:
					nodeState = NodeState.RUNNING;
					return nodeState;
				default:
					nodeState = NodeState.FAILURE;
					return nodeState;
			}
		}
	}

}
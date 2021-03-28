using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace BehaviorTreeBase
{
	public class BTSequence : BTNode
	{

		protected List<BTNode> nodes = new List<BTNode>();


		public BTSequence(string name, List<BTNode> nodes)
		{
			this.name = name;
			this.nodes = nodes;

			Assert.IsTrue((nodes != null || nodes.Count > 0), "BTSequence (" + name + ") must have atleast one child node!");
		}

		public override NodeState Evaluate()
		{
			foreach (BTNode node in nodes)
			{
				switch (node.Evaluate())
				{
					case NodeState.FAILURE:
						nodeState = NodeState.FAILURE;
						return nodeState;
					case NodeState.SUCCESS:
						continue;
					case NodeState.RUNNING:
						nodeState = NodeState.RUNNING;
						return nodeState;
					default:
						nodeState = NodeState.SUCCESS;
						return nodeState;
				}
			}
			nodeState = NodeState.SUCCESS;
			return nodeState;
		}
	}
}

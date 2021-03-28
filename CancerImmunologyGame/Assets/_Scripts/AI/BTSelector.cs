using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace BehaviorTreeBase
{
	public class BTSelector : BTNode
	{

		// The child nodes for this selector
		protected List<BTNode> nodes = new List<BTNode>();

		/// <summary>
		/// The constructor requires a lsit of child nodes to be passed in.
		/// </summary>
		public BTSelector(string name, List<BTNode> nodes)
		{
			this.name = name;
			this.nodes = nodes;

			Assert.IsTrue((nodes != null || nodes.Count > 0), "BTSelector (" + name + ") must have atleast one child node!");

		}


		/// <summary>
		/// If any of the children reports a success, the selector will
		/// immediately report a success upwards.If all children fail,
		/// it will report a failure instead.
		/// </summary>
		/// <returns></returns>
		public override NodeState Evaluate()
		{
			foreach (BTNode node in nodes)
			{
				switch (node.Evaluate())
				{
					case NodeState.FAILURE:
						continue;
					case NodeState.SUCCESS:
						nodeState = NodeState.SUCCESS;
						return nodeState;
					case NodeState.RUNNING:
						nodeState = NodeState.RUNNING;
						return nodeState;
					default:
						continue;
				}
			}
			nodeState = NodeState.FAILURE;
			return nodeState;
		}
	}

}
 
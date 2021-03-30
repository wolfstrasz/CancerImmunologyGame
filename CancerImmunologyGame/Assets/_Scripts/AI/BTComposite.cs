using System.Collections.Generic;
using UnityEngine.Assertions;

namespace BehaviourTreeBase
{
	public abstract class BTComposite : BTNode
	{
		protected List<BTNode> nodes = new List<BTNode>();


		/// <summary>
		/// The constructor requires a name and list of child nodes to be passed in.
		/// </summary>
		public BTComposite(string namea, List<BTNode> nodes)
		{
			this.name = namea;
			this.nodes = nodes;

			Assert.IsTrue((nodes != null || nodes.Count > 0), "BTComposite (" + name + ") must have atleast one child node!");
		}


		internal override void ResetTreeNode()
		{
			nodeState = NodeState.RUNNING;

			foreach ( BTNode node in nodes)
			{
				node.ResetTreeNode();
			}
		}
	}
}
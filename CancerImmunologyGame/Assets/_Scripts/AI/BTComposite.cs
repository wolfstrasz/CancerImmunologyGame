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
		protected BTComposite(string name, List<BTNode> nodes)
		{
			this.name = name;
			this.nodes = nodes;

			//Assert.IsTrue((nodes != null || nodes.Count > 0), "BTComposite (" + base.name + ") must have atleast one child node!");
		}

		protected BTComposite(string name, int nodeCount)
		{
			this.name = name;
			nodes = new List<BTNode>(nodeCount);
		}

		public void AddNode(BTNode node)
		{
			nodes.Add(node);
		}

		internal override void ResetTreeNode()
		{
			nodeState = NodeStates.RUNNING;

			foreach ( BTNode node in nodes)
			{
				node.ResetTreeNode();
			}
		}
	}
}
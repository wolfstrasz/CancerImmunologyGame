using UnityEngine.Assertions;

namespace BehaviourTreeBase
{
	public abstract class BTDecorator : BTNode
	{
		protected BTNode node = null;

		/// <summary>
		/// The constructor requires a name and a child node to be passed in.
		/// </summary>
		public BTDecorator(string name, BTNode node)
		{
			this.name = name;
			this.node = node;
			Assert.IsTrue(node != null, "BTDecorator (" + name + ") must have a child node!");
		}

		internal override void ResetTreeNode()
		{
			nodeState = NodeStates.RUNNING;
			node.ResetTreeNode();
		}
	}
}

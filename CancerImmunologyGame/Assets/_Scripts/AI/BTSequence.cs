using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions;
using UnityEngine;
namespace BehaviourTreeBase
{
	public class BTSequence : BTComposite
	{

		/// <summary>
		/// The constructor requires a list of child nodes to be passed in.
		/// </summary>
		public BTSequence(string name, List<BTNode> nodes) : base (name, nodes) { }

		/// <summary>
		/// If any of the children reports a failure, the sequence will
		/// immediately report a failure upwards. If all children succeed,
		/// it will report a success instead.
		/// </summary>
		/// <returns></returns>
		internal override NodeState Evaluate()
		{
			foreach (BTNode node in nodes)
			{
				switch (node.Evaluate())
				{
					case NodeState.FAILURE:
						nodeState = NodeState.FAILURE;
						Debug.Log(name + "\t has been evaluated to " + nodeState);
						return nodeState;
					case NodeState.SUCCESS:
						continue;
					case NodeState.RUNNING:
						nodeState = NodeState.RUNNING;
						Debug.Log(name + "\t has been evaluated to " + nodeState);
						return nodeState;
					default:
						nodeState = NodeState.SUCCESS;
						Debug.Log(name + "\t has been evaluated to " + nodeState);

						return nodeState;
				}
			}
			nodeState = NodeState.SUCCESS;
			Debug.Log(name + "\t has been evaluated to " + nodeState);
			return nodeState;
		}
	}
}

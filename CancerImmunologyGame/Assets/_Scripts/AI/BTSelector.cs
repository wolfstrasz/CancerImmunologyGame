using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions;
using UnityEngine;
namespace BehaviourTreeBase
{
	public class BTSelector : BTComposite
	{

		/// <summary>
		/// The constructor requires a list of child nodes to be passed in.
		/// </summary>
		public BTSelector(string name, List<BTNode> nodes) : base(name, nodes) { }


		/// <summary>
		/// If any of the children reports a success, the selector will
		/// immediately report a success upwards.If all children fail,
		/// it will report a failure instead.
		/// </summary>
		/// <returns></returns>
		internal override NodeStates Evaluate()
		{
			foreach (BTNode node in nodes)
			{
				switch (node.Evaluate())
				{
					case NodeStates.FAILURE:
						continue;
					case NodeStates.SUCCESS:
						nodeState = NodeStates.SUCCESS;
						//Debug.Log(name + "\t has been evaluated to " + nodeState);
						return nodeState;
					case NodeStates.RUNNING:
						nodeState = NodeStates.RUNNING;
						//Debug.Log(name + "\t has been evaluated to " + nodeState);
						return nodeState;
					default:
						continue;
				}
			}
			nodeState = NodeStates.FAILURE;
			//Debug.Log(name + "\t has been evaluated to " + nodeState);
			return nodeState;
		}
	}

}
 
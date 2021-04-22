using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace BehaviourTreeBase
{
	public class BTRepeater : BTDecorator
	{
		private int timesExecuted = 0;
		private int timesToRepeat = 1;
		private NodeStates acceptedState = NodeStates.RUNNING;

		public BTRepeater (string name, BTNode node, int timesToRepeat, NodeStates acceptedState) : base (name, node)
		{
			//this.timesToRepeat = timesToRepeat;
			//this.acceptedState = acceptedState;

			Assert.IsTrue(node != null, "BTRepeater (" + name + ") must have a child node!");
			Assert.IsTrue(acceptedState != NodeStates.RUNNING, "BTRepeater (" + name + ") must evaluate until a SUCCESS or FAILURE state. Currently set to RUNNING state.");
		}

		internal override NodeStates Evaluate()
		{
			if (timesExecuted < timesToRepeat)
			{
				NodeStates childState = node.Evaluate();

				if (childState == NodeStates.RUNNING)
				{
					nodeState = NodeStates.RUNNING;
					return nodeState;
				} else if (childState == acceptedState)
				{
					nodeState = NodeStates.SUCCESS;
					return nodeState;
				} else // if it is not the accepted state
				{
					timesExecuted++;
					nodeState = NodeStates.RUNNING;
					return NodeState;
				}
			}
			return NodeStates.RUNNING;
		}
	}
}
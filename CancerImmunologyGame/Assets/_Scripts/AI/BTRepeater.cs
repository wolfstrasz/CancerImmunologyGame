using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace BehaviourTreeBase
{
	public class BTRepeater : BTDecorator
	{
		private int timesExecuted = 0;
		private int timesToRepeat = 1;
		private NodeState acceptedState = NodeState.RUNNING;

		public BTRepeater (string name, BTNode node, int timesToRepeat, NodeState acceptedState) : base (name, node)
		{
			//this.timesToRepeat = timesToRepeat;
			//this.acceptedState = acceptedState;

			Assert.IsTrue(node != null, "BTRepeater (" + name + ") must have a child node!");
			Assert.IsTrue(acceptedState != NodeState.RUNNING, "BTRepeater (" + name + ") must evaluate until a SUCCESS or FAILURE state. Currently set to RUNNING state.");
		}

		internal override NodeState Evaluate()
		{
			if (timesExecuted < timesToRepeat)
			{
				NodeState childState = node.Evaluate();

				if (childState == NodeState.RUNNING)
				{
					nodeState = NodeState.RUNNING;
					return nodeState;
				} else if (childState == acceptedState)
				{
					nodeState = NodeState.SUCCESS;
					return nodeState;
				} else // if it is not the accepted state
				{
					timesExecuted++;
					nodeState = NodeState.RUNNING;
					return NodeState;
				}
			}
			return NodeState.RUNNING;
		}
	}
}
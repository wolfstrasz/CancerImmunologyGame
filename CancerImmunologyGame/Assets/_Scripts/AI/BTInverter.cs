﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace BehaviourTreeBase
{
	public class BTInverter : BTDecorator
	{

		public BTInverter(string name, BTNode node) : base (name, node)
		{
			Assert.IsTrue(node != null, "BTInverter (" + name + ") must have a child node!");
		}

		internal override NodeState Evaluate()
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
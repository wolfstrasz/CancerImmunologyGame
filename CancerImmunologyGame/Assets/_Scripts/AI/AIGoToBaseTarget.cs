using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTreeBase
{
    public class AIGoToBaseTarget : BTActionNode
    {

		private IAICancerInteractor interactor;

		public AIGoToBaseTarget(string name, BehaviourTree owner, IAICancerInteractor interactor) : base(name, owner, "AIGoToBaseTarget")
		{
			this.interactor = interactor;
		}


		protected override NodeStates OnEvaluateAction()
		{

			GameObject objectFound = interactor.BasePoint;
			if (objectFound == null)
			{
				nodeState = NodeStates.FAILURE;
				return nodeState;
			}
			else
			{
				interactor.Target = objectFound;
				interactor.AcceptableDistanceFromTarget = 1f;
				nodeState = NodeStates.SUCCESS;
				return nodeState;
			}
		}

		protected override void OnResetTreeNode()
		{
		}

    }
}

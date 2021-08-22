using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTreeBase;

namespace ImmunotherapyGame.AI
{
	public class AISelectABase : BTActionNode
    {

		private AIHomeData homeData;
		private AITargetingData targetData;
		public AISelectABase(string name, BehaviourTree owner, AIHomeData homeData, AITargetingData targetData) : base(name, owner, "AISelectABase")
		{
			this.homeData = homeData;
			this.targetData = targetData;
		}


		protected override NodeStates OnEvaluateAction()
		{
			GameObject objectFound = homeData.home;
			if (objectFound == null)
			{
				Debug.LogWarning("AI is missing home object from home data");
				nodeState = NodeStates.FAILURE;
				return nodeState;
			}
			else
			{
				targetData.currentTarget = objectFound;
				targetData.acceptableDistanceFromCurrentTarget = homeData.acceptableDistance;
				nodeState = NodeStates.SUCCESS;
				return nodeState;
			}
		}

		protected override void OnResetTreeNode()
		{
		}

    }
}

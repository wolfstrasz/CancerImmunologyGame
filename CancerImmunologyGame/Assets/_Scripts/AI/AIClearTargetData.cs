using UnityEngine;
using BehaviourTreeBase;

namespace ImmunotherapyGame.AI
{
	public class AIClearTargetData : BTActionNode
	{
		private AITargetingData targetData;

		public AIClearTargetData(string name, BehaviourTree owner, AITargetingData targetData) : base(name, owner, "AIClearTargetData")
		{
			this.targetData = targetData;
		}

		protected override NodeStates OnEvaluateAction()
		{
			// Free target
			targetData.currentTarget = null;
			targetData.acceptableDistanceFromCurrentTarget = 0f;

			nodeState = NodeStates.SUCCESS;
			return nodeState;
		}

		protected override void OnResetTreeNode() { }

	}
}

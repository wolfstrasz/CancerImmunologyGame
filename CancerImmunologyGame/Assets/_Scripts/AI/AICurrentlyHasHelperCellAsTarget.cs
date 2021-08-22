using BehaviourTreeBase;

namespace ImmunotherapyGame.AI
{
	public class AICurrentlyHasHelperCellAsTarget : BTActionNode
	{
		public AITargetingData targetData;
		public AIHealerData healerData;

		public AICurrentlyHasHelperCellAsTarget(string name, BehaviourTree owner, AITargetingData targetData, AIHealerData healerData) : base(name, owner, "AICurrentlyHasHelperCellAsTarget")
		{
			this.targetData = targetData;
			this.healerData = healerData; 
		}

		protected override NodeStates OnEvaluateAction()
		{
			if (targetData.currentTarget == null || healerData.bookedHelperTCell == null)
			{
				nodeState = NodeStates.FAILURE;
				return nodeState;
			}

			nodeState = NodeStates.SUCCESS;
			return nodeState;
		}

		protected override void OnResetTreeNode()
		{
		}


	}
}
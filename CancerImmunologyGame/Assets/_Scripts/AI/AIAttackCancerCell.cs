using BehaviourTreeBase;


namespace ImmunotherapyGame.AI
{  
	public class AIAttackCancerCell : BTActionNode
	{
		private IAICancerCellInteractor interactor = null;

		public AIAttackCancerCell(string name, BehaviourTree owner, IAICancerCellInteractor interactor) : base(name, owner, "Attack Cancer Cell")
		{
			this.interactor = interactor;
		}


		protected override NodeStates OnEvaluateAction()
		{

			if (interactor.TargetedEvilCell != null)
			{
				interactor.ControlledCell.Attack(interactor.TargetedEvilCell.transform.position);

				nodeState = NodeStates.RUNNING;
				return nodeState;
			}
			else
			{
				nodeState = NodeStates.FAILURE;
				return nodeState;
			}
		}

		protected override void OnResetTreeNode()
		{
		}

	}
}
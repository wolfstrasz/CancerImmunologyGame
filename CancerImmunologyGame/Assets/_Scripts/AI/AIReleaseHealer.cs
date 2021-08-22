using UnityEngine;
using BehaviourTreeBase;

namespace ImmunotherapyGame.AI
{

	public class AIReleaseHealer : BTActionNode
	{
		private AIHealerData healerData;

		public AIReleaseHealer(string name, BehaviourTree owner, AIHealerData healerData) : base(name, owner, "AIReleaseHealer")
		{
			this.healerData = healerData;
		}

		protected override NodeStates OnEvaluateAction()
		{

			if (healerData.bookedHelperTCell == null && healerData.bookingSpot == null)
			{
				nodeState = NodeStates.SUCCESS;
				return nodeState;
			}

			Debug.Log("Releasing Helper" + healerData.bookedHelperTCell);
			// Free booked cell
			healerData.bookedHelperTCell.FreeBookingSpot(healerData.bookingSpot);
			healerData.bookedHelperTCell = null;
			healerData.bookingSpot = null;

			nodeState = NodeStates.SUCCESS;
			return nodeState;
		}

		protected override void OnResetTreeNode() { }
	}


	
}
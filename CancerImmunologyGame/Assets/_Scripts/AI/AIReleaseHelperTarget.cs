using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTreeBase;
using Cells;

public class AIReleaseHelperTarget : BTActionNode
{
	private IAIHelperCellInteractor handler;
	public AIReleaseHelperTarget (string name, BehaviourTree owner, IAIHelperCellInteractor handler) : base(name, owner, "RealeaseTarget")
	{
		this.handler = handler;
	}

	protected override NodeStates OnEvaluateAction()
	{
		Debug.Log("Releasing Helper" + handler.BookedHelperTCell);
		// Free booked cell
		handler.BookedHelperTCell.FreeBookingSpot(handler.BookingSpot);
		handler.BookedHelperTCell = null;

		// Free target
		handler.Target = null;
		handler.AcceptableDistanceFromTarget = 0f;

		nodeState = NodeStates.SUCCESS;
		return nodeState;
	}

	protected override void OnResetTreeNode()
	{
	}


}

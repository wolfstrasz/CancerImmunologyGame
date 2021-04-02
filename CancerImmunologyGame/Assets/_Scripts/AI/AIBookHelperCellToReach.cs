using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTreeBase;
using Cells;

public class AIBookHelperCellToReach : BTActionNode
{

	private IAIHelperCellInteractor interactor;

	public AIBookHelperCellToReach(string name, BehaviourTree owner, IAIHelperCellInteractor interactor) : base (name, owner, "BookHelperCell")
	{
		this.interactor = interactor;
	}

	protected override NodeStates OnEvaluateAction()
	{
		Transform cellTransform = interactor.ControlledCell.transform;
		List<HelperTCell> helperCells = new List<HelperTCell>(GlobalGameData.HelperTCells);
		helperCells.Sort(delegate (HelperTCell a, HelperTCell b) {
			return Vector3.SqrMagnitude(a.transform.position - cellTransform.position)
			.CompareTo(Vector3.SqrMagnitude(b.transform.position - cellTransform.position));
		});


		// If it has a currently booked HelperTcell and it is the closest one to the cell
		// Set it as a target to interact with
		if (helperCells[0] == interactor.BookedHelperTCell)
		{
			interactor.Target = interactor.BookingSpot;
			interactor.AcceptableDistanceFromTarget = 0.1f;
			nodeState = NodeStates.SUCCESS;
			return nodeState;
		}

		// else make sure you unbook from previous Cell
		if (interactor.BookedHelperTCell != null)
			interactor.BookedHelperTCell.FreeBookingSpot(interactor.BookingSpot);

		// Find closest helper cell with free spots
		GameObject bookedSpot = null;
		int helperId = 0;
		while (bookedSpot == null && helperId < helperCells.Count)
		{
			bookedSpot = helperCells[helperId].ReserveBookingSpot();
			helperId++;
		}

		// Check if any spot was found.
		if (bookedSpot == null)
		{
			nodeState = NodeStates.FAILURE;
			return nodeState;
		}

		// If spot was found set the correct data.
		interactor.BookedHelperTCell = helperCells[helperId - 1];
		interactor.BookingSpot = bookedSpot;
		interactor.Target = bookedSpot;
		interactor.AcceptableDistanceFromTarget = 0.2f;
		nodeState = NodeStates.SUCCESS;
		return nodeState;
	}


	protected override void OnResetTreeNode()
	{
	}

}
 
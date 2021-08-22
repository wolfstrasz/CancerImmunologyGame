using System.Collections.Generic;
using UnityEngine;
using BehaviourTreeBase;


namespace ImmunotherapyGame.AI
{
	public class AIBookHealer : BTActionNode
	{
		AIControlledCellData controlledCellData;
		AIHealerData healerData;
		AITargetingData targetData;

		public AIBookHealer(string name, BehaviourTree owner, AIControlledCellData controlledCellData, AIHealerData healerData, AITargetingData targetData) : base(name, owner, "AIBookHealer")
		{
			this.controlledCellData = controlledCellData;
			this.healerData = healerData;
			this.targetData = targetData;
		}

		protected override NodeStates OnEvaluateAction()
		{
			if (controlledCellData.controlledCell == null)
			{
				Debug.Log("AI Controller does not controll a cell");
				nodeState = NodeStates.FAILURE;
				return nodeState;
			}

			Vector3 controlledCellPosition = controlledCellData.controlledCell.transform.position;

			// Sort the list of all helper T Cells by squared distance
			// Using sqdist for optimisation purposes
			List<HelperTCell> helperCells = new List<HelperTCell>(GlobalLevelData.HelperTCells);
			helperCells.Sort(delegate (HelperTCell a, HelperTCell b)
			{
				return Vector3.SqrMagnitude(a.transform.position - controlledCellPosition)
				.CompareTo(Vector3.SqrMagnitude(b.transform.position - controlledCellPosition));
			});

			// NOTE: There is a chance that another patroling Helper T Cell becomes closer and 
			// the Killer cell will need to change to it and reach it.

			// If it has a currently booked HelperTcell and it is the closest one to the cell
			// Set it as a target to interact with
			if (helperCells[0] == healerData.bookedHelperTCell)
			{
				targetData.currentTarget = healerData.bookingSpot;
				targetData.acceptableDistanceFromCurrentTarget = healerData.acceptableDistance;
				nodeState = NodeStates.SUCCESS;
				return nodeState;
			}

			//  Make sure you unbook from previous Cell
			if (healerData.bookedHelperTCell != null)
			{
				healerData.bookedHelperTCell.FreeBookingSpot(healerData.bookingSpot);
			}

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
			healerData.bookedHelperTCell = helperCells[helperId - 1];
			healerData.bookingSpot = bookedSpot;
			targetData.currentTarget = bookedSpot;
			targetData.acceptableDistanceFromCurrentTarget = healerData.acceptableDistance;
			nodeState = NodeStates.SUCCESS;
			return nodeState;
		}


		protected override void OnResetTreeNode()
		{
		}

	}
}
 
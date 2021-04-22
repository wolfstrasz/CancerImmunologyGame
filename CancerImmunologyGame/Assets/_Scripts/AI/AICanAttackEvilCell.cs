using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTreeBase;

namespace ImmunotherapyGame.AI
{
	public class AICanAttackEvilCell : BTActionNode
	{
		private IAICancerCellInteractor interactor;

		private KillerCell controlCell = null;

		private float range = 1f;
		private float rangeReducer = 0.8f;

		public AICanAttackEvilCell(string name, BehaviourTree owner, IAICancerCellInteractor interactor) : base(name, owner, "CanAttackCancerCell")
		{
			this.interactor = interactor;
			controlCell = interactor.ControlledCell;
			range = controlCell.Range * rangeReducer;
		}


		protected override NodeStates OnEvaluateAction()
		{
			if (interactor.TargetedEvilCell == null || interactor.Target == null)
			{
				nodeState = NodeStates.FAILURE;
				return nodeState;
			}

			if (Vector3.SqrMagnitude(controlCell.transform.position - interactor.TargetedEvilCell.transform.position) < range * range)
			{
				nodeState = NodeStates.SUCCESS;
				return nodeState;
			}

			nodeState = NodeStates.FAILURE;
			return nodeState;
		}

		protected override void OnResetTreeNode()
		{
			controlCell = interactor.ControlledCell;
			range = controlCell.Range * rangeReducer;
		}

	}
}
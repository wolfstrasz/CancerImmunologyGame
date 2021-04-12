﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTreeBase;
using Cells;
using Cells.Cancers;

public class AIFindEvilCellTarget : BTActionNode
{

	private IAICancerInteractor interactor;

	public AIFindEvilCellTarget(string name, BehaviourTree owner, IAICancerInteractor interactor) : base(name, owner, "AIFindClosestTarget")
	{
		this.interactor = interactor;
	}


	protected override NodeStates OnEvaluateAction()
	{

		//if (controller.TargetedCancerCell != null)
		//{
		//	nodeState = NodeStates.SUCCESS;
		//	return nodeState;
		//}

		Debug.Log("Evaluating AIFindEVILCELLS0");

		List<Cancer> possibleCancerTargets = new List<Cancer>();

		for (int i = 0; i < interactor.TargetCancers.Count; ++i)
		{
			if (interactor.TargetCancers[i].IsAlive)
				possibleCancerTargets.Add(interactor.TargetCancers[i]);
		}

		if (possibleCancerTargets.Count <= 0)
		{
			interactor.Target = null;
			interactor.TargetedEvilCell = null;
			nodeState = NodeStates.FAILURE;
			return nodeState;
		}

		Debug.Log("Evaluating AIFindEVILCELLS1");

		Cancer selectedCancer = null;
		if (possibleCancerTargets.Count == 1)
		{
			selectedCancer = possibleCancerTargets[0];
		}
		else
		{
			selectedCancer = possibleCancerTargets[Random.Range(0, possibleCancerTargets.Count)];
		}

		Debug.Log("Evaluating AIFindEVILCELLS2");

		if (selectedCancer.AllCells.Count == 0)
		{
			interactor.Target = null;
			interactor.TargetedEvilCell = null;
			nodeState = NodeStates.FAILURE;
			return nodeState;
		}

		// Find closest cell from these
		Vector3 position = interactor.ControlledCell.transform.position;
		int index = 0;
		//while (index < selectedCancer.AllCells.Count && selectedCancer.AllCells[index].isImmune)
		//	index++;

		//Debug.Log("Evaluating AIFindEVILCELLS2-2");

		//if (index >= selectedCancer.AllCells.Count)
		//{
		//	interactor.Target = null;
		//	interactor.TargetedEvilCell = null;
		//	nodeState = NodeStates.FAILURE;
		//	return nodeState;
		//}

		//Debug.Log("Evaluating AIFindEVILCELLS2-3");

		float closestDistance = Vector3.SqrMagnitude(selectedCancer.AllCells[index].transform.position - position);
		EvilCell closestObject = selectedCancer.AllCells[index];

		Debug.Log("Evaluating AIFindEVILCELLS2-4");

		foreach (EvilCell foundObject in selectedCancer.AllCells)
		{
			if (!foundObject.isImmune)
			{
				float distance = Vector3.SqrMagnitude(foundObject.transform.position - position);
				if (distance < closestDistance)
				{
					closestDistance = distance;
					closestObject = foundObject;
				}
			}
		}

		Debug.Log("Evaluating AIFindEVILCELLS3");

		GameObject objectFound = closestObject.gameObject;

		Debug.Log("Evaluating AIFindEVILCELLS4");

		if (objectFound == null)
		{
			nodeState = NodeStates.FAILURE;
			interactor.Target = null;
			interactor.TargetedEvilCell = null;

			return nodeState;
		}
		else
		{
			interactor.Target = objectFound;
			interactor.AcceptableDistanceFromTarget = interactor.ControlledCell.Range;
			interactor.TargetedEvilCell = objectFound.GetComponent<EvilCell>();
			nodeState = NodeStates.SUCCESS;
			return nodeState;
		}
	}

	protected override void OnResetTreeNode()
	{

	}
}

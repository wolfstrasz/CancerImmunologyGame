using System.Collections.Generic;
using UnityEngine;
using BehaviourTreeBase;
using ImmunotherapyGame.Cancers;

namespace ImmunotherapyGame.AI
{
	public class AIFindEvilCellTarget : BTActionNode
	{

		private IAICancerInteractor interactor;

		public AIFindEvilCellTarget(string name, BehaviourTree owner, IAICancerInteractor interactor) : base(name, owner, "AIFindClosestTarget")
		{
			this.interactor = interactor;
		}


		protected override NodeStates OnEvaluateAction()
		{

			//if (interactor.Target != null && interactor.Target.GetComponent<Cell>() != null)
			//{
			//	nodeState = NodeStates.SUCCESS;
			//	return nodeState;
			//}

			//Debug.Log("Evaluating AIFindEVILCELLS0");

			//List<Cancer> possibleCancerTargets = new List<Cancer>();


			//for (int i = 0; i < interactor.TargetCancers.Count; ++i)
			//{
			//	if (interactor.TargetCancers[i].IsAlive)
			//		possibleCancerTargets.Add(interactor.TargetCancers[i]);
			//}

			//if (possibleCancerTargets.Count <= 0)
			//{
			//	interactor.Target = null;
			//	interactor.TargetedEvilCell = null;
			//	nodeState = NodeStates.FAILURE;
			//	return nodeState;
			//}

			//Debug.Log("Evaluating AIFindEVILCELLS1");

			//Cancer selectedCancer = null;
			//if (possibleCancerTargets.Count == 1)
			//{
			//	selectedCancer = possibleCancerTargets[0];
			//}
			//else
			//{

			//	// Find one with the highest cells


			//	int maxCount = 0;
			//	foreach (Cancer cancer in possibleCancerTargets)
			//	{
			//		if (cancer.AllCells.Count > maxCount)
			//		{
			//			maxCount = cancer.AllCells.Count;
			//			selectedCancer = cancer;
			//		}
			//	}
			//}

			//Debug.Log("Evaluating AIFindEVILCELLS2");

			//if (selectedCancer.AllCells.Count == 0)
			//{
			//	interactor.Target = null;
			//	interactor.TargetedEvilCell = null;
			//	nodeState = NodeStates.FAILURE;
			//	return nodeState;
			//}

			//// Find closest cell from these
			//Vector3 position = interactor.ControlledCell.transform.position;
			//int index = 0;
			////while (index < selectedCancer.AllCells.Count && selectedCancer.AllCells[index].isImmune)
			////	index++;

			////Debug.Log("Evaluating AIFindEVILCELLS2-2");

			////if (index >= selectedCancer.AllCells.Count)
			////{
			////	interactor.Target = null;
			////	interactor.TargetedEvilCell = null;
			////	nodeState = NodeStates.FAILURE;
			////	return nodeState;
			////}

			////Debug.Log("Evaluating AIFindEVILCELLS2-3");

			//float closestDistance = Vector3.SqrMagnitude(selectedCancer.AllCells[index].transform.position - position);
			//Cell closestObject = selectedCancer.AllCells[index];

			//Debug.Log("Evaluating AIFindEVILCELLS2-4");

			//foreach (EvilCell foundObject in selectedCancer.AllCells)
			//{
			//	if (!foundObject.isImmune)
			//	{
			//		float distance = Vector3.SqrMagnitude(foundObject.transform.position - position);
			//		if (distance < closestDistance)
			//		{
			//			closestDistance = distance;
			//			closestObject = foundObject;
			//		}
			//	}
			//}

			//Debug.Log("Evaluating AIFindEVILCELLS3");

			//GameObject objectFound = closestObject.gameObject;

			//Debug.Log("Evaluating AIFindEVILCELLS4");

			//if (objectFound == null)
			//{
			//	nodeState = NodeStates.FAILURE;
			//	interactor.Target = null;
			//	interactor.TargetedEvilCell = null;

			//	return nodeState;
			//}
			//else
			//{
			//	interactor.Target = objectFound;
			//	interactor.AcceptableDistanceFromTarget = interactor.ControlledCell.Range * 0.7f;
			//	interactor.TargetedEvilCell = objectFound.GetComponent<EvilCell>();
			//	nodeState = NodeStates.SUCCESS;
			//	return nodeState;
			//}
			nodeState = NodeStates.FAILURE;
			return nodeState;
		}

		protected override void OnResetTreeNode()
		{

		}
	}
}
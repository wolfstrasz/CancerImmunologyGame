using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTreeBase;

namespace ImmunotherapyGame.AI
{
	public class AIFindClosestTargetOfType<TypeOfTarget> : BTActionNode where TypeOfTarget : MonoBehaviour
	{

		private IAITargetHandler controller;
		private bool random = false;

		public AIFindClosestTargetOfType(string name, BehaviourTree owner, IAITargetHandler controller, bool random = false) : base(name, owner, "AIFindClosestTarget")
		{
			this.controller = controller;
			this.random = random;
		}

		protected override NodeStates OnEvaluateAction()
		{

			GameObject objectFound = null;
			if (random)
			{
				TypeOfTarget[] allItems = GameObject.FindObjectsOfType<TypeOfTarget>();

				if (allItems.Length > 1)
				{
					int randomValue = Random.Range(0, 100);
					objectFound = allItems[randomValue % allItems.Length].gameObject;
				}
				else if (allItems.Length == 1)
				{
					objectFound = allItems[0].gameObject;
				}
			}
			else
			{
				objectFound = Utils.FindClosestGameObjectOfType<TypeOfTarget>(controller.ControlledCell.transform.position);
			}

			// Check object found
			if (objectFound == null)
			{
				nodeState = NodeStates.FAILURE;
				return nodeState;
			}
			else
			{
				controller.Target = objectFound;
				controller.AcceptableDistanceFromTarget = 1f;
				nodeState = NodeStates.SUCCESS;
				return nodeState;
			}
		}

		protected override void OnResetTreeNode()
		{

		}
	}
}
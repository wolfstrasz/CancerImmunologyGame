using BehaviourTreeBase;
using ImmunotherapyGame.Cancers;
using UnityEngine;

namespace ImmunotherapyGame.AI
{
	public class AIGetCancerToAttack : BTActionNode
	{
		private AICombatData combatData;
		private AIControlledCellData controlledCellData;

		public AIGetCancerToAttack(string name, BehaviourTree owner, AIControlledCellData controlledCellData, AICombatData combatData ) : base(name, owner, "AIGetCancerToAttack")
		{
			this.combatData = combatData;
			this.controlledCellData = controlledCellData;
		}


		protected override NodeStates OnEvaluateAction()
		{
			if (combatData.focusType == AICombatData.TargetFocus.CLOSEST)
			{
				// Find closest cancer to fight
				float minDist = 1000000f;
				Cancer cancerTarget = null;
				GameObject controlledObject = controlledCellData.controlledObject;
				Vector3 controlledObjectPosition = controlledObject.transform.position;

				for (int i = 0; i < combatData.cancersToFight.Count; ++i)
				{
					if (combatData.cancersToFight[i].IsAlive)
					{
						Cancer cancer = combatData.cancersToFight[i];
						float distance = Vector3.SqrMagnitude(cancer.transform.position - controlledObjectPosition);
						if (distance < minDist)
						{
							cancerTarget = cancer;
							minDist = distance;
						}
					}
				}

				// Fail if no alive cancers found
				if (cancerTarget == null)
				{
					nodeState = NodeStates.FAILURE;
					return nodeState;
				}

				// Otherwise set is as a target to reach
				combatData.currentCancerTarget = cancerTarget;

				nodeState = NodeStates.SUCCESS;
				return nodeState;
			}
			else if (combatData.focusType == AICombatData.TargetFocus.MOST_SEVERE)
			{
				// Find closest cancer to fight
				float minDist = 1000000f;
				Cancer cancerTarget = null;
				GameObject controlledObject = controlledCellData.controlledObject;
				Vector3 controlledObjectPosition = controlledObject.transform.position;

				for (int i = 0; i < combatData.cancersToFight.Count; ++i)
				{
					if (combatData.cancersToFight[i].IsAlive)
					{
						Cancer cancer = combatData.cancersToFight[i];
						float distance = Vector3.SqrMagnitude(cancer.transform.position - controlledObjectPosition);
						if (distance < minDist)
						{
							cancerTarget = cancer;
							minDist = distance;
						}
					}
				}

				// Fail if no alive cancers found
				if (cancerTarget == null)
				{
					nodeState = NodeStates.FAILURE;
					return nodeState;
				}

				// Otherwise set is as a target to reach
				combatData.currentCancerTarget = cancerTarget;

				nodeState = NodeStates.SUCCESS;
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

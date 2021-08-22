using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using BehaviourTreeBase;
using ImmunotherapyGame.Player;

namespace ImmunotherapyGame.AI
{
	[RequireComponent(typeof(Seeker))]
	public class AIController : MonoBehaviour
	{
		[SerializeField] private BehaviourTree tree = null;
		[SerializeField] private KillerCell controlledCell = null;
		[SerializeField] private AIHomeData homeData;
		[SerializeField] private AITargetingData targetData;
		[SerializeField] private AIPathfindingData pathfindingData;
		[SerializeField] private AIControlledCellData controlledCellData;
		[SerializeField] private AIHealerData healerData;
		[SerializeField] private AICombatData combatData;
		[SerializeField] private float distanceFromBaseToDissapear = 4.0f;

		public void Start()
		{
			InitialiseBehaviourTree();
		}

		public void OnEnable()
		{
			controlledCellData.controlledCell = controlledCell;
			controlledCell.onDeathEvent += OnCellDeath;
			GlobalLevelData.AIControllers.Add(this);
		}

		private void OnCellDeath(Cell cell)
		{
			if (GlobalLevelData.RespawnAreas == null || GlobalLevelData.RespawnAreas.Count == 0)
			{
				Debug.LogWarning("Zero respawn areas found on map. Respawning at same position");
				return;
			}

			// Find closest spawn location
			List<PlayerRespawnArea> respawnLocations = GlobalLevelData.RespawnAreas;
			Vector3 closestRespawnLocation = respawnLocations[0].Position;
			float minDistance = Vector3.Distance(transform.position, respawnLocations[0].Position);

			foreach (var area in respawnLocations)
			{
				float distance = Vector3.Distance(transform.position, area.Position);
				if (distance <= minDistance)
				{
					minDistance = distance;
					closestRespawnLocation = area.Position;
				}
			}

			// Transport cell and heal TODO: Move to cell doing it.
			controlledCell.transform.position = closestRespawnLocation;
			transform.position = closestRespawnLocation;
			controlledCell.Respawn();
		}

		public void OnDisable()
		{
			controlledCellData.controlledCell = null;
			controlledCell.onDeathEvent -= OnCellDeath;
			GlobalLevelData.AIControllers.Remove(this);
		}

		private void InitialiseBehaviourTree()
		{
			tree = new BehaviourTree(); // Garbage collection will clean it if something requires reinitialisation

			BTSelector root = new BTSelector("Root", 4);
			{

				BTSequence checkIfInNeedOfHealing = new BTSequence("Check if in need of healing", 3);
				{
					BTActionNode alreadyHasAHealer = new AICurrentlyHasHelperCellAsTarget("Already has a Healer", tree, targetData, healerData);
					BTInverter doesNotHaveAHealer = new BTInverter("Does not have a Healer", alreadyHasAHealer);

					BTSelector checkIfInCriticalCondition = new BTSelector("Check if in critical condition", 2);
					{
						BTActionNode hasLowHealth = new AIHealthConditional("Has low health", tree, controlledCellData, ValueConditionalOperator.LESS_THAN_EQUAL, 0.25f);
						BTActionNode hasLowEnergy = new AIEnergyConditional("Has low energy", tree, controlledCellData, ValueConditionalOperator.LESS_THAN_EQUAL, 0.30f);
						checkIfInCriticalCondition.AddNode(hasLowHealth);
						checkIfInCriticalCondition.AddNode(hasLowEnergy);
					}

					BTActionNode bookHealer = new AIBookHealer("Book a Healer", tree, controlledCellData, healerData, targetData);

					checkIfInNeedOfHealing.AddNode(doesNotHaveAHealer);
					checkIfInNeedOfHealing.AddNode(checkIfInCriticalCondition);
					checkIfInNeedOfHealing.AddNode(bookHealer);
				}

				BTSequence healingState = new BTSequence("Healing State", 3);
				{
					BTActionNode alreadyHasAHealer = new AICurrentlyHasHelperCellAsTarget("Already has a Healer", tree, targetData, healerData);
					BTActionNode reachTheHealer = new AIReachDestination("Reach the healer", tree, controlledCellData, pathfindingData, targetData);
					BTSelector nearTheHealer = new BTSelector("Near the healer", 2);
					{
						BTSequence checkIfReadyToForFighting = new BTSequence("Check if ready for fighting", 2);
						{
							BTSequence checkForStableCondition = new BTSequence("Is In Stable Condition", 2);
							{
								BTActionNode hasEnoughHealth = new AIHealthConditional("Has Enough Health", tree, controlledCellData, ValueConditionalOperator.MORE_THAN, 0.80f);
								BTActionNode hasEnoughEnergy = new AIEnergyConditional("Has Enough Energy", tree, controlledCellData, ValueConditionalOperator.MORE_THAN, 0.85f);
								checkForStableCondition.AddNode(hasEnoughHealth);
								checkForStableCondition.AddNode(hasEnoughEnergy);
							}

							BTSequence prepareForFighting = new BTSequence("Prepare for fighting", 2);
							{
								BTActionNode releaseHealer = new AIReleaseHealer("Release Healer", tree, healerData);
								BTActionNode clearTargetData = new AIClearTargetData("Clear Target Data", tree, targetData);
								prepareForFighting.AddNode(releaseHealer);
								prepareForFighting.AddNode(clearTargetData);
							}

							checkIfReadyToForFighting.AddNode(checkForStableCondition);
							checkIfReadyToForFighting.AddNode(prepareForFighting);
						}

						BTActionNode waitToHeal = new AIWait("WaitToHeal", tree, 100f, false);

						nearTheHealer.AddNode(checkIfReadyToForFighting);
						nearTheHealer.AddNode(waitToHeal);
					}

					healingState.AddNode(alreadyHasAHealer);
					healingState.AddNode(reachTheHealer);
					healingState.AddNode(nearTheHealer);
				}


				BTSequence fightingState = new BTSequence("Fighting State", 4);
				{
					BTActionNode getACancerTarget = new AIGetCancerToAttack("Get a Cancer to attack", tree, controlledCellData, combatData);
					BTActionNode clearTargetData = new AIClearTargetData("Clear target data", tree, targetData);
					BTActionNode getCellTarget = new AIFindACellTarget("Get Cell target", tree, controlledCellData, combatData, targetData);

					BTSelector checkIfInRange = new BTSelector("Check if in range", 3);
					{
						BTActionNode useSecondaryAttack = new AIUseKillerCellSecondaryAbility("Use secondary Ability", tree, controlledCellData);
						BTActionNode usePrimaryAttack = new AIUseKillerCellPrimaryAbility("Use secondary Ability", tree, controlledCellData);
						BTActionNode reachCellTarget = new AIReachDestination("Reach Cell Target", tree, controlledCellData, pathfindingData, targetData);

						checkIfInRange.AddNode(useSecondaryAttack);
						checkIfInRange.AddNode(usePrimaryAttack);
						checkIfInRange.AddNode(reachCellTarget);
					}
									
					fightingState.AddNode(getACancerTarget);
					fightingState.AddNode(clearTargetData);
					fightingState.AddNode(getCellTarget);
					fightingState.AddNode(checkIfInRange);
				}

				// Should be last
				BTSequence goToBaseState = new BTSequence("Going to base state", 2);
				{
					BTActionNode targetTheBase = new AISelectABase("Target the base", tree, homeData, targetData);
					BTActionNode reachTheBase = new AIReachDestination("Reach the base", tree, controlledCellData, pathfindingData, targetData);

					goToBaseState.AddNode(targetTheBase);
					goToBaseState.AddNode(reachTheBase);
				}

				root.AddNode(checkIfInNeedOfHealing);
				root.AddNode(healingState);
				root.AddNode(fightingState);
				root.AddNode(goToBaseState);
			}

			tree.rootNode = root;
		}

		public void OnUpdate()
		{
			// Update position 
			transform.position = controlledCell.transform.position;

			// Reset the data
			controlledCellData.movementDirection = Vector3.zero;
			controlledCellData.speed = controlledCell.CurrentSpeed;

			// Evaluate and apply movement
			tree.Evaluate();
			controlledCell.MovementVector = controlledCellData.movementDirection;

			// Hide if reached base
			if (Vector3.SqrMagnitude(controlledCell.transform.position - homeData.home.transform.position) < distanceFromBaseToDissapear)
			{
				controlledCell.gameObject.SetActive(false);
				gameObject.SetActive(false);
				return;
			}
		}
	}

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTreeBase;
using Pathfinding;

public class KillerCellAI : BehaviourTree
{
	[Header ("Killer Cell AI")]
	public KillerCell cell;
	public Transform healTarget = null;
	public Transform baseTarget = null;
	public AIDestinationSetter setter = null;
	public AIPath path = null;

	void Start()
	{

		// ACTIONS
		BTActionNode hasToHeal = new AINeedToHeal("HasToHeal", this, cell);
		BTActionNode goToHeal = new AIReachDestination("Reach Healer", this, setter, path, healTarget);
		BTActionNode waitToHeal = new AIWait("HealWait", this, 4f, false);
		BTActionNode goToBase = new AIReachDestination("Reach Base", this, setter, path, baseTarget);
		BTActionNode imitateFighting = new AIWait("Imitate", this, 10f, true);

		// GO AND HEAL
		List<BTNode> shouldHealList = new List<BTNode>();
		shouldHealList.Add(hasToHeal);
		shouldHealList.Add(goToHeal);
		shouldHealList.Add(waitToHeal);
		BTSequence shouldHeal = new BTSequence("HealSequence", shouldHealList );

		// GO TO BASE
		List<BTNode> shouldGoToBaseList = new List<BTNode>();
		shouldGoToBaseList.Add(goToBase);
		shouldGoToBaseList.Add(imitateFighting);
		BTSequence shouldGoToBase = new BTSequence("GoToBase", shouldGoToBaseList);

		// ROOT 
		List<BTNode> rootSelectorList = new List<BTNode>();
		rootSelectorList.Add(shouldHeal);
		rootSelectorList.Add(shouldGoToBase);
		rootNode = new BTSelector("rootNode", rootSelectorList);
	}

	protected override void ResetTree()
	{
		waitState = NodeState.RUNNING;
		currentProcessingNode = null;
		rootNode.ResetTreeNode();
	}



	// WIATING
	// -----------------------------------------------------
	public NodeState waitState = NodeState.RUNNING;
	public IEnumerator waitRoutine = null;

	public NodeState WaitForSecondsAI()
	{
		if (waitState != NodeState.RUNNING)
			return waitState;

		if (waitRoutine == null)
		{
			waitRoutine = WaitFor3Seconds();
			StartCoroutine(waitRoutine);
		}

		return waitState;
	}

	IEnumerator WaitFor3Seconds()
	{
		yield return new WaitForSecondsRealtime(3.0f);
		waitState = NodeState.SUCCESS;
		waitRoutine = null;
	}

	// HEALING CHECK
	// -----------------------------------------------------

	public NodeState HasToHeal()
	{
		if (cell.Health + cell.Health < KillerCell.maxHealth)
		{
			return NodeState.SUCCESS;
		}

		return NodeState.FAILURE;
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using BehaviourTreeBase;
using Cells;

[RequireComponent(typeof(Seeker))]
public class AIController : MonoBehaviour, IAIMovementController
{
	[Header("AI Data")]
	[SerializeField]
	private BehaviourTree tree = null;
	[SerializeField]
	private KillerCell controlledCell = null;
	[SerializeField]
	private bool initialised = false;

	[Header("MovementControllInterface (Read Only)")]
	[SerializeField]
	private Seeker seeker = null;
	[SerializeField]
	private GameObject target = null;
	[SerializeField]
	private float acceptableDistanceFromEndOfPath = 1.0f;
	[SerializeField]
	private Vector2 movementVector = Vector2.zero;
	private Vector2 zeroVector = Vector2.zero;
	


	public void Start()
	{
		seeker = GetComponent<Seeker>();
		InitialiseBehaviourTree();

		initialised = true;
	}

	private void InitialiseBehaviourTree()
	{
		tree = new BehaviourTree();
		// ACTIONS
		BTActionNode hasToHeal = new AINeedToHealKillerCell("HasToHeal", tree, this);
		BTActionNode findClosestHelper = new AIFindClosestTargetOfType<HelperTCell>("FindHelperCell", tree, this);
		BTActionNode goToHeal = new AIReachDestination("Reach Healer", tree, this);

		// GO AND HEAL
		List<BTNode> shouldHealList = new List<BTNode>();
		shouldHealList.Add(hasToHeal);
		shouldHealList.Add(findClosestHelper);
		shouldHealList.Add(goToHeal);
		BTSequence shouldHeal = new BTSequence("HealSequence", shouldHealList);

		// Set ROOT 
		List<BTNode> rootSelectorList = new List<BTNode>();
		rootSelectorList.Add(shouldHeal);
		tree.rootNode = new BTSelector("rootNode", rootSelectorList);
	}

	void Update()
	{
		if (!initialised) return;
		movementVector = zeroVector;
		tree.Evaluate();
	}


	void FixedUpdate()
	{
		controlledCell.MovementVector = movementVector;
	}


	// IAI Movement Controller
	public Seeker GetSeeker()
	{
		return seeker;
	}

	public void UpdateMovementDirection(Vector3 directionVector)
	{
		movementVector.x = directionVector.x;
		movementVector.y = directionVector.y;
	}

	public float GetAcceptableDistanceFromTarget()
	{
		return acceptableDistanceFromEndOfPath;
	}

	// IAIKillerCellController
	public KillerCell GetControlledCell()
	{
		return controlledCell;
	}

	public Transform GetControlledCellTransform()
	{
		if (controlledCell == null)
			return null;
		else return controlledCell.transform;
	}

	// IAIObjectiveHolder
	public GameObject GetTarget()
	{
		return target;
	}

	public void SetTarget(GameObject target)
	{
		this.target = target;
	}

	public Transform GetTargetTransform()
	{
		if (target == null)
			return null;
		else return target.transform;
	}
}
